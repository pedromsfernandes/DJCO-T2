using UnityEngine;

namespace Light
{
    public class Mirror : BeamEmitter
    {
        int numHits = 0;

        //sound
        [FMODUnity.EventRef]
        public string selectedMirrorReflectSound = "event:/FX/Crystal/Mirror/MirrorReflect";
        FMOD.Studio.EventInstance mirrorReflectSoundEvent;

        void Start()
        {
            //sound
            mirrorReflectSoundEvent = FMODUnity.RuntimeManager.CreateInstance(selectedMirrorReflectSound);
        }

        private void Update()
        {
            FMOD.Studio.PLAYBACK_STATE fmodPBState;
            mirrorReflectSoundEvent.getPlaybackState(out fmodPBState);
                                 
            if(numHits > 0 && fmodPBState != FMOD.Studio.PLAYBACK_STATE.PLAYING)
            {
                Rigidbody originalRigidbody = this.GetComponentInChildren<Rigidbody>();
                FMODUnity.RuntimeManager.AttachInstanceToGameObject(mirrorReflectSoundEvent, this.transform, originalRigidbody);
                mirrorReflectSoundEvent.start();
            }
            else if(numHits == 0 && fmodPBState == FMOD.Studio.PLAYBACK_STATE.PLAYING)
            {
                mirrorReflectSoundEvent.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            }

            numHits = 0;
        }

        protected override void OnBeamSense(LightBeam beam, RaycastHit hit, Vector3 direction)
        {
            EmitBeam(beam.LightColor, hit.point, Vector3.Reflect(direction, hit.normal));

            numHits++;
        }
    }
}
