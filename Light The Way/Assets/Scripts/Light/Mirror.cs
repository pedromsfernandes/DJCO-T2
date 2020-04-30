using Light;
using UnityEngine;

public class Mirror : MonoBehaviour
{
    int _beamCnt = 0;

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
        _beamCnt++;
        LightBeam newBeam = transform.Find("laserPool").childCount < _beamCnt 
            ? LightBeam.CreateLightBeam(transform.Find("laserPool"), 
                beam.GetComponent<LightColor>(), origin, direction) 
            : LightBeam.UpdateLightBeam(transform.Find("laserPool").GetChild(_beamCnt - 1).gameObject, 
                beam.GetComponent<LightColor>(), origin, direction);
        
        newBeam.Enable(true);
    }

    void LateUpdate()
    {
        for (int i = _beamCnt; i < transform.Find("laserPool").childCount; i++)
        {
            transform.Find("laserPool").GetChild(i).gameObject.SetActive(false);
        }
        _beamCnt = 0;
    }
}
