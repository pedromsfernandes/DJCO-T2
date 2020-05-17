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

        PlayBeamSound(playerTransform, playerRigidbody);
    }


    [PunRPC]
    void PlayRedBeamSoundSelf(bool play, string originalPlayerName)
    {


        if(playerTransform.name != originalPlayerName)
            return;

        Debug.Log("Play Red Beam Sound: " + play + " / On + " + originalPlayerName + " == " + playerTransform.name);

        GameObject originalPlayer = GameObject.Find(originalPlayerName);
        Transform originalTransform = originalPlayer.GetComponent<Transform>();
        Rigidbody originalRigidbody = originalPlayer.GetComponentInChildren<Rigidbody>();
        //FMODUnity.RuntimeManager.AttachInstanceToGameObject(redBeamSoundEvent, playerTransform, playerRigidbody);

        Debug.Log("Position = " + originalTransform.position);
        Debug.Log("Velocity = " + originalRigidbody.velocity);

        FMODUnity.RuntimeManager.AttachInstanceToGameObject(redBeamSoundEvent, originalTransform, originalRigidbody);

        if (play)
        {
            redBeamSoundEvent.start();
        }
        else
        {
            redBeamSoundEvent.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        }
    }


    void PlayBeamSound(Transform playerTransform, Rigidbody playerRigidbody)
    {
        
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(greenBeamSoundEvent, playerTransform, playerRigidbody);
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(blueBeamSoundEvent, playerTransform, playerRigidbody);

        if (GameState.Instance.currentTool == 1)
        {
            if (GameState.Instance.castingRay)
            {
                FMOD.Studio.PLAYBACK_STATE fmodPBState;
                redBeamSoundEvent.getPlaybackState(out fmodPBState);
                if (fmodPBState != FMOD.Studio.PLAYBACK_STATE.PLAYING)
                {
                    //redBeamSoundEvent.start();
                    if(GetComponent<PhotonView>().IsMine){
                        GetComponent<PhotonView>().RPC("PlayRedBeamSoundSelf", RpcTarget.All, true,  playerTransform.name);
                    }
                }
            }
            else
            {
                FMOD.Studio.PLAYBACK_STATE fmodPBState;
                redBeamSoundEvent.getPlaybackState(out fmodPBState);
                if (fmodPBState == FMOD.Studio.PLAYBACK_STATE.PLAYING)
                {
                    //redBeamSoundEvent.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                     if(GetComponent<PhotonView>().IsMine){
                        GetComponent<PhotonView>().RPC("PlayRedBeamSoundSelf", RpcTarget.All, false,  playerTransform.name);
                     }
                }
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


}
