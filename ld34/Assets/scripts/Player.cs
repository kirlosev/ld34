using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MoveController))]
public class Player : Character {
    public static Player inst;

    Vector3 velocity;
    Vector3 dirInput;
    float startDirInput;
    float deltaAngle = 5;
    public float defDeltaAngle = 5;
    public float maxDeltaAngle = 20;

    MoveController move;

    protected void Awake() {
        base.Awake();
        inst = this;
        move = GetComponent<MoveController>();
    }

    protected void Start() {
        base.Start();
        deltaAngle = defDeltaAngle; 
        velocity = Random.insideUnitCircle.normalized * moveSpeed;
    }

    void Update() {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        Vector3 mouseDir = mousePos - transform.position;

        if (move.collisions.any) {
            Vector3 normalPerp = new Vector3(move.collisions.collInfo.normal.y, 
                                            -move.collisions.collInfo.normal.x);
            float normalDot = Vector3.Dot(velocity, normalPerp);
            if (normalDot < 0) normalPerp *= -1;
            velocity = normalPerp * moveSpeed;

            if (Input.GetButtonDown("ChangeDirection")) {
                velocity = mouseDir.normalized * moveSpeed;
            }
        }

        /*
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
        */
        if (Input.GetButtonDown("Action")) {
            Bullet bullet = ObjPool.inst.getBullet();
            bullet.init(transform.position, mouseDir, character);
        }

        move.MoveDeltaPosition(velocity * Time.deltaTime);
    }
}

// reflection: -2 * Vector3.Dot(velocity, obstacleHit.normal) * (Vector3)obstacleHit.normal + velocity
