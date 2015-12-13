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
    public int cameraValue = 1;

    protected Vector3 size;

    protected void Start() {
        size = GetComponent<Collider2D>().bounds.extents;
        health = maxHealth;
    }

    public virtual void init(Vector3 initPos, Vector3 initDir) {
        health = maxHealth;
    }

    public virtual void Damage(float value) {
        health -= value;
        ColorScreen.instance.MakeColor(Color.red, 0.1f);
        if (health <= 0) {
            gameObject.SetActive(false);
        }
    }
}
