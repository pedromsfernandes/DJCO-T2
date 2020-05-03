using Photon.Pun;
using UnityEngine;

namespace Light
{
    public class LightBeam : MonoBehaviour
    {
        protected static GameObject BeamModel;

        protected bool Active = false;
        protected Vector3 Origin { private get; set; }
        protected Vector3 Direction { private get; set; }

        protected LineRenderer Lr;
        public LightColor LightColor { get; private set; }

        public static LightBeam CreateLightBeam(Transform parent, LightColor lightColor, Vector3 origin, Vector3 direction)
        {
            var newBeam = Instantiate(BeamModel, parent, true);
            return UpdateLightBeam(newBeam, lightColor, origin, direction);
        }
        
        public static LightBeam UpdateLightBeam(GameObject gameObject, LightColor lightColor, Vector3 origin, Vector3 direction)
        {
            return gameObject.GetComponent<LightBeam>().SetupBeam(lightColor, origin, direction);
        }

        public void AddColorRpc(LightColor lightColor)
        {
            GetComponent<PhotonView>().RPC("AddColorSelf", RpcTarget.All, lightColor.Type);
        }
        
        public void RemoveColorRpc(LightColor lightColor)
        {
            GetComponent<PhotonView>().RPC("RemoveColorSelf", RpcTarget.All, lightColor.Type);
        }

        [PunRPC]
        private void AddColorSelf(int colorType)
        {
            UpdateColor(LightColor.AddColor(LightColor.Of((LightType) colorType)));
        }
        
        [PunRPC]
        private void RemoveColorSelf(int colorType)
        {
            UpdateColor(LightColor.RemoveColor(LightColor.Of((LightType) colorType)));
        }
        
        public LightBeam UpdateColor(LightColor lightColor)
        {
            LightColor = lightColor;
            Lr.startColor = lightColor.GetColor();
            Lr.endColor = lightColor.GetColor();
            
            return this;
        }

        private LightBeam SetupBeam(LightColor lightColor, Vector3 origin, Vector3 direction)
        {
            Lr = GetComponent<LineRenderer>();
            Origin = origin;
            Direction = direction;
            
            return UpdateColor(lightColor);
        }

        protected virtual void Update()
        {
            if (Active)
            {
                ProcessRayBeam();
            }
        }

        protected void ProcessRayBeam()
        {
            Lr.SetPosition(0, Origin);
            if (Physics.Raycast(Origin, Direction, out RaycastHit hit))
            {
                if (hit.collider)
                {
                    Lr.SetPosition(1, hit.point);
                    GameState.Instance.lastBeamHit = hit.point;
                    if (hit.transform.gameObject.layer == LayerMask.NameToLayer("LightHit"))
                        hit.transform.gameObject.SendMessage("Hit", new object[] {this, hit, Direction});
                }
            }
            else
            {
                Lr.SetPosition(1, Direction * 5000);
            }
        }

        [PunRPC]
        public void Enable(bool op)
        {
            Active = op;
            gameObject.SetActive(op);
        }
    }
}
