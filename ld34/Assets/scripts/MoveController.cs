using UnityEngine;
using System.Collections;

public class MoveController : MonoBehaviour {
    public int horzRayAmount = 4;
    public int vertRayAmount = 4;

    float horzRaySpacing = 0;
    float vertRaySpacing = 0;

    float skinWidth = 0.015f;

    Collider2D col;
    Vector3 colliderSize;
    bool onGround = false;
    Vector3 moveDirection;

    public LayerMask groundLayer;

    public CollisionInfo collisions;
    public struct CollisionInfo {
        public bool above, below;
        public bool left, right;
        public bool any {
            get {
                return above || below || left || right;
            }
        }
        public int faceDir;
        public RaycastHit2D collInfo;

        public void Reset() {
            above = below = false;
            left = right = false;
        }
    }

    RaycastOrigins raycastOrigins;
    struct RaycastOrigins {
        public Vector3 topLeft, topRight;
        public Vector3 bottomLeft, bottomRight;
    }

    void Awake() {
        col = GetComponent<Collider2D>();
        colliderSize = col.bounds.extents;
        CalcRaySpacing();
        collisions.faceDir = 1;
    }

    public void MoveDeltaPosition(Vector3 deltaPos) {
        UpdateRaycastOrigins();
        collisions.Reset();

        if (deltaPos.y != 0) {
            CheckVerticalCollision(ref deltaPos);
        }

        if (deltaPos.x != 0) {
            collisions.faceDir = (int)Mathf.Sign(deltaPos.x);
        }
        CheckHorizontalCollision(ref deltaPos);

        transform.Translate(deltaPos);
    }

    void CheckVerticalCollision(ref Vector3 velocity) {
        moveDirection.y = Mathf.Sign(velocity.y);
        var rayLength = Mathf.Abs(velocity.y) + skinWidth;
        for (var i = 0; i < vertRayAmount; ++i) {
            var outPos = (moveDirection.y == -1) ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;
            outPos += Vector3.right * (vertRaySpacing * i);
            var hit = Physics2D.Raycast(outPos, Vector3.up * moveDirection.y, rayLength, groundLayer);

            Debug.DrawRay(outPos, Vector3.up * moveDirection.y * rayLength, hit?Color.red:Color.blue);

            if (hit) {
                velocity.y = (hit.distance - skinWidth) * moveDirection.y;
                rayLength = hit.distance;
                collisions.above = moveDirection.y == 1;
                collisions.below = moveDirection.y == -1;
                collisions.collInfo = hit;
            }
        }
    }

    void CheckHorizontalCollision(ref Vector3 velocity) {
        moveDirection.x = collisions.faceDir;
        var rayLength = Mathf.Abs(velocity.x) + skinWidth;

        if (Mathf.Abs(velocity.x) < skinWidth) {
            rayLength = 2*skinWidth;
        }

        for (var i = 0; i < horzRayAmount; ++i) {
            var outPos = (moveDirection.x == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;
            outPos += Vector3.up * (horzRaySpacing * i);

            var hit = Physics2D.Raycast(outPos, Vector3.right * moveDirection.x, rayLength, groundLayer);

            Debug.DrawRay(outPos, Vector3.right * moveDirection.x * rayLength, hit?Color.red:Color.blue);

            if (hit) {
                velocity.x = (hit.distance - skinWidth) * moveDirection.x;
                rayLength = hit.distance;
                collisions.right = moveDirection.x == 1;
                collisions.left = moveDirection.x == -1;
                collisions.collInfo = hit;
            }
        }
    }

    void UpdateRaycastOrigins() {
        var bounds = col.bounds;
        bounds.Expand(skinWidth * -2);

        raycastOrigins.bottomLeft  = new Vector3(bounds.min.x, bounds.min.y);
        raycastOrigins.bottomRight = new Vector3(bounds.max.x, bounds.min.y);
        raycastOrigins.topLeft     = new Vector3(bounds.min.x, bounds.max.y);
        raycastOrigins.topRight    = new Vector3(bounds.max.x, bounds.max.y);
    }

    void CalcRaySpacing() {
        var bounds = col.bounds;
        bounds.Expand(skinWidth * -2);

        horzRayAmount = (int)Mathf.Clamp(horzRayAmount, 2, int.MaxValue);
        vertRayAmount = (int)Mathf.Clamp(vertRayAmount, 2, int.MaxValue);

        horzRaySpacing = bounds.size.y / (horzRayAmount - 1);
        vertRaySpacing = bounds.size.x / (vertRayAmount - 1);
    }
}
