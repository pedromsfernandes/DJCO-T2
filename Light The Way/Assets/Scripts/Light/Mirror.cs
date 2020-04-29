using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mirror : MonoBehaviour
{
    public GameObject beamModel;

    int beamCnt = 0;

    void Hit(object[] args)
    {
        LightBeam beam = (LightBeam)args[0];
        RaycastHit h = (RaycastHit)args[1];
        Vector3 dir = (Vector3)args[2];

        Vector3 origin = h.point;
        Vector3 direction = Vector3.Reflect(dir, h.normal);

        UpdateBeam(beam, origin, direction);
    }

    void UpdateBeam(LightBeam beam, Vector3 origin, Vector3 direction)
    {
        LineRenderer lr;

        beamCnt++;

        if (transform.Find("laserPool").childCount < beamCnt)
        {
            GameObject newBeam = (GameObject)Instantiate(beamModel);
            newBeam.transform.parent = transform.Find("laserPool");
            newBeam.AddComponent<LightColor>();
            lr = newBeam.GetComponent<LineRenderer>();
        }
        else
        {
            lr = transform.Find("laserPool").GetChild(beamCnt - 1).gameObject.GetComponent<LineRenderer>();
        }

        lr.gameObject.GetComponent<LightColor>().SetColor(beam.GetComponent<LightColor>());
        lr.SetPosition(0, origin);
        RaycastHit hit;
        if (Physics.Raycast(origin, direction, out hit))
        {
            if (hit.collider)
            {
                lr.SetPosition(1, hit.point);
                if (hit.transform.gameObject.layer == LayerMask.NameToLayer("LightHit"))
                    hit.transform.gameObject.SendMessage("Hit", new object[] { beam, hit, direction });
            }
        }
        else
        {
            lr.SetPosition(1, direction * 5000);
        }

        lr.gameObject.SetActive(true);
    }

    void LateUpdate()
    {
        for (int i = beamCnt; i < transform.Find("laserPool").childCount; i++)
        {
            transform.Find("laserPool").GetChild(i).gameObject.SetActive(false);
        }
        beamCnt = 0;
    }
}
