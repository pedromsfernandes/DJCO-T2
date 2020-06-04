using UnityEngine;

namespace Light
{
    public class Emitter : MonoBehaviour
    {
        public LightType color;
        
        private LightBeam _beam;

        private void Start()
        {
            _beam = transform.GetChild(0).GetComponent<LightBeam>();
            LightBeam.UpdateLightBeam(_beam.gameObject, LightColor.Of(color), _beam.gameObject.transform.position, transform.forward);
            
            _beam.gameObject.SetActive(true);
        }
    }
}