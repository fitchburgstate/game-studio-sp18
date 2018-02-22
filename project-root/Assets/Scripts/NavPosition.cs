using UnityEngine;
using UnityEngine.AI;

namespace Interactable
{
    public class NavPosition 
    {
        public bool RandomPoint(Vector3 center, float range, out Vector3 result)
        {
            for (var i = 0; i < 30; i++)
            {
                var randomPoint = center + Random.insideUnitSphere * range;
                NavMeshHit hit;
                if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
                {
                    result = hit.position;
                    return true;
                }
            }
            result = Vector3.zero;
            return false;
        }
    }
}

