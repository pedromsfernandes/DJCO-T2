using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCollision : MonoBehaviour
{
    Vector3 direction;
    float distance;

    public float minDistance = 1.0f;
    public float maxDistance = 4.0f;
    public float smooth = 10.0f;

    void Start()
    {
        direction = transform.localPosition.normalized;
        distance = transform.localPosition.magnitude;
    }

    void Update()
    {
        if (!GameState.Instance.aiming)
        {
            Vector3 target = transform.parent.TransformPoint(direction * maxDistance);
            RaycastHit hit;

            // mask to avoid collision with player model
            int layerMask = 1 << 8;
            layerMask = ~layerMask;

            if (Physics.Linecast(transform.parent.position, target, out hit, layerMask))
            {
                distance = Mathf.Clamp((hit.distance * 0.9f), minDistance, maxDistance);
            }
            else
            {
                distance = maxDistance;
            }

            transform.localPosition = Vector3.Lerp(transform.localPosition, direction * distance, Time.deltaTime * smooth);
        }
    }
}
