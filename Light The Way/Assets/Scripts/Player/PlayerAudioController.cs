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

    [FMODUnity.EventRef]
    public string selectedWalkingSound;
    FMOD.Studio.EventInstance walkingSoundEventPlayer1;
    FMOD.Studio.EventInstance walkingSoundEventPlayer2;
    FMOD.Studio.EventInstance walkingSoundEventPlayer3;

    Transform playerTransform;
    Rigidbody playerRigidbody;

    void Start()
    {
        redBeamSoundEvent = FMODUnity.RuntimeManager.CreateInstance(selectedRedBeamSound);
        greenBeamSoundEvent = FMODUnity.RuntimeManager.CreateInstance(selectedGreenBeamSound);
        blueBeamSoundEvent = FMODUnity.RuntimeManager.CreateInstance(selectedBlueBeamSound);

        walkingSoundEventPlayer1 = FMODUnity.RuntimeManager.CreateInstance(selectedWalkingSound);
        walkingSoundEventPlayer2 = FMODUnity.RuntimeManager.CreateInstance(selectedWalkingSound);
        walkingSoundEventPlayer3 = FMODUnity.RuntimeManager.CreateInstance(selectedWalkingSound);
    }

    void Update()
    {

        playerTransform = GetComponent<Transform>();
        playerRigidbody = GetComponentInChildren<Rigidbody>();

        PlayBeamSound(playerTransform, playerRigidbody);

        PlayWalkingSound();
    }

    #region BeamSound
    void PlayBeamSound(Transform playerTransform, Rigidbody playerRigidbody)
    {
        Debug.Log("Casting Ray? " + GameState.Instance.castingRay);

        if (GameState.Instance.currentTool == 1)
        {
            if (GameState.Instance.castingRay)
            {
                playRedBeamSound(true);
                playGreenBeamSound(false);
                playBlueBeamSound(false);
            }
            else
            {
                playRedBeamSound(false);
            }
        }

        else if (GameState.Instance.currentTool == 2)
        {
            if (GameState.Instance.castingRay)
            {
                playGreenBeamSound(true);
                playRedBeamSound(false);
                playBlueBeamSound(false);
            }
            else
            {
                playGreenBeamSound(false);
            }
        }


        else if (GameState.Instance.currentTool == 3)
        {
            if (GameState.Instance.castingRay)
            {
                playBlueBeamSound(true);
                playRedBeamSound(false);
                playGreenBeamSound(false);
            }
            else
            {
                playBlueBeamSound(false);
            }
        }

        else
        {
            playBlueBeamSound(false);
            playRedBeamSound(false);
            playGreenBeamSound(false);
        }
    }


    

    void playRedBeamSound(bool active)
    {
        FMOD.Studio.PLAYBACK_STATE fmodPBState;
        redBeamSoundEvent.getPlaybackState(out fmodPBState);

        if (active)
        {
            if (fmodPBState != FMOD.Studio.PLAYBACK_STATE.PLAYING && GetComponent<PhotonView>().IsMine)
            {
                GetComponent<PhotonView>().RPC("PlayRedBeamSoundSelf", RpcTarget.All, true, playerTransform.name);
            }
        }
        else
        {
            if (fmodPBState == FMOD.Studio.PLAYBACK_STATE.PLAYING && GetComponent<PhotonView>().IsMine)
            {
                GetComponent<PhotonView>().RPC("PlayRedBeamSoundSelf", RpcTarget.All, false, playerTransform.name);
            }
        }
    }

    void playGreenBeamSound(bool active)
    {
        FMOD.Studio.PLAYBACK_STATE fmodPBState;
        greenBeamSoundEvent.getPlaybackState(out fmodPBState);

        if (active)
        {
            if (fmodPBState != FMOD.Studio.PLAYBACK_STATE.PLAYING && GetComponent<PhotonView>().IsMine)
            {
                GetComponent<PhotonView>().RPC("PlayGreenBeamSoundSelf", RpcTarget.All, true, playerTransform.name);
            }
        }
        else
        {
            if (fmodPBState == FMOD.Studio.PLAYBACK_STATE.PLAYING && GetComponent<PhotonView>().IsMine)
            {
                GetComponent<PhotonView>().RPC("PlayGreenBeamSoundSelf", RpcTarget.All, false, playerTransform.name);
            }
        }
    }

    void playBlueBeamSound(bool active)
    {
        FMOD.Studio.PLAYBACK_STATE fmodPBState;
        blueBeamSoundEvent.getPlaybackState(out fmodPBState);

        if (active)
        {
            if (fmodPBState != FMOD.Studio.PLAYBACK_STATE.PLAYING && GetComponent<PhotonView>().IsMine)
            {
                GetComponent<PhotonView>().RPC("PlayBlueBeamSoundSelf", RpcTarget.All, true, playerTransform.name);
            }
        }
        else
        {
            if (fmodPBState == FMOD.Studio.PLAYBACK_STATE.PLAYING && GetComponent<PhotonView>().IsMine)
            {
                GetComponent<PhotonView>().RPC("PlayBlueBeamSoundSelf", RpcTarget.All, false, playerTransform.name);
            }
        }
    }




    [PunRPC]
    void PlayRedBeamSoundSelf(bool play, string originalPlayerName)
    {
        if (playerTransform.name != originalPlayerName)
            return;
        GameObject originalPlayer = GameObject.Find(originalPlayerName);
        Transform originalTransform = originalPlayer.GetComponent<Transform>();
        Rigidbody originalRigidbody = originalPlayer.GetComponentInChildren<Rigidbody>();

        FMODUnity.RuntimeManager.AttachInstanceToGameObject(redBeamSoundEvent, originalTransform, originalRigidbody);

        Debug.Log("Red Beam Sound " + originalTransform.position + " from " + originalPlayerName + " / active: " + play);

        if (play)
        {
            redBeamSoundEvent.start();
        }
        else
        {
            redBeamSoundEvent.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        }
    }
    
    [PunRPC]
    void PlayGreenBeamSoundSelf(bool play, string originalPlayerName)
    {
        if (playerTransform.name != originalPlayerName)
            return;
        GameObject originalPlayer = GameObject.Find(originalPlayerName);
        Transform originalTransform = originalPlayer.GetComponent<Transform>();
        Rigidbody originalRigidbody = originalPlayer.GetComponentInChildren<Rigidbody>();

        FMODUnity.RuntimeManager.AttachInstanceToGameObject(greenBeamSoundEvent, originalTransform, originalRigidbody);

        Debug.Log("Green Beam Sound " + originalTransform.position + " from " + originalPlayerName + " / active: " + play);

        if (play)
        {
            greenBeamSoundEvent.start();
        }
        else
        {
            greenBeamSoundEvent.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        }
    }

    [PunRPC]
    void PlayBlueBeamSoundSelf(bool play, string originalPlayerName)
    {
        if (playerTransform.name != originalPlayerName)
            return;
        GameObject originalPlayer = GameObject.Find(originalPlayerName);
        Transform originalTransform = originalPlayer.GetComponent<Transform>();
        Rigidbody originalRigidbody = originalPlayer.GetComponentInChildren<Rigidbody>();

        FMODUnity.RuntimeManager.AttachInstanceToGameObject(blueBeamSoundEvent, originalTransform, originalRigidbody);

        Debug.Log("Blue Beam Sound " + originalTransform.position + " from " + originalPlayerName + " / active: " + play);

        if (play)
        {
            blueBeamSoundEvent.start();
        }
        else
        {
            blueBeamSoundEvent.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        }
    }
    #endregion

    #region Footsteps
    void PlayWalkingSound()
    {
        FMOD.Studio.PLAYBACK_STATE fmodPBState;
        if(playerTransform.name == "Player(Clone)")
        {
            walkingSoundEventPlayer1.getPlaybackState(out fmodPBState);
        }
        else if (playerTransform.name == "Player2(Clone)")
        {
            walkingSoundEventPlayer2.getPlaybackState(out fmodPBState);
        }
        else
        {
            walkingSoundEventPlayer3.getPlaybackState(out fmodPBState);
        }

        if (GameState.Instance.moving)
        {
            if(fmodPBState != FMOD.Studio.PLAYBACK_STATE.PLAYING && GetComponent<PhotonView>().IsMine)
            {
                Debug.Log("Moving");
                GetComponent<PhotonView>().RPC("PlayWalkingSoundSelf", RpcTarget.All, true, playerTransform.name);
            }
        }
        else
        {
            if (fmodPBState == FMOD.Studio.PLAYBACK_STATE.PLAYING && GetComponent<PhotonView>().IsMine)
            {
                Debug.Log("STOP Moving");
                GetComponent<PhotonView>().RPC("PlayWalkingSoundSelf", RpcTarget.All, false, playerTransform.name);
            }
        }
    }

    [PunRPC]
    void PlayWalkingSoundSelf(bool play, string originalPlayerName)
    {
        if (playerTransform.name != originalPlayerName)
            return;
        GameObject originalPlayer = GameObject.Find(originalPlayerName);
        Transform originalTransform = originalPlayer.GetComponent<Transform>();
        Rigidbody originalRigidbody = originalPlayer.GetComponentInChildren<Rigidbody>();

        Debug.Log("Moving Sound " + originalTransform.position + " from " + originalPlayerName + " / active: " + play);

        if (playerTransform.name == "Player(Clone)")
        {
            FMODUnity.RuntimeManager.AttachInstanceToGameObject(walkingSoundEventPlayer1, originalTransform, originalRigidbody);

            if (play)
            {
                walkingSoundEventPlayer1.start();
            }
            else
            {
                walkingSoundEventPlayer1.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            }
        }
        else if (playerTransform.name == "Player2(Clone)")
        {
            FMODUnity.RuntimeManager.AttachInstanceToGameObject(walkingSoundEventPlayer2, originalTransform, originalRigidbody);

            if (play)
            {
                walkingSoundEventPlayer2.start();
            }
            else
            {
                walkingSoundEventPlayer2.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            }
        }
        else
        {
            FMODUnity.RuntimeManager.AttachInstanceToGameObject(walkingSoundEventPlayer3, originalTransform, originalRigidbody);

            if (play)
            {
                walkingSoundEventPlayer3.start();
            }
            else
            {
                walkingSoundEventPlayer3.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            }
        }
    }
    #endregion
}
