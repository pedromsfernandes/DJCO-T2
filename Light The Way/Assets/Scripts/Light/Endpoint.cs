using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Endpoint : MonoBehaviour
{
    LineRenderer lr;

    void Start()
    {
        lr = transform.Find("Laser").GetComponent<LineRenderer>();
        lr.gameObject.AddComponent<LightColor>();
        lr.gameObject.GetComponent<LightColor>().SetColor(false, false, false);
        lr.SetColors(lr.gameObject.GetComponent<LightColor>().GetColor(), lr.gameObject.GetComponent<LightColor>().GetColor());
        lr.SetPosition(0, new Vector3(transform.position.x, 0, transform.position.z));
        lr.SetPosition(1, new Vector3(transform.position.x, 50000, transform.position.z));
    }
}
