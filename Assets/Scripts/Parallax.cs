using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    private float length, Startpos;
    public GameObject cam;
    public float parallaxeffect;
    void Start()
    {
        Startpos = transform.position.x;
        length=GetComponent<SpriteRenderer>().bounds.size.x;
    }
    void Update()
    {
        float temp = (cam.transform.position.x * parallaxeffect);
        float dist = (cam.transform.position.x * parallaxeffect);
        transform.position = new Vector3(Startpos + dist, transform.position.y, transform.position.z);

        if (temp > Startpos + length) Startpos += length;
        else if (temp < Startpos - length) Startpos -= length;
    }
}
