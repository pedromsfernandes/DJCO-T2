using Photon.Pun;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Light
{
    public class Endpoint : BeamSensor
    {
        private LightBeam _endpointBeam;

        [SerializeField]
        private LightColor target;

        public LightType targetColor;

        public bool open = false;
        public bool active = true;

        private LightColor currentColor = LightColor.Of(LightType.None);

        private LightColor previousColor = LightColor.Of(LightType.None);

        //sound
        [FMODUnity.EventRef]
        public string selectedHitEndpointSound = "event:/FX/Crystal/CrystalHit";

        [FMODUnity.EventRef]
        public string selectedOpenEndpointSound = "event:/FX/Crystal/CrystalReflect";

        private void Start()
        {
            _endpointBeam = transform.GetChild(0).GetComponent<LightBeam>();
            target = LightColor.Of(targetColor);
            LightBeam.UpdateLightBeam(_endpointBeam.gameObject, target,
                transform.position, transform.up);
            _endpointBeam.EnableSelf(true);

            _endpointBeam.gameObject.SetActive(true);

        }

        protected override void OnBeamSense(LightBeam beam, RaycastHit hit, Vector3 reflectedDirection)
        {
            currentColor.AddColor(beam.LightColor);
        }

        

        void LateUpdate()
        {
            if(!active)
                return;

            if (currentColor.GetColor() == target.GetColor())
            {
                _endpointBeam.gameObject.SetActive(false);
                open = true;

                // GetComponent<PhotonView>().RPC("OnOpenEndpointSoundSelf", RpcTarget.All, this.name);
            }
            else
            {
                _endpointBeam.gameObject.SetActive(true);
                open = false;
            }

            IEnumerable<LightColor> current = currentColor.DivideColors();
            IEnumerable<LightColor> targetDiv = target.DivideColors();
            LightColor beamColor = LightColor.Of(LightType.None);

            if(!current.Contains(LightColor.Of(LightType.Red)) && targetDiv.Contains(LightColor.Of(LightType.Red)))
                beamColor.AddColor(LightColor.Of(LightType.Red));
            if(!current.Contains(LightColor.Of(LightType.Green)) && targetDiv.Contains(LightColor.Of(LightType.Green)))
                beamColor.AddColor(LightColor.Of(LightType.Green));
            if(!current.Contains(LightColor.Of(LightType.Blue)) && targetDiv.Contains(LightColor.Of(LightType.Blue)))
                beamColor.AddColor(LightColor.Of(LightType.Blue));

            LightBeam.UpdateLightBeam(_endpointBeam.gameObject, beamColor,
                transform.position, transform.up);


            if (!currentColor.ToString().Equals(previousColor.ToString()))
            {
                previousColor = currentColor;
                // GetComponent<PhotonView>().RPC("OnBeamSenseSoundSelf", RpcTarget.All, this.name);
            }

            currentColor = LightColor.Of(LightType.None);
        }

        [PunRPC]
        void OnBeamSenseSoundSelf(string originalObjectName)
        {
            GameObject originalObject = GameObject.Find(originalObjectName);
            Transform originalTransform = originalObject.GetComponent<Transform>();

            FMODUnity.RuntimeManager.PlayOneShot(selectedHitEndpointSound, originalTransform.position);
        }

        [PunRPC]
        void OnOpenEndpointSoundSelf(string originalObjectName)
        {
            GameObject originalObject = GameObject.Find(originalObjectName);
            Transform originalTransform = originalObject.GetComponent<Transform>();

            FMODUnity.RuntimeManager.PlayOneShot(selectedOpenEndpointSound, originalTransform.position);
        }

        public void Deactivate()
        {
            _endpointBeam.gameObject.SetActive(false);
            active = false;
        }
    }
}
