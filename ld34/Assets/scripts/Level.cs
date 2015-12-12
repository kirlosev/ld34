using UnityEngine;
using System.Collections;

public class Level : MonoBehaviour {
    public static Level inst;
    public Transform lbCorner, rtCorner;
    
    void Awake() {
        inst = this;
    }
    
    void Start() {
        /*
        for (int i = 0; i < 250; ++i) {
            Bird b = ObjPool.inst.getBird();
            b.init(Random.insideUnitCircle * 10, Random.insideUnitCircle.normalized);
        }
        */
    }
    
    void OnDrawGizmos() {
        Gizmos.DrawWireCube(lbCorner.position + (rtCorner.position-lbCorner.position)/2, (rtCorner.position-lbCorner.position));
    }
}
