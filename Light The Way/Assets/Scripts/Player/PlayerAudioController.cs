using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudioController : MonoBehaviour
{
    [FMODUnity.EventRef]
    public string selectedSound;
    FMOD.Studio.EventInstance soundEvent;

    // Start is called before the first frame update
    void Start()
    {
        soundEvent = FMODUnity.RuntimeManager.CreateInstance(selectedSound);
    }

    // Update is called once per frame
    void Update()
    {
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(soundEvent, GetComponent<Transform>(), GetComponent<Rigidbody>());
        PlayBeamSound();
        
    }

    void PlayBeamSound()
    {

        if (GameState.Instance.castingRay)
        {
            Debug.Log("Casting Ray");
            FMOD.Studio.PLAYBACK_STATE fmodPBState;
            soundEvent.getPlaybackState(out fmodPBState);
            if (fmodPBState != FMOD.Studio.PLAYBACK_STATE.PLAYING)
            {
                soundEvent.start();
            }
        }
        else
        {
            soundEvent.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        }
    }
}
