using System.Collections.Generic;
using UnityEngine;

namespace Light
{
    public class Divider : BeamEmitter
    {
        protected override void OnBeamSense(LightBeam beam, RaycastHit hit, Vector3 reflectedDirection)
        {
            Vector3 diff = hit.point - transform.position;

            var primaryColors = new List<LightColor>(beam.LightColor.DivideColors());
            int angle = -90, angleDelta = -180 / (primaryColors.Count + 1);
            foreach (var color in primaryColors)
            {
                angle += angleDelta;

                var up = transform.up;
                var originDiff = Quaternion.AngleAxis(angle, up) * diff;
                var direction = Vector3.ProjectOnPlane(originDiff, up);
                EmitBeam(color, transform.position + originDiff, direction);
            }
        }
    }
}
