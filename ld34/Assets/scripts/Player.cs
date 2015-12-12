using UnityEngine;
using System.Collections;

public class Player : Character {
    public static Player inst;

    Vector3 velocity;
    Vector3 dirInput;
    float startDirInput;
    public float deltaAngle = 5;
    public float defDeltaAngle = 5;
    public float maxDeltaAngle = 20;

    protected void Awake() {
        base.Awake();
        inst = this;
    }

    protected void Start() {
        base.Start();
        velocity = Random.insideUnitCircle.normalized * moveSpeed;
    }

    void Update() {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        Vector3 mouseDir = mousePos - transform.position;
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
        if (Input.GetButtonDown("Action")) {
            Bullet bullet = ObjPool.inst.getBullet();
            bullet.init(transform.position, mouseDir, character);
        }

        transform.position += velocity * Time.deltaTime;
    }

    void FixedUpdate() {
        if (checkCollision()) {
            float deltaDist = size.x - obstacleHit.distance;
            transform.position += (Vector3)obstacleHit.normal * deltaDist
                               * Time.fixedDeltaTime * 10;

            Vector3 normalPerp = new Vector3(obstacleHit.normal.y, 
                                            -obstacleHit.normal.x);
            float normalDot = Vector3.Dot(velocity, normalPerp);
            if (normalDot < 0) normalPerp *= -1;
            velocity = normalPerp * moveSpeed;
        }
    }

    bool checkCollision() {
        obstacleHit = Physics2D.Raycast(transform.position, 
                                        velocity, 
                                        size.x, 
                                        obstacleMask);
        return obstacleHit;
    }
}

// reflection: -2 * Vector3.Dot(velocity, obstacleHit.normal) * (Vector3)obstacleHit.normal + velocity
