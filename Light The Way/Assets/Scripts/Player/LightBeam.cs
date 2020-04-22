using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightBeam : MonoBehaviour
{
    public GameObject source;
    public GameObject camera;

    LineRenderer lr;
    bool active = false;

    void Start()
    {
        lr = GetComponent<LineRenderer>();
    }

    void Update()
    {
        if(active)
        {
            lr.SetPosition(0, source.transform.position);
            RaycastHit hit;
            if(Physics.Raycast(camera.transform.position, camera.transform.forward, out hit))
            {
                if(hit.collider)
                {
                    lr.SetPosition(1, hit.point);
                }
            }
            else
            {
                lr.SetPosition(1, camera.transform.forward*5000);
            }
        }
    }

    public void Enable(bool op)
    {
        this.active = op;
        this.gameObject.SetActive(op);
    }
}
