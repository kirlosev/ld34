using UnityEngine;
using System.Collections;

public class Prop : MonoBehaviour {
    public Sprite[] variants;

    public void init() {
        var index = Random.Range(0, variants.Length);
        GetComponent<SpriteRenderer>().sprite = variants[index];
    }
}
