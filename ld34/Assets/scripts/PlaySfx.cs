using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class PlaySfx : MonoBehaviour {
    public static PlaySfx inst;
    AudioSource sfxSource;
    float defPitch;
    
    void Awake() {
        inst = this;
        sfxSource = GetComponent<AudioSource>();
        defPitch = sfxSource.pitch;
    }
    
    public void PlaySound(AudioClip soundClip) {
        sfxSource.pitch = defPitch * Random.Range(0.8f, 1.1f); 
        sfxSource.PlayOneShot(soundClip);
    }
}
