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

            currentColor = LightColor.Of(LightType.None);
        }

        public void Deactivate()
        {
            _endpointBeam.gameObject.SetActive(false);
            active = false;
        }
    }
}
