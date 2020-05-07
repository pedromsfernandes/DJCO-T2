using Photon.Pun;
using UnityEngine;

namespace Light
{
    public class Destroyable : BeamSensor
    {
        private float _timeWithHit = 0f;
        private float _timeSinceLastHit = 0f;

        public float timeout = 1f;
        public float timeToDestroy = 5f;

        protected override void OnBeamSense(LightBeam beam, RaycastHit hit, Vector3 reflectedDirection)
        {
            if (GameState.Instance.canDestroyObjects && GameState.Instance.hasTool1 && GameState.Instance.currentTool == 1)
            {
                _timeSinceLastHit = 0;
            }
        }

        private void Update()
        {
            if (_timeSinceLastHit > timeout)
                return;

            _timeSinceLastHit += Time.deltaTime;
            _timeWithHit += Time.deltaTime;

            if (_timeSinceLastHit > timeout)
            {
                _timeWithHit = 0;
                return;
            }

            if (_timeWithHit > timeToDestroy)
                Destroy();
        }

        private void Destroy()
        {
            GetComponent<PhotonView>().RPC("DestroySelf", RpcTarget.All);
        }

        [PunRPC]
        void DestroySelf()
        {
            this.gameObject.SetActive(false);
        }
    }
}
