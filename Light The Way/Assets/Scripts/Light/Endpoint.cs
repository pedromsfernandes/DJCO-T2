using UnityEngine;

namespace Light
{
    public class Endpoint : BeamSensor
    {
        private LightBeam _endpointBeam;

        private void Start()
        {
            _endpointBeam = transform.GetChild(0).GetComponent<LightBeam>();
            LightBeam.UpdateLightBeam(_endpointBeam.gameObject, LightColor.Of(LightType.None), 
                transform.position, Vector3.up);
            
            _endpointBeam.Enable(true);
        }

        protected override void OnBeamSense(LightBeam beam, RaycastHit hit, Vector3 reflectedDirection)
        {
            _endpointBeam.StageColor(_endpointBeam.LightColor.AddColor(beam.LightColor));
        }
    }
}
