using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Game : MonoBehaviour {
    public static Game inst;

    public float animateDur = 1;

    public bool gameStarted = false;
    public Transform[] titleObjects;

    public bool gameEnded = false;
    public Transform[] endObjects;
    public Text scoreText;

    void Awake() {
        inst = this;
    }

    void Start() {
        gameStarted = false;
        StartCoroutine(waitForStart());
    }

    IEnumerator waitForStart() {
        for (int i = 0; i < titleObjects.Length; ++i) {
            titleObjects[i].gameObject.SetActive(true);
        }

        while (true) {
            if (Input.GetButtonDown("ChangeDirection") 
                || Input.GetButtonDown("Action")) {
                StartCoroutine(animateStart());
                yield break;
            }
            yield return null;
        }
    }

    IEnumerator animateStart() {
        float timer = 0;
        while (timer < animateDur) {
            for (int i = 0; i < titleObjects.Length; ++i) {
                titleObjects[i].transform.position += Vector3.up * Screen.height * 2
                                                   * timer / animateDur 
                                                   * Time.deltaTime;
            }
            timer += Time.deltaTime;
            yield return null;
        }
        gameStarted = true;
        for (int i = 0; i < titleObjects.Length; ++i) {
            titleObjects[i].gameObject.SetActive(false);
        }
    }

    public void endGame(bool success, int score) {
        gameEnded = true;

        if (success) {
            int highscore = score;
            if (!PlayerPrefs.HasKey("Highscore")) {
                PlayerPrefs.SetInt("Highscore", score);
            } else {
                highscore = PlayerPrefs.GetInt("Highscore");
            }
            if (score > highscore) {
                PlayerPrefs.SetInt("Highscore", score);
                highscore = score;
            }
            scoreText.text = "VICTORY!\nSCORE: " + score + "\nMAX: " + highscore;
        } else {
            scoreText.text = "YOU FAILED!";
        }

        StartCoroutine(animateEnd());
    }

    IEnumerator animateEnd() {
        for (int i = 0; i < endObjects.Length; ++i) {
            endObjects[i].gameObject.SetActive(true);
        }

        for (int i = 0; i < endObjects.Length; ++i) {
            endObjects[i].transform.position += Vector3.up * Screen.height;
        }
        float timer = 0;
        while (timer < animateDur) {
            for (int i = 0; i < endObjects.Length; ++i) {
                endObjects[i].transform.position += -Vector3.up * Screen.height * 2
                    * timer / animateDur 
                    * Time.deltaTime;
            }
            timer += Time.deltaTime;
            yield return null;
        }
        StartCoroutine(waitForRestart());
    }

    IEnumerator waitForRestart() {
        for (int i = 0; i < titleObjects.Length; ++i) {
            titleObjects[i].gameObject.SetActive(true);
        }

        while (true) {
            if (Input.GetButtonDown("ChangeDirection") 
                || Input.GetButtonDown("Action")) {
                Application.LoadLevel(Application.loadedLevel);
                yield break;
            }
            yield return null;
        }
    }
}
