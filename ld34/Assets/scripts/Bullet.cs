using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {
    protected Vector3 velocity;
    public float moveSpeed = 7.88f;
    public float damageValue = 1;
    protected RaycastHit2D 
        obstacleHit,
        damageHit;
    protected Vector3 size;
    public LayerMask 
        obstacleMask = 1 << 8;
    Character owner;
    public float reloadingSpeed = 0.35f;

    protected void Start() {
        size = GetComponent<Collider2D>().bounds.extents;
    }

    public virtual void init(Vector3 pos, Vector3 dir, Character owner) {
        transform.position = pos;
        velocity = dir.normalized * moveSpeed;
        this.owner = owner;
    }

    protected void FixedUpdate() {
        if (checkObstacle()) {
            onHit();
        }
        if (checkDamage()) {
            damageHit.collider.GetComponent<Character>().Damage(damageValue);
            onHit();
        }
        
        if (transform.position.x < Level.inst.lbCorner.position.x 
            || transform.position.x > Level.inst.rtCorner.position.x
            || transform.position.y < Level.inst.lbCorner.position.y
            || transform.position.y > Level.inst.rtCorner.position.y) {
                gameObject.SetActive(false);
            }
    }

    protected void Update() {
        var angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
            
        transform.position += velocity * Time.deltaTime;
    }

    protected virtual bool checkObstacle() {
        obstacleHit = Physics2D.Raycast(transform.position, 
                                        velocity, 
                                        size.x, 
                                        obstacleMask);
        return obstacleHit;
    }

    protected virtual bool checkDamage() {
        damageHit = Physics2D.Raycast(transform.position, 
                                      velocity, 
                                      size.x, 
                                      owner.enemyMask);
        return damageHit;
    }

    public Color hitColor;
    
    protected virtual void onHit() {
        ColorScreen.instance.MakeColor(hitColor, 0.01f);
        gameObject.SetActive(false);
    }
}
