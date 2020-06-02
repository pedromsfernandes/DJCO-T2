using UnityEngine;

namespace Light
{
    public class Mirror : BeamEmitter
    {
        protected override void OnBeamSense(LightColor beam, Vector3 point, Vector3 normal, Vector3 direction)
        {
            EmitBeam(beam, point, Vector3.Reflect(direction, normal));
        }
    }
}
