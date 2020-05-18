using System;
using Photon.Pun;
using UnityEngine;

namespace Light
{
    public class LightBeam : MonoBehaviour
    {
        [FMODUnity.EventRef]
        public string lightBeamSound;
        public AudioSource crystalAudioSource;

        protected static GameObject BeamModel;

        protected bool Active = false;
        protected Vector3 Origin { get; set; }
        protected Vector3 Direction { get; set; }

        protected LineRenderer Lr;

        public LightColor LightColor { get; private set; }
        private LightColor _stageColor = LightColor.Of(LightType.None);
        private bool _stage;

        public static LightBeam CreateLightBeam(Transform parent, LightColor lightColor, Vector3 origin, Vector3 direction)
        {
            var newBeam = Instantiate(BeamModel, parent, true);
            return UpdateLightBeam(newBeam, lightColor, origin, direction);
        }

        public static LightBeam UpdateLightBeam(GameObject gameObject, LightColor lightColor, Vector3 origin, Vector3 direction)
        {
            return gameObject.GetComponent<LightBeam>().SetupBeam(lightColor, origin, direction);
        }

        public virtual LightBeam UpdateColor(LightColor lightColor)
        {
            LightColor = lightColor;

            if (Lr == null)
                Lr = GetComponent<LineRenderer>();
           
            Lr.startColor = lightColor.GetColor();
            Lr.endColor = lightColor.GetColor();

            return this;
        }

        public void StageAddColor(LightColor lightColor)
        {
            StageColor(_stageColor.AddColor(lightColor));
        }
        
        private void StageColor(LightColor lightColor)
        {
            _stage = true;
            _stageColor = lightColor;
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

        protected virtual void LateUpdate()
        {
            if (_stage)
            {
                UpdateColor(_stageColor);
                
                var emptyColor = LightColor.Of(LightType.None);
                if (LightColor.Equals(emptyColor))
                    _stage = false;
                else
                    StageColor(emptyColor);

                Active = _stage;
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
                        hit.transform.gameObject.SendMessage("Hit", new object[] { this, hit, Direction });
                }
            }
            else
            {
                Lr.SetPosition(1, Direction * 5000);
                GameState.Instance.lastBeamHit = Direction * 5000;
            }


        }

       
        [PunRPC]
        public void EnableSelf(bool op)
        {
            Active = op;
            gameObject.SetActive(op);

        }
    }
}
