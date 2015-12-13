using UnityEngine;
using System.Collections;

public class Explosion : MonoBehaviour {
	public SpriteRenderer sr;
    public Color[] animColors;
    public Color[] expColors;
    public bool oneHitAnim = false;
    public float animDuration = 1f;
    public float animSpeed = 12;

    IEnumerator Animate() {
        var i = 0;
        var animTimer = 0f;
        while (animTimer <= animDuration) {
            sr.color = animColors[i++%animColors.Length];
            ColorScreen.instance.MakeColor(expColors[i++%expColors.Length], Time.fixedDeltaTime);
            yield return new WaitForSeconds(1f/animSpeed);
            animTimer += 1f / animSpeed;
        }
        gameObject.SetActive(false);
    }

    public void Init(Vector3 pos) {
        transform.position = pos;
        StartCoroutine(Animate());
    }
}
