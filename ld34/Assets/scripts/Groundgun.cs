using UnityEngine;
using System.Collections;

public class Groundgun : Character {
    public Transform[] gunCont;
    public float rotateSpeed = 10;
    Character ch;
    public float attackRange = 10;

    void Awake() {
        ch = GetComponent<Character>();
    }

    void Start() {
        base.Start();
        StartCoroutine(shoot());
    }

    void Update() {
        for (var i = 0; i < gunCont.Length; ++i) {
            var playerDir = Player.inst.transform.position - gunCont[i].transform.position;
            var targAngle = Mathf.Atan2(playerDir.y, playerDir.x) * Mathf.Rad2Deg;
            if (targAngle < 0)
                targAngle += 360;
            var currAngle = Mathf.Atan2(gunCont[i].transform.right.y, 
                gunCont[i].transform.right.x)
                * Mathf.Rad2Deg;
            if (currAngle < 0)
                currAngle += 360;
            var deltaAngle = targAngle - currAngle;
            if (Mathf.Abs(deltaAngle) > 180) {
                deltaAngle += 360 * Mathf.Sign(deltaAngle) * -1;
            }
            var angle = currAngle + deltaAngle * Time.fixedDeltaTime * rotateSpeed;
            gunCont[i].transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }

    IEnumerator shoot() {
        while (true) {
            if (!Game.inst.gameStarted || Game.inst.gameEnded) {
                yield return null;
                continue;
            }
            for (var i = 0; i < gunCont.Length; ++i) {
                var playerDir = Player.inst.transform.position - gunCont[i].transform.position;
                if (playerDir.magnitude < attackRange) {
                    var bullet = ObjPool.inst.getLaser();
                    bullet.init(transform.position, gunCont[i].transform.right, ch);
                }
                yield return new WaitForSeconds(Random.value);
            }
            yield return null;
        }
    }

    public override void Damage(float value) {
        health -= value;
        ColorScreen.instance.MakeColor(Color.red, 0.1f);
        if (health <= 0) {
            Highscore.inst.AddPoint(1000);

            var expMan = ObjPool.inst.getExplosionManager();
            expMan.Init(transform.position);

            gameObject.SetActive(false);
        }
    }
}
