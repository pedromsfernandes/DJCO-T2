using System;
using UnityEngine;

namespace Lighting
{
    public class SunController : MonoBehaviour
    {
        public float rotationSpeed = 400;

        private Transform _transform;

        // Start is called before the first frame update
        private void Start()
        {
            _transform = transform;
        }

        private void Update()
        {
            float scrollAxis = Input.GetAxis("Mouse ScrollWheel");
            
            if (GameState.Instance.canRotateSun && Math.Abs(scrollAxis) > 0.01)
            {
                _transform.Rotate(Math.Sign(scrollAxis) * Time.deltaTime * rotationSpeed, 0, 0);
            }
        }
    }
}
