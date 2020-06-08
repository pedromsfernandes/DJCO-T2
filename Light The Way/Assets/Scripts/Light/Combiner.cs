using UnityEngine;

namespace Light
{
    public class Combiner : BeamSensor
    {
        private LightBeam _combinerBeam;

        int numHits = 0;

        //sound
        [FMODUnity.EventRef]
        public string selectedCombinerReflectSound = "event:/FX/Crystal/Combiner/CombinerReflect";
        FMOD.Studio.EventInstance combinerReflectSoundEvent;

        private void Start()
        {
            _combinerBeam = transform.GetChild(0).GetComponent<LightBeam>();
            LightBeam.UpdateLightBeam(_combinerBeam.gameObject, LightColor.Of(LightType.None), 
                _combinerBeam.gameObject.transform.position, transform.forward);
            
            _combinerBeam.gameObject.SetActive(true);

            //sound
            combinerReflectSoundEvent = FMODUnity.RuntimeManager.CreateInstance(selectedCombinerReflectSound);
        }

        private void Update()
        {
            FMOD.Studio.PLAYBACK_STATE fmodPBState;
            combinerReflectSoundEvent.getPlaybackState(out fmodPBState);

            if (numHits > 0 && fmodPBState != FMOD.Studio.PLAYBACK_STATE.PLAYING)
            {
                Rigidbody originalRigidbody = this.GetComponentInChildren<Rigidbody>();
                FMODUnity.RuntimeManager.AttachInstanceToGameObject(combinerReflectSoundEvent, this.transform, originalRigidbody);
                combinerReflectSoundEvent.start();
            }
            else if (numHits == 0 && fmodPBState == FMOD.Studio.PLAYBACK_STATE.PLAYING)
            {
                combinerReflectSoundEvent.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            }

            numHits = 0;
        }

        protected override void OnBeamSense(LightBeam beam, RaycastHit hit, Vector3 reflectedDirection)
        {
            _combinerBeam.StageAddColor(beam.LightColor);

            numHits++;
        }
    }
}