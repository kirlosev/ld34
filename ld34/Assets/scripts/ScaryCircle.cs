using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ScaryCircle : MonoBehaviour {
    List<GameObject> scrapCtrl = new List<GameObject>();
    
    public void init(Vector3 pos, float targetRadius) {
        transform.position = pos;

        var col = GetComponent<Collider2D>(); 
        col.enabled = true;
        
        float currSize      = col.bounds.size.x;
        Vector2 scale       = transform.localScale;
        scale.x             = targetRadius * scale.x / currSize;
        scale.y             = scale.x;
        transform.localScale = scale;
        
        scrapCtrl.Clear();
        var amountOfFeathers = Random.Range(5, 8);
        for (var i = 0; i < amountOfFeathers; ++i) {
            var f = ObjPool.inst.getScrap();
            f.init();
            f.transform.position = transform.position + (Vector3)Random.insideUnitCircle * targetRadius;
            f.gameObject.SetActive(true);
            scrapCtrl.Add(f.gameObject);
        }

        StartCoroutine(DestroyAfterSeconds(Random.value + 1));
    }
    
    IEnumerator DestroyAfterSeconds(float timeToDestroy) {
        var timer = 0f;
        while (timer < timeToDestroy) {
            for (var i = 0; i < scrapCtrl.Count; ++i) {
                var deltaAngle = Random.Range(0.1f, 1);
                var angle = Mathf.Atan2(scrapCtrl[i].transform.right.y, 
                                        scrapCtrl[i].transform.right.x) 
                                        * Mathf.Rad2Deg;
                scrapCtrl[i].transform.rotation = Quaternion.Euler(0, 0, 
                                                  angle + deltaAngle * Random.Range(-1, 1));
                scrapCtrl[i].transform.position += (scrapCtrl[i].transform.position - transform.position - Vector3.up * 9) * Random.value * Time.deltaTime;
            }
            timer += Time.deltaTime;
            yield return null;
        }
        for (var i = 0; i < scrapCtrl.Count; ++i) {
//            feathersToControl[i].transform.localScale = Vector3.one;
            scrapCtrl[i].gameObject.SetActive(false);
        }
        gameObject.SetActive(false);
    }
}
