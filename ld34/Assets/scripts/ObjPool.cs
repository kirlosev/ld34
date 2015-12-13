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

    public Bullet laserInst;
    List<Bullet> laserCont = new List<Bullet>();

    public Bullet getLaser() {
        for (int i = 0; i < laserCont.Count; ++i) {
            if (!laserCont[i].gameObject.activeInHierarchy) {
                laserCont[i].gameObject.SetActive(true);
                return laserCont[i];
            }
        }
        Bullet b = Instantiate(laserInst, transform.position, Quaternion.identity) 
            as Bullet;
        laserCont.Add(b);
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

    public ExplosionManager expManInst;
    List<ExplosionManager> expManCont = new List<ExplosionManager>();

    public ExplosionManager getExplosionManager() {
        for (int i = 0; i < expManCont.Count; ++i) {
            if (!expManCont[i].gameObject.activeInHierarchy) {
                expManCont[i].gameObject.SetActive(true);
                return expManCont[i];
            }
        }
        ExplosionManager b = Instantiate(expManInst, transform.position, Quaternion.identity) 
            as ExplosionManager;
        expManCont.Add(b);
        return b;
    }

    public Explosion expInst;
    List<Explosion> expCont = new List<Explosion>();

    public Explosion getExplosion() {
        for (int i = 0; i < expCont.Count; ++i) {
            if (!expCont[i].gameObject.activeInHierarchy) {
                expCont[i].gameObject.SetActive(true);
                return expCont[i];
            }
        }
        Explosion b = Instantiate(expInst, transform.position, Quaternion.identity) 
            as Explosion;
        expCont.Add(b);
        return b;
    }

    void Awake() {
        inst = this;
    }
}
