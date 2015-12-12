using UnityEngine;
using System.Collections;

public class ColorScreen : MonoBehaviour {
    public static ColorScreen instance;
    
    Texture2D screenTexture;
    GUIStyle screenStyle;
    public Color screenColor;
    public float screenDuration = 0.1f;
    
    void Awake() {
        instance = this;
    }
    
    public void Start() {
        screenTexture = new Texture2D(1, 1);
        screenStyle = new GUIStyle();
    }
    
    public void MakeColor(Color color, float duration) {
        screenColor = color;
        screenDuration = duration;
        StartCoroutine(MakeColorProcess());
    }
    
    IEnumerator MakeColorProcess() {
        screenTexture.SetPixel(0, 0, screenColor);
        screenTexture.wrapMode = TextureWrapMode.Repeat;
        screenTexture.Apply();
        screenStyle.normal.background = screenTexture;
        yield return new WaitForSeconds(screenDuration);
        screenTexture.SetPixel(0, 0, Color.clear);
        screenTexture.Apply();
    }
    
    void OnGUI() {
        GUI.Label(new Rect(0f, 0f, Screen.width, Screen.height), 
                  screenTexture, screenStyle);
    }
}
