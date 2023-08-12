using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blobber : MonoBehaviour
{
    Transform host;
    public Movement manager;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(transform.position, manager.host);
        float size = (manager.scale - distance) / 3;
        float scale = Mathf.Clamp(size, 0f, manager.maxSize);
        float oldX = transform.localScale.x;

        transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(scale, scale, scale), 0.01f);
        if (oldX > transform.localScale.x && transform.localScale.x < 0.01f) {
            Destroy(gameObject);
        } else {
        }
    }
}
