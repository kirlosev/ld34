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

    void Awake() {
        inst = this;
        cam = GetComponent<Camera>();
    }

    void LateUpdate() {
        Vector3 nextPos = target.transform.position;
        nextPos.z = transform.position.z;
        
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
