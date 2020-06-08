using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Light
{
    public class Splitter : BeamEmitter
    {
        public int angle;

        int numHits = 0;

        //sound
        [FMODUnity.EventRef]
        public string selectedSplitterReflectSound = "event:/FX/Crystal/Splitter/SplitterReflect";
        FMOD.Studio.EventInstance splitterReflectSoundEvent;

        private void Start()
        {
            //sound
            splitterReflectSoundEvent = FMODUnity.RuntimeManager.CreateInstance(selectedSplitterReflectSound);
        }

        private void Update()
        {
            FMOD.Studio.PLAYBACK_STATE fmodPBState;
            splitterReflectSoundEvent.getPlaybackState(out fmodPBState);

            if (numHits > 0 && fmodPBState != FMOD.Studio.PLAYBACK_STATE.PLAYING)
            {
                Rigidbody originalRigidbody = this.GetComponentInChildren<Rigidbody>();
                FMODUnity.RuntimeManager.AttachInstanceToGameObject(splitterReflectSoundEvent, this.transform, originalRigidbody);
                splitterReflectSoundEvent.start();
            }
            else if (numHits == 0 && fmodPBState == FMOD.Studio.PLAYBACK_STATE.PLAYING)
            {
                splitterReflectSoundEvent.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            }

            numHits = 0;
        }

        protected override void OnBeamSense(LightBeam beam, RaycastHit hit, Vector3 direction)
        {
            Vector3 diff = Vector3.ProjectOnPlane(hit.point - transform.position, transform.up);

            Vector3 direction1 = Quaternion.AngleAxis(180 - angle/2f, transform.up) * diff;
            Vector3 direction2 = Quaternion.AngleAxis(180 + angle/2f, transform.up) * diff;
            
            Vector3 start1 = direction1 + transform.position + Vector3.Project(hit.point - transform.position, transform.up);
            Vector3 start2 = direction2 + transform.position + Vector3.Project(hit.point - transform.position, transform.up);

            EmitBeam(beam.LightColor, start1, direction1);
            EmitBeam(beam.LightColor, start2, direction2);

            numHits++;
        }
    }
}
