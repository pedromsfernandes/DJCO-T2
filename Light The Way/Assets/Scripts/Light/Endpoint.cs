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

        protected override void OnBeamSenseStart(LightBeam beam, RaycastHit hit, Vector3 reflectedDirection)
        {
            _endpointBeam.UpdateColor(_endpointBeam.LightColor.AddColor(beam.LightColor));
            Debug.Log(_endpointBeam.LightColor._type);
        }
    
        protected override void OnBeamSenseEnd(LightBeam beam, RaycastHit hit, Vector3 reflectedDirection)
        {
            _endpointBeam.UpdateColor(_endpointBeam.LightColor.RemoveColor(beam.LightColor));
        }
    }
}
