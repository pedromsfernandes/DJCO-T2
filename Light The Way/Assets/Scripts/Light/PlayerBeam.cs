using Photon.Pun;
using UnityEngine;

namespace Light
{
    public class PlayerBeam : LightBeam
    {
        public GameObject beamModel;
        public GameObject source;
        public GameObject camera;
        
        private void Awake()
        {
            BeamModel = beamModel;
        }
        
        private void Start()
        {
            Lr = GetComponent<LineRenderer>();
            UpdateColor(LightColor.Of(LightType.Blue));
        }

        protected override void Update()
        {
            if (Active)
            {
                Origin = source.transform.position;
                Direction = camera.transform.forward;
                ProcessRayBeam();
            }
        }

        public override LightBeam UpdateColor(LightColor color)
        {
            GetComponent<PhotonView>().RPC("UpdateColorSelf", RpcTarget.All, color.Type);
            return this;
        }

        [PunRPC]
        private void UpdateColorSelf(int colorType)
        {
            base.UpdateColor(LightColor.Of((LightType) colorType));
        }
        
        // Enables the LightBeam for all Clients (Used when starting a chain of LightBeams
        public void EnableSelf(bool op)
        {
            GameState.Instance.castingRay = op;
            GetComponent<PhotonView>().RPC("Enable", RpcTarget.All, op);
        }
    }
}