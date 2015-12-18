using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
[AddComponentMenu("Image Effects/PixelBoy")]
public class PixelBoy : MonoBehaviour {
    public int w = 720;
    public int h = 640;

    protected void Start() {
        if (!SystemInfo.supportsImageEffects) {
            enabled = false;
            return;
        }
    }

    void Update() {
        if (Input.GetButton("Action")) {
            w = Mathf.RoundToInt(Mathf.Clamp(w - 10, 340, 720));
        } else {
            w = Mathf.RoundToInt(Mathf.Clamp(w + 10, 340, 720));
        }
        var aspectRation = (float)Screen.height / (float)Screen.width;
        h = Mathf.RoundToInt(w * aspectRation);
    }

    void OnRenderImage(RenderTexture source, RenderTexture destination) {
        source.filterMode = FilterMode.Point;
        RenderTexture buffer = RenderTexture.GetTemporary(w, h, -1);
        buffer.filterMode = FilterMode.Point;
        Graphics.Blit(source, buffer);
        Graphics.Blit(buffer, destination);
        RenderTexture.ReleaseTemporary(buffer);
    }
}