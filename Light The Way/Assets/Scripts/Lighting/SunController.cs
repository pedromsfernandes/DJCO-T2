using System;
using UnityEngine;

namespace Lighting
{
    public class SunController : MonoBehaviour
    {
        public float rotationSpeed;

        private Transform _transform;
        
        private float _finalRotation;

        // Start is called before the first frame update
        void Start()
        {
            _transform = transform;
        }

        private void Update()
        {
            _transform.rotation = Quaternion.RotateTowards(_transform.rotation, 
                Quaternion.Euler(_finalRotation, 0, 0), 
                Time.deltaTime * rotationSpeed);
        }

        public void SetSunHour(float hour)
        {
            _finalRotation = hour * 270 / 24 - 45;
        }
    }
}
