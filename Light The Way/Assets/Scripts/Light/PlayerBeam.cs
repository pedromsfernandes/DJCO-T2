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
            _lightColor = gameObject.AddComponent<LightColor>().SetColor(false, false, true);
            _lr = GetComponent<LineRenderer>();
        
            _lr.startColor = _lightColor.GetColor();
            _lr.endColor = _lightColor.GetColor();
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
        
        
        // Enables the LightBeam for all Clients (Used when starting a chain of LightBeams
        public void EnableSelf(bool op)
        {
            GetComponent<PhotonView>().RPC("Enable", RpcTarget.All, op);
        }
        
        // public void Enable(bool op, Vector3 origin, Vector3 direction)
        // {
        //     _active = op;
        //     gameObject.SetActive(op);
        // }
    }
}