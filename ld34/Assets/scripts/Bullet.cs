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

    protected void Start() {
        size = GetComponent<Collider2D>().bounds.extents;
    }

    public virtual void init(Vector3 pos, Vector3 dir, Character owner) {
        transform.position = pos;
        velocity = dir * moveSpeed;
        this.owner = owner;
    }

    protected void FixedUpdate() {
        if (checkObstacle()) {
            onHit();
        }
        if (checkDamage()) {
            damageHit.collider.GetComponent<Character>().damage(damageValue);
            onHit();
        }
    }

    protected void Update() {
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

    protected virtual void onHit() {
        gameObject.SetActive(false);
    }
}
