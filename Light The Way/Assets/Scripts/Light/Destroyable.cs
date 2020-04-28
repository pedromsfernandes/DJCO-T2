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

    private bool hit = false;
    void Hit(object[] args)
    {
        hit = true;
        timeSinceLastHit = 0;
    }

    void Update()
    {
        timeSinceLastHit += Time.deltaTime;
        timeWithHit += Time.deltaTime;

        if (timeSinceLastHit > TIMEOUT)
        {
            timeWithHit = 0;
        }

        if (timeWithHit > TIME_TO_DESTROY)
        {
            this.Destroy();
        }
    }


    public void Destroy()
    {
        this.GetComponent<PhotonView>().RPC("DestroySelf", RpcTarget.All);
    }

    [PunRPC]
    void DestroySelf()
    {
        Destroy(this.gameObject);
    }
}
