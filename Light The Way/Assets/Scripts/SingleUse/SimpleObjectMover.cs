using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Voice.Unity;
using Photon.Voice.PUN;

public class SimpleObjectMover : MonoBehaviourPun
{
    private Animator _animator;

    [SerializeField]
    private float _moveSpeed = 1f;

    private PhotonVoiceView _photonVoiceView;

    private string value;


    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _photonVoiceView = GetComponentInParent<PhotonVoiceView>();
        value = "player" + Random.Range(0, 9999);
    }

    // Update is called once per frame
    void Update()
    {
        if (base.photonView.IsMine)
        {
            float x = Input.GetAxisRaw("Horizontal");
            float y = Input.GetAxisRaw("Vertical");
            transform.position += (new Vector3(x, y, 0f) * _moveSpeed) * Time.deltaTime;

            UpdateMovingBoolean((x != 0f || y != 0f));
        }

        Debug.Log(_photonVoiceView.IsSpeaking);
        MasterManager.UpdatePlayer(value, _photonVoiceView.IsSpeaking);
    }

    private void UpdateMovingBoolean(bool moving)
    {
        _animator.SetBool("Moving", moving);
    }
}
