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
    
    public ScaryCircle scaryCircleInst;
    List<ScaryCircle> scaryCircleCont = new List<ScaryCircle>();

    public ScaryCircle getScaryCircle() {
        for (int i = 0; i < scaryCircleCont.Count; ++i) {
            if (!scaryCircleCont[i].gameObject.activeInHierarchy) {
                scaryCircleCont[i].gameObject.SetActive(true);
                return scaryCircleCont[i];
            }
        }
        ScaryCircle sc = Instantiate(scaryCircleInst, transform.position, Quaternion.identity) 
                       as ScaryCircle;
        scaryCircleCont.Add(sc);
        return sc;
    }
    
    public Bird birdInst;
    List<Bird> birdCont = new List<Bird>();

    public Bird getBird() {
        for (int i = 0; i < birdCont.Count; ++i) {
            if (!birdCont[i].gameObject.activeInHierarchy) {
                birdCont[i].gameObject.SetActive(true);
                return birdCont[i];
            }
        }
        Bird b = Instantiate(birdInst, transform.position, Quaternion.identity) 
                  as Bird;
        birdCont.Add(b);
        return b;
    }
    
    public Scrap scrapInst;
    List<Scrap> scrapCont = new List<Scrap>();

    public Scrap getScrap() {
        for (int i = 0; i < scrapCont.Count; ++i) {
            if (!scrapCont[i].gameObject.activeInHierarchy) {
                scrapCont[i].gameObject.SetActive(true);
                return scrapCont[i];
            }
        }
        Scrap b = Instantiate(scrapInst, transform.position, Quaternion.identity) 
                  as Scrap;
        scrapCont.Add(b);
        return b;
    }

    void Awake() {
        inst = this;
    }
}
