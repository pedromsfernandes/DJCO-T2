using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Endpoint : MonoBehaviour
{
    public LightColor target;
    public GameObject door;

    LineRenderer lr;
    LightColor lc;

    void Start()
    {
        lr = transform.Find("Laser").GetComponent<LineRenderer>();
        lc = lr.gameObject.AddComponent<LightColor>();
        lc.SetColor(false, false, false);
        lr.SetColors(target.GetColor(), target.GetColor());
        lr.SetPosition(0, new Vector3(transform.position.x, transform.position.y, transform.position.z));
        lr.SetPosition(1, new Vector3(transform.position.x, transform.position.y + 50000, transform.position.z));
    }

    void Hit(object[] args)
    {
        LightBeam beam = (LightBeam)args[0];
        RaycastHit h = (RaycastHit)args[1];
        Vector3 dir = (Vector3)args[2];

        lc.AddColor(beam.gameObject.GetComponent<LightColor>());
    }

    void LateUpdate()
    {
        if(lc.GetColor() == target.GetColor())
        {
            lr.gameObject.SetActive(false);
            door.SetActive(false);
        }

        lc.SetColor(false, false, false);
    }
}
