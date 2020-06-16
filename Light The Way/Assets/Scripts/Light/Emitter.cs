using System;
using System.Collections;
using UnityEngine;

namespace Light
{
    public class Emitter : MonoBehaviour
    {
        public LightType color;

        private LightColor _color;
        private LightBeam _beam;

        private void Start()
        {
            _color = LightColor.Of(color);
            _beam = transform.GetChild(0).GetComponent<LightBeam>();

            LightBeam.UpdateLightBeam(_beam.gameObject, _color, 
                _beam.transform.position, transform.forward);
            
            _beam.gameObject.SetActive(true);
        }

        private void Update()
        {
            _beam.StageAddColor(_color);
            LightBeam.UpdateLightBeam(_beam.gameObject, _color, 
                _beam.transform.position, transform.forward);
        }
    }
}