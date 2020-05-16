using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

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

    Transform playerTransform;
    Rigidbody playerRigidbody;

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

        playerTransform = GetComponent<Transform>();
        playerRigidbody = GetComponentInChildren<Rigidbody>();

        PlayBeamSoundSelf(playerTransform, playerRigidbody);
        
    }

    void PlayBeamSoundSelf(Transform playerTransform, Rigidbody playerRigidbody)
    {
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(redBeamSoundEvent, playerTransform, playerRigidbody);
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(greenBeamSoundEvent, playerTransform, playerRigidbody);
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(blueBeamSoundEvent, playerTransform, playerRigidbody);

        if(GameState.Instance.currentTool == 1)
        {
            if (GameState.Instance.castingRay)
            {
                FMOD.Studio.PLAYBACK_STATE fmodPBState;
                redBeamSoundEvent.getPlaybackState(out fmodPBState);
                if (fmodPBState != FMOD.Studio.PLAYBACK_STATE.PLAYING)
                {
                    redBeamSoundEvent.start();
                }
            }
            else
            {
                redBeamSoundEvent.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            }
        }
        else if (GameState.Instance.currentTool == 2)
        {
            if (GameState.Instance.castingRay)
            {
                FMOD.Studio.PLAYBACK_STATE fmodPBState;
                greenBeamSoundEvent.getPlaybackState(out fmodPBState);
                if (fmodPBState != FMOD.Studio.PLAYBACK_STATE.PLAYING)
                {
                    greenBeamSoundEvent.start();
                }
            }
            else
            {
                greenBeamSoundEvent.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            }
        }
        else if (GameState.Instance.currentTool == 3)
        {
            if (GameState.Instance.castingRay)
            {
                FMOD.Studio.PLAYBACK_STATE fmodPBState;
                blueBeamSoundEvent.getPlaybackState(out fmodPBState);
                if (fmodPBState != FMOD.Studio.PLAYBACK_STATE.PLAYING)
                {
                    blueBeamSoundEvent.start();
                }
            }
            else
            {
                blueBeamSoundEvent.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            }
        }



    }

    /*public void playBeamSound(string toolName)
    {
        //Debug.Log("Photon destroy tool " + toolName);
        //this.GetComponent<PhotonView>().RPC("deletePickedUpToolSelf", RpcTarget.All, toolName);
    }

    [PunRPC]
    void deletePickedUpToolSelf(string toolName)
    {
        //Debug.Log("Destroying Tool " + toolName);
        //GameObject tool = GameObject.Find(toolName);
        //Destroy(tool);
    }*/
}
