using UnityEngine;
using System.Collections;

public class QuadSky : MonoBehaviour {
    public Material groundMat;
    public Transform lbCorner, rtCorner;
    public bool needBoxColl = false;
    
    MeshFilter mf;
    MeshRenderer mr;
    
    void Start() {
        var mesh = new Mesh();
        mf = gameObject.AddComponent<MeshFilter>();
        mf.sharedMesh = mesh;

        var vertices = new Vector3[4];
        vertices[0] = Vector3.right * lbCorner.position.x 
                    + Vector3.up * lbCorner.position.y + Vector3.forward;
        vertices[1] = Vector3.right * lbCorner.position.x 
                    + Vector3.up * rtCorner.position.y + Vector3.forward;
        vertices[2] = Vector3.right * rtCorner.position.x 
                    + Vector3.up * rtCorner.position.y + Vector3.forward;
        vertices[3] = Vector3.right * rtCorner.position.x 
                    + Vector3.up * lbCorner.position.y + Vector3.forward;

        var uvs = new Vector2[4];
        uvs[0] = new Vector2(0, 0);
        uvs[1] = new Vector2(0, 1);
        uvs[2] = new Vector2(1, 1);
        uvs[3] = new Vector2(1, 0);

        var triangles = new int[] { 0, 1, 2, 0, 2, 3 };
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;
        mesh.RecalculateNormals();

        mr = gameObject.AddComponent<MeshRenderer>();
        mr.sharedMaterial = groundMat;
        
        if (needBoxColl) {
            gameObject.AddComponent<BoxCollider2D>();
        }
    }
    
    void LateUpdate() {
        mr.material.mainTextureOffset += (Vector2.right * Mathf.Clamp(Player.inst.velocity.x, -1, 1)
                                       + Vector2.up * Mathf.Clamp(Player.inst.velocity.y, -1, 1)) / 2 * Time.deltaTime;
    }
}
