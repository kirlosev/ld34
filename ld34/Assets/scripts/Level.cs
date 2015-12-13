using UnityEngine;
using System.Collections;

public class Level : MonoBehaviour {
    public static Level inst;
    public Transform lbCorner, rtCorner;
    public Prop prop;
    public Groundgun[] groundGunInst;
    
    void Awake() {
        inst = this;
    }
    
    void Start() {
        var amountOfProps = 150;
        for (int i = 0; i < amountOfProps; ++i) {
            var posX = Random.Range(lbCorner.position.x, rtCorner.position.x);
            var pr = Instantiate(prop, Vector3.right * posX, Quaternion.identity)
                as Prop;
            pr.init();
        }

        var amountOfGroundGun = 20;
        for (int i = 0; i < amountOfGroundGun; ++i) {
            var posX = Random.Range(lbCorner.position.x, rtCorner.position.x);
            var ind = Random.Range(0, groundGunInst.Length);
            var pr = Instantiate(groundGunInst[ind], Vector3.right * posX, Quaternion.identity)
                as Groundgun;
        }
    }
    
    void OnDrawGizmos() {
        Gizmos.DrawWireCube(lbCorner.position + (rtCorner.position-lbCorner.position)/2, (rtCorner.position-lbCorner.position));
    }
}
