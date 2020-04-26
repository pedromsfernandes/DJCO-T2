using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mirror : MonoBehaviour
{
    public LineRenderer lr;

    LightBeam beam;
    Vector3 origin;
    Vector3 direction;

    bool reflecting = false;

    void Hit(object[] args)
    {
        beam = (LightBeam)args[0];
        RaycastHit h = (RaycastHit)args[1];
        Vector3 dir = (Vector3)args[2];

        origin = h.point;
        direction = Vector3.Reflect(dir, h.normal);
        reflecting = true;
        UpdateBeam();
    }

    void UpdateBeam()
    {
        if (reflecting)
        {
            Debug.Log(this.gameObject.name);
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
        }
    }

    void LateUpdate()
    {
        if(reflecting)
        {
            reflecting = false;
            lr.gameObject.SetActive(true);
        }
        else
        {
            lr.gameObject.SetActive(false);
        }
    }
}
