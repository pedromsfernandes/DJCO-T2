using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Light
{
    public class Splitter : BeamEmitter
    {
        public int angle;

        protected override void OnBeamSense(LightBeam beam, RaycastHit hit, Vector3 direction)
        {
            Vector3 diff = Vector3.ProjectOnPlane(hit.point - transform.position, transform.up);

            Vector3 direction1 = Quaternion.AngleAxis(180 - angle/2f, transform.up) * diff;
            Vector3 direction2 = Quaternion.AngleAxis(180 + angle/2f, transform.up) * diff;
            
            Vector3 start1 = direction1 + transform.position + Vector3.Project(hit.point - transform.position, transform.up);
            Vector3 start2 = direction2 + transform.position + Vector3.Project(hit.point - transform.position, transform.up);

            EmitBeam(beam.LightColor, start1, direction1);
            EmitBeam(beam.LightColor, start2, direction2);
        }
    }
}
