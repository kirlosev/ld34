using UnityEngine;
using System.Collections;

public class WaveManager : MonoBehaviour {
    public Wave[] waveQueue;
    int currWave = 0;
    bool playerWin = false;
    
    void Start() {
        waveQueue[0].init();
        currWave = 0;
        playerWin = false;
    }
    
    void FixedUpdate() {
        if (playerWin) return;
        if (!waveQueue[currWave].isActive) {
            currWave++;
            if (currWave < waveQueue.Length) {
                Debug.Log("wave #"+currWave);
                waveQueue[currWave].init();
            } else {
                Debug.Log("you win!");
                playerWin = true;
            }
        }
    }
}
