using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjPool : MonoBehaviour {
    public static ObjPool inst;

    public Bullet bulletInst;
    List<Bullet> bulletCont = new List<Bullet>();

    public Bullet getBullet() {
        for (int i = 0; i < bulletCont.Count; ++i) {
            if (!bulletCont[i].gameObject.activeInHierarchy) {
                bulletCont[i].gameObject.SetActive(true);
                return bulletCont[i];
            }
        }
        Bullet b = Instantiate(bulletInst, transform.position, Quaternion.identity) 
                   as Bullet;
        bulletCont.Add(b);
        return b;
    }

    void Awake() {
        inst = this;
    }
}
