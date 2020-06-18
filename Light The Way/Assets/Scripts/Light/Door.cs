using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class Door : MonoBehaviour
{
    public Light.Endpoint[] endpoints;
    public Door[] doors;
    public Temple temple;
    public float height;
    //sound
    [FMODUnity.EventRef]
    public string selectedOpenSound = "event:/FX/Door/DoorOpen";

    bool open = false;

    void Update()
    {
        if(!open)
        {
            for (int i = 0; i < endpoints.Length; i++)
            {
                if (!endpoints[i].open)
                    return;
            }

            GetComponent<PhotonView>().RPC("OpenDoorSelf", RpcTarget.All);
            open = true;
        }
    }

    [PunRPC]
    public void OpenDoorSelf()
    {
        foreach (var ep in endpoints)
        {
            ep.Deactivate();
        }

        foreach (var door in doors)
        {
            door.OpenDoorSelf();
        }

        if(temple != null)
            temple.Complete();

        StartCoroutine("DoorOpenAnim");

        FMODUnity.RuntimeManager.PlayOneShot(selectedOpenSound, this.transform.position);
    }

    IEnumerator DoorOpenAnim()
    {
        float step = height / 330f;

        for (int i = 0; i < 330; i++)
        {
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y - step, transform.localPosition.z);
            yield return new WaitForSeconds(0.01f);
        }
    }
}
