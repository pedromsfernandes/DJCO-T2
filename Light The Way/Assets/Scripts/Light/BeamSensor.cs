using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Photon.Pun;

namespace Light
{
    public abstract class BeamSensor : MonoBehaviour
    {
        protected void Hit(IReadOnlyList<object> args)
        {
            GetComponent<PhotonView>().RPC("HitSelf", RpcTarget.All, args);
        }

        [PunRPC]
        public void HitSelf(IReadOnlyList<object> args)
        {
            LightColor beam = LightColor.Of((LightType)args[0]);
            Vector3 point = (Vector3)args[1];
            Vector3 normal = (Vector3)args[2];
            Vector3 direction = (Vector3)args[3];
            
            OnBeamSense(beam, point, normal, direction);
        }

        /**
         * Called every frame while a beam is sensed
         */
        protected abstract void OnBeamSense(LightColor beam, Vector3 point, Vector3 normal, Vector3 reflectedDirection);
    }
}