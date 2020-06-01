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

        public LightType toolColor;

        //sound
        [FMODUnity.EventRef]
        public string selectedExplosionSound;
        FMOD.Studio.EventInstance explosionSoundEvent;

        void Start()
        {
            //sound
            explosionSoundEvent = FMODUnity.RuntimeManager.CreateInstance(selectedExplosionSound);
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
        protected override void OnBeamSense(LightBeam beam, RaycastHit hit, Vector3 reflectedDirection)
        {
            if (GameState.Instance.canDestroyObjects && beam.LightColor.Equals(toolColor))
            {
                _timeSinceLastHit = 0;
            }
        }

        private void Destroy()
        {
            GetComponent<PhotonView>().RPC("DestroySelf", RpcTarget.All, GameState.Instance.playerTransform.name);
        }

        [PunRPC]
        void DestroySelf(string originalPlayerName)
        {
            GameObject originalPlayer = GameObject.Find(originalPlayerName);
            Transform originalTransform = originalPlayer.GetComponent<Transform>();
            Rigidbody originalRigidbody = originalPlayer.GetComponentInChildren<Rigidbody>();

            FMODUnity.RuntimeManager.AttachInstanceToGameObject(explosionSoundEvent, originalTransform, originalRigidbody);

            explosionSoundEvent.start();


            this.gameObject.SetActive(false);
        }
    }
}
