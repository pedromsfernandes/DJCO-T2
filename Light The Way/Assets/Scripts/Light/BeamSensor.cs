using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Light
{
    using BeamInfo = Tuple<LightBeam, RaycastHit, Vector3>;
    
    public abstract class BeamSensor : MonoBehaviour
    {
        private readonly List<BeamInfo> _lightBeams = new List<BeamInfo>();
        private IEnumerable<BeamInfo> _previousLightBeams = new List<BeamInfo>();
        
        protected void Hit(IReadOnlyList<object> args)
        {
            LightBeam beam = (LightBeam)args[0];
            RaycastHit hit = (RaycastHit)args[1];
            Vector3 direction = (Vector3)args[2];

            _lightBeams.Add(new BeamInfo(beam, hit, direction));
            OnBeamSense(beam, hit, direction);
        }

        private void LateUpdate()
        {
            foreach (var (beam, hit, direction) in _lightBeams.Except(_previousLightBeams))
                OnBeamSenseStart(beam, hit, direction);
            foreach (var (beam, hit, direction) in _previousLightBeams.Except(_lightBeams))
                OnBeamSenseEnd(beam, hit, direction);

            _previousLightBeams = _lightBeams.ToList();
            _lightBeams.Clear();
        }

        
        /**
         * Called every frame while a beam is sensed
         */
        protected virtual void OnBeamSense(LightBeam beam, RaycastHit hit, Vector3 reflectedDirection)
        {
        }

        /**
         * Called when beam is first sensed
         */
        protected virtual void OnBeamSenseStart(LightBeam beam, RaycastHit hit, Vector3 reflectedDirection)
        {
        }
        
        /**
         * Called when beam is no longer sensed
         */
        protected virtual void OnBeamSenseEnd(LightBeam beam, RaycastHit hit, Vector3 reflectedDirection)
        {
        }
    }
}