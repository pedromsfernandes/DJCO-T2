using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Light
{
    public abstract class BeamSensor : MonoBehaviour
    {
        protected void Hit(IReadOnlyList<object> args)
        {
            GetComponent<PhotonView>().RPC("HitSelf", args);
        }

        [PunRPC]
        public void HitSelf(IReadOnlyList<object> args)
        {
            LightBeam beam = (LightBeam)args[0];
            RaycastHit hit = (RaycastHit)args[1];
            Vector3 direction = (Vector3)args[2];
            
            OnBeamSense(beam, hit, direction);
        }

        /**
         * Called every frame while a beam is sensed
         */
        protected abstract void OnBeamSense(LightBeam beam, RaycastHit hit, Vector3 reflectedDirection);
    }
}