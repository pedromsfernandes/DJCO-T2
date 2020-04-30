using Photon.Pun;
using UnityEngine;

namespace Light
{
    public class LightBeam : MonoBehaviour
    {
        protected static GameObject BeamModel;

        protected LightColor _lightColor;
        protected LineRenderer _lr;

        protected bool Active = false;
        protected Vector3 Origin { private get; set; }
        protected Vector3 Direction { private get; set; }
        
        public static LightBeam CreateLightBeam(Transform parent, LightColor lightColor, Vector3 origin, Vector3 direction)
        {
            var newBeam = Instantiate(BeamModel, parent, true);
            return UpdateLightBeam(newBeam, lightColor, origin, direction);
        }
        
        public static LightBeam UpdateLightBeam(GameObject gameObject, LightColor lightColor, Vector3 origin, Vector3 direction)
        { 
            return gameObject.GetComponent<LightBeam>().SetupBeam(lightColor, origin, direction);
        }

        private LightBeam SetupBeam(LightColor lightColor, Vector3 origin, Vector3 direction)
        {
            _lightColor = gameObject.AddComponent<LightColor>().SetColor(lightColor);
            _lr = GetComponent<LineRenderer>();
        
            _lr.startColor = _lightColor.GetColor();
            _lr.endColor = _lightColor.GetColor();
            
            Origin = origin;
            Direction = direction;
            return this;
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
            _lr.SetPosition(0, Origin);
            if (Physics.Raycast(Origin, Direction, out RaycastHit hit))
            {
                if (hit.collider)
                {
                    _lr.SetPosition(1, hit.point);
                    GameState.Instance.lastBeamHit = hit.point;
                    if (hit.transform.gameObject.layer == LayerMask.NameToLayer("LightHit"))
                        hit.transform.gameObject.SendMessage("Hit", new object[] {this, hit, Direction});
                }
            }
            else
            {
                _lr.SetPosition(1, Direction * 5000);
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
