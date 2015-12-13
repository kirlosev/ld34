using UnityEngine;
using System.Collections;

public class BossWave : Wave {
    public Boss bossInst;
    Boss boss;

    public override bool isActive() {
        if (boss.hasBirds) return true;
        return boss.gameObject.activeInHierarchy;
    }

    public override void init() {
        for (var j = 0; j < initPos.Length; ++j) {
            boss = Instantiate(bossInst, initPos[0].position, Quaternion.identity) as Boss;
            boss.init(Vector3.zero, Vector3.zero);
        }
    }
}
