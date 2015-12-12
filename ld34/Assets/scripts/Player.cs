using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
    Vector3 velocity;
    public float moveSpeed = 3.75f;
    RaycastHit2D obstacleHit;
    Vector3 size;
    public LayerMask obstacleMask = 1 << 8;
    Vector3 dirInput;
    float startDirInput;
    public float deltaAngle = 5;
    public float defDeltaAngle = 5;
    public float maxDeltaAngle = 20;

    void Start() {
        size = GetComponent<Collider2D>().bounds.extents;
        velocity = Random.insideUnitCircle.normalized * moveSpeed;
    }

    void Update() {
        if (Input.GetButtonDown("ChangeDirection")) {
            dirInput = velocity;
            startDirInput = Time.time;
            deltaAngle = defDeltaAngle;
        } else if (Input.GetButtonUp("ChangeDirection")) {
            velocity = dirInput * moveSpeed;
        }
        if (Input.GetButton("ChangeDirection")) {
            float dirAngle = Mathf.Atan2(dirInput.y, dirInput.x) * Mathf.Rad2Deg;
            dirAngle += deltaAngle;
            deltaAngle += (Time.time - startDirInput)/10;
            deltaAngle = Mathf.Clamp(deltaAngle, defDeltaAngle, maxDeltaAngle);
            dirInput.x = Mathf.Cos(dirAngle * Mathf.Deg2Rad);
            dirInput.y = Mathf.Sin(dirAngle * Mathf.Deg2Rad);
            Debug.DrawRay(transform.position, dirInput, Color.cyan);
        }

        if (checkCollision()) {
            velocity = -2 * Vector3.Dot(velocity, obstacleHit.normal) 
                       * (Vector3)obstacleHit.normal + velocity;
        }
        transform.position += velocity * Time.deltaTime;
    }

    bool checkCollision() {
        obstacleHit = Physics2D.Raycast(transform.position, 
                                        velocity, 
                                        size.x, 
                                        obstacleMask);
        return obstacleHit;
    }
}
