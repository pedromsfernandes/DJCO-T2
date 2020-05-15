using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudioController : MonoBehaviour
{
    [FMODUnity.EventRef]
    public string selectedRedBeamSound;
    [FMODUnity.EventRef]
    public string selectedGreenBeamSound;
    [FMODUnity.EventRef]
    public string selectedBlueBeamSound;
    FMOD.Studio.EventInstance redBeamSoundEvent;
    FMOD.Studio.EventInstance greenBeamSoundEvent;
    FMOD.Studio.EventInstance blueBeamSoundEvent;

    // Start is called before the first frame update
    void Start()
    {
        redBeamSoundEvent = FMODUnity.RuntimeManager.CreateInstance(selectedRedBeamSound);
        greenBeamSoundEvent = FMODUnity.RuntimeManager.CreateInstance(selectedGreenBeamSound);
        blueBeamSoundEvent = FMODUnity.RuntimeManager.CreateInstance(selectedBlueBeamSound);
    }

    // Update is called once per frame
    void Update()
    {
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(redBeamSoundEvent, GetComponent<Transform>(), GetComponent<Rigidbody>());
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(greenBeamSoundEvent, GetComponent<Transform>(), GetComponent<Rigidbody>());
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(blueBeamSoundEvent, GetComponent<Transform>(), GetComponent<Rigidbody>());
        PlayBeamSound();
        
    }

    void PlayBeamSound()
    {

        if (GameState.Instance.castingRay)
        {
            if (GameState.Instance.currentTool == 1)
            {
                FMOD.Studio.PLAYBACK_STATE fmodPBState;
                redBeamSoundEvent.getPlaybackState(out fmodPBState);
                if (fmodPBState != FMOD.Studio.PLAYBACK_STATE.PLAYING)
                {
                    redBeamSoundEvent.start();
                }
            }
            else if (GameState.Instance.currentTool == 2)
            {
                FMOD.Studio.PLAYBACK_STATE fmodPBState;
                greenBeamSoundEvent.getPlaybackState(out fmodPBState);
                if (fmodPBState != FMOD.Studio.PLAYBACK_STATE.PLAYING)
                {
                    greenBeamSoundEvent.start();
                }
            }
            else if (GameState.Instance.currentTool == 3)
            {
                FMOD.Studio.PLAYBACK_STATE fmodPBState;
                blueBeamSoundEvent.getPlaybackState(out fmodPBState);
                if (fmodPBState != FMOD.Studio.PLAYBACK_STATE.PLAYING)
                {
                    blueBeamSoundEvent.start();
                }
            }
        }
        else
        {
            redBeamSoundEvent.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            greenBeamSoundEvent.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            blueBeamSoundEvent.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        }
    }
}
