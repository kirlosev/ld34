using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class WaveManager : MonoBehaviour {
    public Wave[] waveQueue;
    int currWave = 0;
    bool playerWin = false;
    public Jet jetInst;
    List<Jet> jetCont = new List<Jet>();
    int prevJetWave = 0;
    public Text waveNameText;
    
    void Start() {
        currWave = 0;
        playerWin = false;
        StartCoroutine(waveSwitcher());
        StartCoroutine(checkJets());
    }

    IEnumerator waveSwitcher() {
        while (!playerWin) {
            while (!Game.inst.gameStarted) {
                yield return new WaitForFixedUpdate();
            }

            waveNameText.text = waveQueue[currWave].waveName;
            waveNameText.gameObject.SetActive(true);
            for (var i = 0; i < 15; ++i) {
                waveNameText.gameObject.SetActive(!waveNameText.gameObject.activeInHierarchy);
                yield return new WaitForSeconds(0.21f);
            }
            waveNameText.gameObject.SetActive(false);
            waveQueue[currWave].init();
            while (waveQueue[currWave].isActive()) {
                if (currWave > 1) {
                    if (Random.value > 0.9f && jetCont.Count < 2 && prevJetWave != currWave) {
                        var fromUp = Random.value > 0.5f;
                        var fromRight = Random.value > 0.5f;
                        var pos = new Vector3(fromRight ? Level.inst.rtCorner.position.x : Level.inst.lbCorner.position.x, 
                                              fromUp ? Level.inst.rtCorner.position.y : Level.inst.lbCorner.position.y);
                        var j = ObjPool.inst.getJet();
                        j.init(Vector3.zero, Vector3.zero);
                        jetCont.Add(j);
                    } else if (jetCont.Count >= 2) {
                        prevJetWave = currWave;
                    }
                }
                yield return new WaitForFixedUpdate();
            }
            currWave++;
            if (currWave >= waveQueue.Length) {
                Game.inst.endGame(true, Highscore.inst.currScore);
                playerWin = true;
            } 
            yield return null;
        }
    }

    IEnumerator checkJets() {
        while (true) {
            for (var i = 0; i < jetCont.Count; ) {
                if (!jetCont[i].gameObject.activeInHierarchy) {
                    jetCont.RemoveAt(i);
                } else {
                    ++i;
                }
            }
            yield return new WaitForSeconds(1);
        }
    }
}
