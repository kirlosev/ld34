using UnityEngine;
using System.Collections;

public class PlayerInput : MonoBehaviour {
    public Vector3 mousePos;
    public bool shoot;
    public float engineForce;
    public bool boost;

    void Update() {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;
        
        engineForce = Input.GetAxis("ChangeDirection"); 
        boost = Input.GetButtonUp("ChangeDirection");
        shoot = Input.GetButton("Action");
    }
}
