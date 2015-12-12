using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Wave : MonoBehaviour {
    public int amountOfEnemies = 50;
    List<Bird> enemyCont = new List<Bird>();
    public Transform initPos;
    public bool isActive {
        get {
            return enemyCont.Count > 0;
        }
    }
    
    public void init() {
        for (var i = 0; i < amountOfEnemies; ++i) {
            Bird b = ObjPool.inst.getBird();
            b.init(initPos.position + (Vector3)Random.insideUnitCircle, 
                   Random.insideUnitCircle.normalized);
            enemyCont.Add(b); 
        }
        StartCoroutine(checkContainer());
    }
    
    IEnumerator checkContainer() {
        while (isActive) {
            for (var i = 0; i < enemyCont.Count; ) {
                if (!enemyCont[i].gameObject.activeInHierarchy) {
                    enemyCont.RemoveAt(i);
                } else {
                    ++i;
                }
            }    
            yield return new WaitForSeconds(1);    
        }
    }
}
