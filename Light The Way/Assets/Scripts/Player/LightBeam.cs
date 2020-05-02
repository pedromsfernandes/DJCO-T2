using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class LightBeam : MonoBehaviour
{
    public GameObject source;
    public GameObject camera;
    public LightBeam lightBeam;

    LineRenderer lr;
    bool active = false;

    void Start()
    {
        lr = GetComponent<LineRenderer>();
        lr.gameObject.AddComponent<LightColor>();
        lr.gameObject.GetComponent<LightColor>().SetColor(false, false, true);
        lr.SetColors(lr.gameObject.GetComponent<LightColor>().GetColor(), lr.gameObject.GetComponent<LightColor>().GetColor());
        lightBeam = this.GetComponent<LightBeam>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            lightBeam.UpdateColor(true, false, false);
        }
        else if (Input.GetKeyDown(KeyCode.G))
        {
            lightBeam.UpdateColor(false, true, false);
        }
        else if (Input.GetKeyDown(KeyCode.B))
        {
            lightBeam.UpdateColor(false, false, true);
        }

        if (active)
        {
            lr.SetPosition(0, source.transform.position);
            RaycastHit hit;
            if (Physics.Raycast(source.transform.position, camera.transform.forward, out hit))
            {
                if (hit.collider)
                {
                    lr.SetPosition(1, hit.point);
                    GameState.Instance.lastBeamHit = hit.point;
                    if (hit.transform.gameObject.layer == LayerMask.NameToLayer("LightHit"))
                        hit.transform.gameObject.SendMessage("Hit", new object[] { this, hit, camera.transform.forward });
                }
            }
            else
            {
                lr.SetPosition(1, camera.transform.forward * 5000);
                GameState.Instance.lastBeamHit = camera.transform.forward * 5000;
            }
        }
    }

    public void UpdateColor(bool red, bool green, bool blue)
    {
        this.GetComponent<PhotonView>().RPC("UpdateColorSelf", RpcTarget.All, red, green, blue);
    }

    [PunRPC]
    void UpdateColorSelf(bool red, bool green, bool blue)
    {
        lr.gameObject.GetComponent<LightColor>().SetColor(red, green, blue);
        lr.SetColors(lr.gameObject.GetComponent<LightColor>().GetColor(), lr.gameObject.GetComponent<LightColor>().GetColor());
    }

    public void Enable(bool op)
    {
        this.GetComponent<PhotonView>().RPC("EnableSelf", RpcTarget.All, op);
    }

    [PunRPC]
    void EnableSelf(bool op)
    {
        GameState.Instance.castingRay = op;
        this.active = op;
        this.gameObject.SetActive(op);
    }
}
