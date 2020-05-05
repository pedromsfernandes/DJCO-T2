using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Light
{
    public class Splitter : BeamEmitter
    {
        protected override void OnBeamSense(LightBeam beam, RaycastHit hit, Vector3 direction)
        {
            Vector3 diff = hit.point - transform.position;
            diff.y = 0;

            Vector3 direction1 = Quaternion.AngleAxis(120, Vector3.up) * diff;
            Vector3 direction2 = Quaternion.AngleAxis(-120, Vector3.up) * diff;
            
            Vector3 start1 = direction1 + transform.position;
            start1.y = hit.point.y;
            Vector3 start2 = direction2 + transform.position;
            start2.y = hit.point.y;

            EmitBeam(beam.LightColor, start1, direction1);
            EmitBeam(beam.LightColor, start2, direction2);
        }
    }
}
