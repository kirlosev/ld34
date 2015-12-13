using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Highscore : MonoBehaviour {
    public static Highscore inst;
    public Text scoreText;
    public int currScore = 0;

    float lastTime;
    public float comboLimitTime = 1;
    int combo = 1;

    void Awake() {
        inst = this;
    }

    void Start() {
        currScore = 0;
        lastTime = Time.time;
    }

    public void AddPoint(int amount) {
        if (Time.time - lastTime <= comboLimitTime) {
            combo += 1;
        } else {
            combo = 1;
        }
        amount *= combo;
        lastTime = Time.time;
        currScore += amount;
    }

    void FixedUpdate() {
        if (Time.time - lastTime > comboLimitTime) {
            combo = 1;
        }
        scoreText.text = currScore.ToString()+ ((combo > 1) ? ("x"+combo.ToString()) : "");
    }
}
