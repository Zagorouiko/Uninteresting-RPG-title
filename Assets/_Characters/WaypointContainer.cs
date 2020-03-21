using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dragon.Character
{
    public class WaypointContainer : MonoBehaviour
    {
        void OnDrawGizmos()
        {
            Vector3 firstPosition = transform.GetChild(0).position;
            Vector3 previousPosition = firstPosition;

            foreach (Transform waypoint in transform)
            {
                Gizmos.DrawSphere(waypoint.position, 1f);
                Gizmos.DrawLine(previousPosition, waypoint.position);
                previousPosition = waypoint.position;
            }
            Gizmos.DrawLine(previousPosition, firstPosition);
        }
    }
}

