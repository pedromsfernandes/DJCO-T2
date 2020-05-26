using System.Collections.Generic;
using UnityEngine;

namespace Light
{
    public class Divider : BeamEmitter
    {
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        protected override void OnBeamSense(LightBeam beam, RaycastHit hit, Vector3 reflectedDirection)
        {
            Vector3 diff = hit.point - transform.position;

            var primaryColors = new List<LightColor>(beam.LightColor.DivideColors());
            int angle = 90, angleDelta = 180 / (primaryColors.Count + 1);
            foreach (var color in primaryColors)
            {
                angle += angleDelta;
                
                var originDiff = Quaternion.AngleAxis(angle, transform.up) * diff;
                var direction = Vector3.ProjectOnPlane(originDiff, transform.up);
                EmitBeam(color, transform.position + originDiff, direction);
            }
        }
    }
}
