using UnityEngine;
using System.Collections;

public class Character : MonoBehaviour {
    protected Character character;
    float health;
    public float maxHealth = 100;
    public LayerMask enemyMask = 1 << 10;
    public LayerMask obstacleMask = 1 << 8;
    public float moveSpeed = 3.75f;
    protected RaycastHit2D obstacleHit;
    protected Vector3 size;

    protected void Awake() {
        character = GetComponent<Character>();
    }

    protected void Start() {
        health = maxHealth;
        size = GetComponent<Collider2D>().bounds.extents;
    }

    public void damage(float damageVal) {
        health -= damageVal;
        if (health <= 0) {
            // TODO : game over
            Debug.Log("game over");
        }
    }
}
