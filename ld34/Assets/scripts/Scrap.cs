using UnityEngine;
using System.Collections;

public class Scrap : MonoBehaviour {
    public Sprite[] scrap;
    SpriteRenderer sr;
    
    void Awake() {
        sr = GetComponent<SpriteRenderer>();
    }
    
    public void init() {
        int index = Random.Range(0, scrap.Length);
        sr.sprite = scrap[index];
    }
}
