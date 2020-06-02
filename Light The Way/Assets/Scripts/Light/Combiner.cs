using UnityEngine;

namespace Light
{
    public class Combiner : BeamSensor
    {
        private LightBeam _combinerBeam;

        private void Start()
        {
            _combinerBeam = transform.GetChild(0).GetComponent<LightBeam>();
            LightBeam.UpdateLightBeam(_combinerBeam.gameObject, LightColor.Of(LightType.None), 
                _combinerBeam.gameObject.transform.position, transform.forward);
            
            _combinerBeam.gameObject.SetActive(true);
        }

        protected override void OnBeamSense(LightColor beam, Vector3 point, Vector3 normal, Vector3 reflectedDirection)
        {
            _combinerBeam.StageAddColor(beam);
        }
    }
}