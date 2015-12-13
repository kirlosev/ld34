using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Boss : Character {
    Vector3 velocity;
    public BossGun[] bossGuns;
    int unactiveGunCount;
    public BossShield shield;

    public float moveSpeed = 15;
    public float changeDirSpeed = 15;

    public bool 
        canHitShield = false,
        canHitCore = false;

    public Transform[] birdOutputShield;
    public Transform[] birdOutputCore;
    public Transform[] jetOutputCore;
    public Transform[] barrelLaser;
    public Color targetColor;

    public bool hasBirds {
        get { 
            return checkActiveBirds() != 0 || checkActiveJets() != 0;
        }
    }

    void Start() {
        base.Start();
        unactiveGunCount = 0;
        StartCoroutine(shieldBird());
        StartCoroutine(coreBird());
        StartCoroutine(coreJet());
        StartCoroutine(shootLaser());
    }

    void Update() {
        if (!Game.inst.gameStarted || Game.inst.gameEnded)
            return;

        var midHeightDir = (Level.inst.rtCorner.position.y - Level.inst.lbCorner.position.y) / 2;
        transform.position += velocity *2* Time.deltaTime;
    }

    void FixedUpdate() {
        if (!Game.inst.gameStarted || Game.inst.gameEnded)
            return;
        
        var plDir = Player.inst.transform.position - transform.position;
        velocity += plDir * changeDirSpeed * Time.deltaTime;
        if (velocity.magnitude > moveSpeed) {
            velocity = velocity.normalized * moveSpeed;
        }

        unactiveGunCount = 0;
        if (!canHitShield) {
            for (var i = 0; i < bossGuns.Length; ++i) {
                if (!bossGuns[i].gameObject.activeInHierarchy) {
                    unactiveGunCount++;
                }
            }
        }
        if (unactiveGunCount == bossGuns.Length && !canHitShield) {
            canHitShield = true;
            shield.setColor(targetColor);
        }
        if (!shield.gameObject.activeInHierarchy && !canHitCore) {
            GetComponent<SpriteRenderer>().color = targetColor;
            canHitCore = true;
        }
    }

    public override void Damage(float value) {
        if (!canHitCore) return;

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

    List<Character> birds = new List<Character>();
    IEnumerator shieldBird() {
        while (shield.gameObject.activeInHierarchy) {
            if (checkActiveBirds() < 100) {
                var b = ObjPool.inst.getBird();
                var barrelInd = (Random.value) > 0.5f ? 0 : 1;
                b.init(birdOutputShield[barrelInd].position, Vector3.right * birdOutputShield[barrelInd].localPosition.x);
                birds.Add(b);
            }
            yield return new WaitForSeconds(canHitShield ? 0.3f : 0.7f);
        }
        birds.Clear();
    }

    IEnumerator coreBird() {
        while (gameObject.activeInHierarchy) {
            if (!canHitCore) {
                yield return null;
                continue;
            }
            if (checkActiveBirds() < 50) {
                var b = ObjPool.inst.getBird();
                var barrelInd = (Random.value) > 0.5f ? 0 : 1;
                b.init(birdOutputShield[barrelInd].position, Vector3.right * birdOutputShield[barrelInd].localPosition.x);
                birds.Add(b);
            }
            yield return new WaitForSeconds(0.37f);
        }
        birds.Clear();
    }

    List<Character> jets = new List<Character>();
    IEnumerator coreJet() {
        while (gameObject.activeInHierarchy) {
            if (!canHitCore) {
                yield return null;
                continue;
            }
            if (checkActiveJets() < 2) {
                var b = ObjPool.inst.getJet();
                var barrelInd = (Random.value) > 0.5f ? 0 : 1;
                b.init(jetOutputCore[barrelInd].position, Vector3.right * jetOutputCore[barrelInd].localPosition.x);
                jets.Add(b);
            }
            yield return new WaitForSeconds(3.4f);
        }
        jets.Clear();
    }

    IEnumerator shootLaser() {
        var ch = GetComponent<Character>();
        while (gameObject.activeInHierarchy) {
            if (!canHitCore) {
                yield return null;
                continue;
            }
            var b = ObjPool.inst.getLaser();
            var barrelInd = Random.Range(0, barrelLaser.Length);
            b.init(barrelLaser[barrelInd].position, barrelLaser[barrelInd].localPosition, ch);
            yield return new WaitForSeconds(Random.value);
        }
    }

    int checkActiveBirds() {
        int count = 0;
        for (var i = 0; i < birds.Count; ) {
            if (birds[i].gameObject.activeInHierarchy) {
                count++;
                ++i;
            } else {
                birds.RemoveAt(i);
            }
        }
        return count;
    }

    int checkActiveJets() {
        int count = 0;
        for (var i = 0; i < jets.Count; ) {
            if (jets[i].gameObject.activeInHierarchy) {
                count++;
                ++i;
            } else {
                jets.RemoveAt(i);
            }
        }
        return count;
    }
}
