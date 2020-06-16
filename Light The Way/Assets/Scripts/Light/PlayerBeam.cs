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

        [FMODUnity.EventRef]
        public string selectedRedMissfireSound;
        [FMODUnity.EventRef]
        public string selectedGreenMissfireSound;
        [FMODUnity.EventRef]
        public string selectedBlueMissfireSound;
        FMOD.Studio.EventInstance redMissfireSoundEvent;
        FMOD.Studio.EventInstance greenMissfireSoundEvent;
        FMOD.Studio.EventInstance blueMissfireSoundEvent;

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

            redMissfireSoundEvent = FMODUnity.RuntimeManager.CreateInstance(selectedRedMissfireSound);
            greenMissfireSoundEvent = FMODUnity.RuntimeManager.CreateInstance(selectedGreenMissfireSound);
            blueMissfireSoundEvent = FMODUnity.RuntimeManager.CreateInstance(selectedBlueMissfireSound);
        }

        private void OnEnable()
        {
            InvokeRepeating(nameof(SyncDirection), 0.5f, 0.5f);
        }

        private void SyncDirection()
        {
            GetComponent<PhotonView>().RPC(nameof(SyncDirectionSelf), RpcTarget.All, camera.transform.forward);
        }

        [PunRPC]
        public void SyncDirectionSelf(Vector3 direction)
        {
            Direction = direction;
        }

        private void OnDisable()
        {
            CancelInvoke(nameof(SyncDirection));
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


                FMOD.Studio.PLAYBACK_STATE fmodPBState;
                if (GameState.Instance.currentTool == 1)
                {
                    redMissfireSoundEvent.getPlaybackState(out fmodPBState);
                    if (fmodPBState != FMOD.Studio.PLAYBACK_STATE.PLAYING && GetComponent<PhotonView>().IsMine)
                    {
                        GetComponent<PhotonView>().RPC("PlayMissfireSoundSelf", RpcTarget.All, 1, GameState.Instance.playerTransform.name);
                    }
                }
                else if (GameState.Instance.currentTool == 2)
                {
                    greenMissfireSoundEvent.getPlaybackState(out fmodPBState);
                    if (fmodPBState != FMOD.Studio.PLAYBACK_STATE.PLAYING && GetComponent<PhotonView>().IsMine)
                    {
                        GetComponent<PhotonView>().RPC("PlayMissfireSoundSelf", RpcTarget.All, 2, GameState.Instance.playerTransform.name);
                    }
                }
                else if (GameState.Instance.currentTool == 3)
                {
                    blueMissfireSoundEvent.getPlaybackState(out fmodPBState);
                    if (fmodPBState != FMOD.Studio.PLAYBACK_STATE.PLAYING && GetComponent<PhotonView>().IsMine)
                    {
                        GetComponent<PhotonView>().RPC("PlayMissfireSoundSelf", RpcTarget.All, 3, GameState.Instance.playerTransform.name);
                    }
                }

                //Debug.Log("In Shadow");
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

        [PunRPC]
        void PlayMissfireSoundSelf(int toolId, string originalPlayerName)
        {
            //Debug.Log(originalPlayerName);

            if (transform.parent.transform.name != originalPlayerName)
                return;
            GameObject originalPlayer = GameObject.Find(originalPlayerName);
            Transform originalTransform = originalPlayer.GetComponent<Transform>();
            Rigidbody originalRigidbody = originalPlayer.GetComponentInChildren<Rigidbody>();

            //Debug.Log("Missfire Here");

            if(toolId == 1)
            {

                FMODUnity.RuntimeManager.AttachInstanceToGameObject(redMissfireSoundEvent, originalTransform, originalRigidbody);
                redMissfireSoundEvent.start();
            }
            else if (toolId == 2)
            {

                FMODUnity.RuntimeManager.AttachInstanceToGameObject(greenMissfireSoundEvent, originalTransform, originalRigidbody);
                greenMissfireSoundEvent.start();
            }
            else if (toolId == 3)
            {

                FMODUnity.RuntimeManager.AttachInstanceToGameObject(blueMissfireSoundEvent, originalTransform, originalRigidbody);
                blueMissfireSoundEvent.start();
            }
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

        // Enables the LightBeam for all Clients (Used when starting a chain of LightBeams)
        public void Enable(bool op)
        {

            GameState.Instance.castingRay = op;

            GetComponent<PhotonView>().RPC("EnableSelf", RpcTarget.All, op);
        }
    }
}