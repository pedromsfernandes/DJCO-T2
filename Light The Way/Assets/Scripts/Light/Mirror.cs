using UnityEngine;

namespace Light
{
    public class Mirror : BeamEmitter
    {
        protected override void OnBeamSense(LightBeam beam, RaycastHit hit, Vector3 direction)
        {
            EmitBeam(beam.GetComponent<LightColor>(), hit.point, Vector3.Reflect(direction, hit.normal));
        }
    }
}
