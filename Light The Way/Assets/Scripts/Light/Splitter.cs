using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Light
{
    public class Splitter : BeamEmitter
    {
        public int angle;

        protected override void OnBeamSense(LightColor beam, Vector3 point, Vector3 normal, Vector3 direction)
        {
            Vector3 diff = Vector3.ProjectOnPlane(point - transform.position, transform.up);

            Vector3 direction1 = Quaternion.AngleAxis(180 - angle/2f, transform.up) * diff;
            Vector3 direction2 = Quaternion.AngleAxis(180 + angle/2f, transform.up) * diff;
            
            Vector3 start1 = direction1 + transform.position + Vector3.Project(point - transform.position, transform.up);
            Vector3 start2 = direction2 + transform.position + Vector3.Project(point - transform.position, transform.up);

            EmitBeam(beam, start1, direction1);
            EmitBeam(beam, start2, direction2);
        }
    }
}
