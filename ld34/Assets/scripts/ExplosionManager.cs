using UnityEngine;
using System.Collections;

public class ExplosionManager : MonoBehaviour {
    public AudioClip sound;
    public float explosionRadius = 1f;
    public float damageValue = 2f;
    public LayerMask birdLayer;

    public void Init(Vector3 pos) {
        transform.position = pos;
        PlaySfx.inst.PlaySound(sound);
        var cols = Physics2D.OverlapCircleAll(transform.position, explosionRadius, birdLayer);
        for (var i = 0; i < cols.Length; ++i) {
            cols[i].GetComponent<Character>().Damage(damageValue);
        }
        StartCoroutine(Explode());
    }

    IEnumerator Explode() {
        CameraFollow.inst.Shake(0.4f, 0.1f, Vector3.zero);
        for (var i = 0; i < 5; ++i) {
            var exp = ObjPool.inst.getExplosion();
            exp.gameObject.SetActive(true);
            exp.Init(transform.position + (Vector3)Random.insideUnitCircle * explosionRadius);
            yield return new WaitForFixedUpdate();
        }
    }
}
