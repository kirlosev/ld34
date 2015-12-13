using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Wave : MonoBehaviour {
    public string waveName = "wave #1";

    public int[] amountOfEnemies;
    List<Bird> enemyCont = new List<Bird>();
    public Transform[] initPos;
    public bool isActive {
        get {
            return enemyCont.Count > 0;
        }
    }
    
    public void init() {
        for (var j = 0; j < initPos.Length; ++j) {
            for (var i = 0; i < amountOfEnemies[j]; ++i) {
                Bird b = ObjPool.inst.getBird();
                b.init(initPos[j].position + (Vector3)Random.insideUnitCircle, 
                       Random.insideUnitCircle.normalized);
                enemyCont.Add(b); 
            }
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
