using UnityEngine;
using System.Collections;

public class Character : MonoBehaviour {
    public LayerMask enemyMask;
    protected float health;
    public float maxHealth = 10;
    public bool isAlive {
        get {
            return health > 0;
        }
    }

    protected Vector3 size;

    protected void Start() {
        size = GetComponent<Collider2D>().bounds.extents;
        Init();
    }

    public void Init() {
        health = maxHealth;
    }

    public virtual void Damage(float value) {
        health -= value;
        if (health <= 0) {
            gameObject.SetActive(false);
        }
    }
}
