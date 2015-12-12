using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {
    public static CameraFollow inst;
    
    public Transform target;
    Camera cam;
    Vector3 velocity;
    public float smoothTime = 0.3f;
    bool shaking = false;
    Vector3 shakeDir;
    float shakeForce;
    
    float height, width;

    void Awake() {
        inst = this;
        cam = GetComponent<Camera>();
    }
    
    void Start() {
        height = cam.orthographicSize;
        width = Screen.width * height / Screen.height;
    }

    void LateUpdate() {
        Vector3 nextPos = target.transform.position;
        nextPos.z = transform.position.z;
        var characters = GameObject.FindObjectsOfType(typeof(Character));
        for (int i = 0; i < characters.Length; ++i) {
            var ch = (Character)characters[i];
            var chDir = ch.transform.position - nextPos;
            nextPos += chDir * ch.cameraValue;
        }
        nextPos.z = transform.position.z;
        
        nextPos.x = Mathf.Clamp(nextPos.x, Level.inst.lbCorner.position.x + width, Level.inst.rtCorner.position.x - width);
        nextPos.y = Mathf.Clamp(nextPos.y, Level.inst.lbCorner.position.y + height, Level.inst.rtCorner.position.y - height);
        
        if (shaking) {
            nextPos += ((shakeDir == (Vector3)Vector2.one) 
                       ? (Vector3)Random.insideUnitCircle
                       : shakeDir * Random.value) * shakeForce; 
            transform.position = nextPos;
        } else {
            transform.position = Vector3.SmoothDamp(transform.position, nextPos, 
                                                    ref velocity, smoothTime);
        }
    }
    
    public void Shake(float force, float duration, Vector3 dir) {
        shakeDir = dir;
        shakeForce = force;
        StartCoroutine(ShakeTimer(duration));
    }
    
    IEnumerator ShakeTimer(float duration) {
        shaking = true;
        yield return new WaitForSeconds(duration);
        shaking = false;
    }
}
