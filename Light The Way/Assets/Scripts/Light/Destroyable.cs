using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Destroyable : MonoBehaviour
{
    private float timeWithHit = 0f;

    private float timeSinceLastHit = 0f;

    public float TIMEOUT = 1f;

    public float TIME_TO_DESTROY = 5f;

    public Color TOOL_COLOR = new Color(0, 0, 0.5f, 1);
    void Hit(object[] args)
    {
        LightBeam beam = (LightBeam)args[0];

        if(beam.GetComponent<LightColor>().IsColorEqual(TOOL_COLOR))
            timeSinceLastHit = 0;
    }

    void Update()
    {
        if (timeSinceLastHit > TIMEOUT)
            return;

        timeSinceLastHit += Time.deltaTime;
        timeWithHit += Time.deltaTime;

        if (timeSinceLastHit > TIMEOUT)
        {
            timeWithHit = 0;
            return;
        }

        if (timeWithHit > TIME_TO_DESTROY)
            this.Destroy();
    }


    public void Destroy()
    {
        this.GetComponent<PhotonView>().RPC("DestroySelf", RpcTarget.All);
    }

    [PunRPC]
    void DestroySelf()
    {
        this.gameObject.SetActive(false);
    }
}
