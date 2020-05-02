using System;
using UnityEngine;
using Photon.Pun;

namespace Lighting
{
    public class SunController : MonoBehaviour
    {
        public float rotationSpeed = 400;

        private Transform _transform;

        // Start is called before the first frame update
        private void Start()
        {
            _transform = transform;
        }

        private void Update()
        {
            float scrollAxis = Input.GetAxis("Mouse ScrollWheel");

            if (GameState.Instance.canRotateSun && GameState.Instance.hasTool2 && GameState.Instance.currentTool == 2 && Math.Abs(scrollAxis) > 0.01)
            {
                Rotate(scrollAxis, Time.deltaTime, rotationSpeed);
            }
        }

        public void Rotate(float axis, float delta, float speed)
        {
            this.GetComponent<PhotonView>().RPC("RotateSelf", RpcTarget.All, axis, delta, speed);
        }

        [PunRPC]
        void RotateSelf(float axis, float delta, float speed)
        {
            _transform.Rotate(Math.Sign(axis) * delta * speed, 0, 0);
        }
    }
}
