using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mirror : MonoBehaviour
{
    void Hit(object[] args)
    {
        LightBeam beam = (LightBeam) args[0];
        RaycastHit hit = (RaycastHit) args[1];
        Debug.Log("HIT");
    }
}
