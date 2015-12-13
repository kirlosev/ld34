using UnityEngine;
using System.Collections;

public class BossShield : Character {
    public Boss boss;

    public override void Damage(float value) {
        if (!boss.canHitShield) return;

        health -= value;
        ColorScreen.instance.MakeColor(Color.red, 0.1f);
        if (health <= 0) {
            Highscore.inst.AddPoint(100);

            var amountOfExp = Random.Range(6, 10);

            for (var i = 0; i < amountOfExp; ++i) {
                var expMan = ObjPool.inst.getExplosionManager();
                expMan.Init(transform.position + (Vector3)Random.insideUnitCircle * size.x);
            }

            StartCoroutine(blink());
        }
    }

    IEnumerator blink() {
        var amount = Random.Range(4, 8);
        var sr = GetComponent<SpriteRenderer>();
        for (var i = 0; i < amount; ++i) {
            sr.enabled = !sr.enabled;
            yield return new WaitForSeconds(Random.value);
        }
        gameObject.SetActive(false);
    }

    public void setColor(Color color) {
        GetComponent<SpriteRenderer>().color = color;
    }
}
