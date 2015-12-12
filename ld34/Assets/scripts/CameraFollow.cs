using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {
    public Transform target;
    Camera cam;
    Vector3 velocity;
    public float smoothTime = 0.3f;

    void Awake() {
        cam = GetComponent<Camera>();
    }

    void LateUpdate() {
        Vector3 nextPos = target.transform.position;
        nextPos.z = transform.position.z;
        transform.position = Vector3.SmoothDamp(transform.position, nextPos, 
                                                ref velocity, smoothTime);
    }
}
