using UnityEngine;
using System.Collections;

public class Player : Character {
    public static Player inst;

    public Transform wings;
    public PlayerInput input;
    Vector3 velocity;
    
    public float defMoveSpeed = 4f;
    public float defRotSpeed = 4f;
    float maxMoveSpeed;
    public float slowDownSpeed = 6f;
    
    public float gravity = -9f;

    Character character;
    
    public float startRotTime;
    
    Vector3 dirInput;
    float startDirInput;
    public float deltaAngle = 5;
    public float defDeltaAngle = 5;
    public float maxDeltaAngle = 20;
    public float gunForce = 6.78f;
    
    public LayerMask obstacleMask = 1 << 8;
    
    protected void Awake() {
        inst = this;
        character = GetComponent<Character>();
    }

    void Start() {
        maxMoveSpeed = defMoveSpeed * 1.32f;
        StartCoroutine(Shoot());
    }

    void Update() {
        if (Input.GetButtonUp("ChangeDirection")) {
            dirInput = velocity;
            startDirInput = Time.time;
            deltaAngle = defDeltaAngle;
        } 
        if (!Input.GetButton("ChangeDirection") && !input.shoot) {
            float dirAngle = Mathf.Atan2(dirInput.y, dirInput.x) * Mathf.Rad2Deg;
            dirAngle += deltaAngle;
            deltaAngle += (Time.time - startDirInput)/10;
            deltaAngle = Mathf.Clamp(deltaAngle, defDeltaAngle, maxDeltaAngle);
            dirInput.x = Mathf.Cos(dirAngle * Mathf.Deg2Rad);
            dirInput.y = Mathf.Sin(dirAngle * Mathf.Deg2Rad);
            transform.rotation = Quaternion.Euler(0f, 0f, dirAngle);
        }
        Debug.DrawRay(transform.position, dirInput, Color.cyan);
        
        var angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;
        /*
        if (input.engineForce == 0) {
            angle += deltaAngle;
            transform.rotation = Quaternion.Slerp(transform.rotation, 
                                                  Quaternion.Euler(0f, 0f, angle), 
                                                  Time.deltaTime);
        } else {
            startRotTime = Time.time;
            transform.rotation = Quaternion.Euler(0f, 0f, angle);
        }
        */
        
        velocity += dirInput * 50 * input.engineForce * Time.deltaTime;
        
        RaycastHit2D checkGround = Physics2D.Raycast(transform.position, velocity, size.x * 4, obstacleMask);
        
        if (transform.position.y < 0) {
            velocity += Vector3.up *velocity.magnitude *10* Time.deltaTime;
        }
        
        Debug.DrawRay(transform.position, velocity, Color.magenta);
        if (velocity.magnitude > maxMoveSpeed)
            velocity = velocity.normalized * maxMoveSpeed;
        if (Input.GetButton("ChangeDirection")) {
            transform.rotation = Quaternion.Euler(0f, 0f, angle);
        }
        wings.localScale = Vector3.right + Vector3.up * Mathf.Clamp(Mathf.Abs(transform.right.y), 0, 1);
        transform.position += velocity * Time.deltaTime;
        if (!Input.GetButton("ChangeDirection")) {
            velocity += -velocity * slowDownSpeed * Time.deltaTime;
            velocity.y += gravity * Time.deltaTime;
        }
 /*       
        if (transform.position.x < Level.inst.lbCorner.position.x ||
            transform.position.x > Level.inst.rtCorner.position.x ||
            transform.position.y < Level.inst.lbCorner.position.y ||
            transform.position.y > Level.inst.rtCorner.position.y) {
            transform.position *= -1;
            transform.position += velocity * 0.3f;
        }
*/        
    }

    IEnumerator Shoot() {
        while (true) {
            if (input.shoot) {
                var bullet = ObjPool.inst.getBullet();
                Vector3 dir = transform.right + transform.up * Random.Range(-0.2f, 0.2f);
                bullet.init(transform.position, dir, character);
                CameraFollow.inst.Shake(0.6f, 0.1f, -dir);
                velocity += -dir *gunForce* Time.deltaTime;
                yield return new WaitForSeconds(bullet.reloadingSpeed);
            } else {
                yield return null;
            }
        }
    }
}

// reflection: -2 * Vector3.Dot(velocity, obstacleHit.normal) * (Vector3)obstacleHit.normal + velocity
