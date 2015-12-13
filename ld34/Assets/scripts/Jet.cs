using UnityEngine;
using System.Collections;

public class Jet : Character {
    public Transform wings;

    public float moveSpeed = 15;
    public float changeDirSpeed = 15;
    Vector3 velocity;

    bool canAttack = true;
    public float attackReloadDur = 1;
    public float attackRadius;
    public LayerMask playerLayer = 1 << 10;

    void Start() {
        base.Start();
        moveSpeed += Random.value;
        attackRadius = size.x;
        velocity = Player.inst.transform.position - transform.position;
    }

    void Update() {
        if (!Game.inst.gameStarted || Game.inst.gameEnded)
            return;
        var angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
        wings.localScale = Vector3.right + Vector3.up * Mathf.Clamp(Mathf.Abs(transform.right.y), 0, 1);
        transform.position += velocity *moveSpeed* Time.deltaTime;
    }

    void FixedUpdate() {
        if (!Game.inst.gameStarted || Game.inst.gameEnded)
            return;

        var plDir = Player.inst.transform.position - transform.position;
        velocity += plDir * changeDirSpeed * Time.deltaTime;
        if (velocity.magnitude > moveSpeed) {
            velocity = velocity.normalized * moveSpeed;
        }
        CheckPlayer(); 
    }

    void CheckPlayer() {
        var playerAround = Physics2D.OverlapCircle(transform.position, attackRadius, playerLayer);
        if (playerAround && canAttack && Random.value > 0.7f) {
            Player.inst.Damage(1);
            StartCoroutine(Reload());
        }
    }

    IEnumerator Reload() {
        canAttack = false;
        yield return new WaitForSeconds(attackReloadDur);
        canAttack = true;
    }

    public override void Damage(float value) {
        health -= value;
        ColorScreen.instance.MakeColor(Color.red, 0.1f);
        if (health <= 0) {
            Highscore.inst.AddPoint(100);

            var expMan = ObjPool.inst.getExplosionManager();
            expMan.Init(transform.position);

            gameObject.SetActive(false);
        }
    }
}
