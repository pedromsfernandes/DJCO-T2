using System;
using System.Linq;
using Photon.Pun;
using UnityEngine;

namespace Light
{
    public class PlayerBeam : LightBeam
    {
        public GameObject beamModel;
        public GameObject source;
        public GameObject camera;

        private LineRenderer _sunLr;

        private void Awake()
        {
            BeamModel = beamModel;
        }

        private void Start()
        {
            Lr = GetComponent<LineRenderer>();
            _sunLr = transform.GetComponentsInChildren<LineRenderer>()
                .First(lr => lr.gameObject != this.gameObject);

            _sunLr.startColor = _sunLr.endColor = Color.white;

            int currentTool = GameState.Instance.currentTool;
            if (GetComponent<PhotonView>().IsMine)
            {
                if (currentTool == 1)
                {
                    UpdateColor(LightColor.Of(LightType.Red));
                }
                else if (currentTool == 2)
                {
                    UpdateColor(LightColor.Of(LightType.Green));
                }
                else if (currentTool == 3)
                {
                    UpdateColor(LightColor.Of(LightType.Blue));
                }
            }
        }

        protected override void Update()
        {
            if (!Active) return;

            Origin = source.transform.position;
            Direction = camera.transform.forward;

            if (InShadow())
            {
                _sunLr.startColor = _sunLr.endColor = new Color(0, 0, 0, 0);
                Lr.SetPositions(new[] { Vector3.zero, Vector3.zero });
            }
            else
            {
                _sunLr.startColor = _sunLr.endColor = Color.white;
                _sunLr.SetPositions(new[] { Origin, Origin - 1000 * GameState.Instance.sunDirection });
                ProcessRayBeam();
            }
        }

        private bool InShadow()
        {
            var inShadow = Physics.Raycast(Origin, -GameState.Instance.sunDirection);

            if (GetComponent<PhotonView>().IsMine)
                GameState.Instance.castingRay = !inShadow;

            return inShadow;
        }

        public override LightBeam UpdateColor(LightColor color)
        {
            if (GetComponent<PhotonView>().IsMine)
                GetComponent<PhotonView>().RPC("UpdateColorSelf", RpcTarget.All, color.Type, transform.parent.transform.name);
            return this;
        }

        [PunRPC]
        private void UpdateColorSelf(int colorType, string name)
        {
            if (transform.parent.transform.name == name)
                base.UpdateColor(LightColor.Of((LightType)colorType));
        }

        // Enables the LightBeam for all Clients (Used when starting a chain of LightBeams
        public void Enable(bool op)
        {

            GameState.Instance.castingRay = op;

            GetComponent<PhotonView>().RPC("EnableSelf", RpcTarget.All, op);
        }
    }
}