using UnityEngine;
using System.Collections;

public class BossGun : Character {
    void Start() {
        base.Start();
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
