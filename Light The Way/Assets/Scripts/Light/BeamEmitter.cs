using UnityEngine;

namespace Light
{
    public abstract class BeamEmitter : BeamSensor
    {
        private Transform _laserPool;
        private int _beamCount = 0;

        protected void Awake()
        { 
            _laserPool = transform.Find("laserPool");
            if (!_laserPool) _laserPool = transform;
        }

        protected LightBeam EmitBeam(LightColor color, Vector3 origin, Vector3 direction)
        {
            _beamCount++;
            
            LightBeam newBeam = _laserPool.childCount < _beamCount
                ? LightBeam.CreateLightBeam(_laserPool, color, origin, direction)
                : LightBeam.UpdateLightBeam(_laserPool.GetChild(_beamCount - 1).gameObject, color, origin, direction);
        
            newBeam.EnableSelf(true);
            return newBeam;
        }

        private void LateUpdate()
        {
            for (int i = _beamCount; i < _laserPool.childCount; i++)
            {
                _laserPool.GetChild(i).gameObject.SetActive(false);
            }
            _beamCount = 0;
        }
    }
}