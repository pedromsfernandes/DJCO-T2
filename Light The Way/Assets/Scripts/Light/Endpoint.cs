using UnityEngine;

namespace Light
{
    public class Endpoint : BeamSensor
    {
        private LightBeam _endpointBeam;
        public LightColor target;
        public GameObject door;

        private LightColor currentColor = new LightColor(LightType.None);

        private void Start()
        {
            _endpointBeam = transform.GetChild(0).GetComponent<LightBeam>();
            LightBeam.UpdateLightBeam(_endpointBeam.gameObject, target,
                transform.position, Vector3.up);

            _endpointBeam.gameObject.SetActive(true);

        }

        protected override void OnBeamSense(LightBeam beam, RaycastHit hit, Vector3 reflectedDirection)
        {
            currentColor.AddColor(beam.LightColor);
        }

        void LateUpdate()
        {
            if (currentColor.GetColor() == target.GetColor())
            {
                _endpointBeam.gameObject.SetActive(false);
                door.SetActive(false);
            }

            currentColor.SetColor(LightType.None);
        }
    }
}
