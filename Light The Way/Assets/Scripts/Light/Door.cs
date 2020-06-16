using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class Door : MonoBehaviour
{
    public Light.Endpoint[] endpoints;
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

        GetComponent<PhotonView>().RPC("OpenDoorSelf", RpcTarget.All);
    }

    [PunRPC]
    public void OpenDoorSelf(string originalObjectName)
    {
        foreach (var ep in endpoints)
        {
            ep.Deactivate();
        }

        StartCoroutine("DoorOpenAnim");
        GameObject originalObject = GameObject.Find(originalObjectName);
        Transform originalTransform = originalObject.GetComponent<Transform>();

        FMODUnity.RuntimeManager.PlayOneShot(selectedOpenSound, originalTransform.position);

        this.gameObject.SetActive(false);
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
