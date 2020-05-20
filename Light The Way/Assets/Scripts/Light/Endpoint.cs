using UnityEngine;

namespace Light
{
    public class Endpoint : BeamSensor
    {
        private LightBeam _endpointBeam;

        [SerializeField]
        private LightColor target;

        public LightType targetColor;

        public GameObject door;

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
            if (currentColor.GetColor() == target.GetColor())
            {
                _endpointBeam.gameObject.SetActive(false);
                door.SetActive(false);
            }

            currentColor = LightColor.Of(LightType.None);
        }
    }
}
