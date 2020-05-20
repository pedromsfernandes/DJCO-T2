using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public Light.Endpoint[] endpoints;

    void Update()
    {
        for(int i = 0; i < endpoints.Length; i++)
        {
            if(!endpoints[i].open)
                return;
        }

        foreach (var ep in endpoints)
        {
            ep.Deactivate();
        }

        this.gameObject.SetActive(false);
    }
}
