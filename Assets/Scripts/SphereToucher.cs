using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SphereToucher : MonoBehaviour
{
    public Dictionary<int, GameObject> touches = new Dictionary<int, GameObject>();

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnDrawGizmos() {
        // if (c) {
        // Gizmos.color = Color.red;
        // Gizmos.DrawRay(c.point, c.normal * 5);
        // }
    }

    void OnCollisionEnter(Collision collisionInfo) {
        GameObject g = collisionInfo.gameObject;
        int id = g.GetInstanceID();
        if (!touches.ContainsKey(id)) touches.Add(id, g);
    }

    void OnCollisionExit(Collision collisionInfo) {
        GameObject g = collisionInfo.gameObject;
        int id = g.GetInstanceID();
        if (touches.ContainsKey(id)) touches.Remove(g.GetInstanceID());
    }
}
