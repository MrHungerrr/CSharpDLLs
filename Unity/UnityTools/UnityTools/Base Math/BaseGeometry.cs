using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


namespace Vkimow.Unity.Tools.Math
{
    public static class BaseGeometry
    {

        public static Quaternion GetQuaternionToY(Transform from, Vector3 to)
        {
            Quaternion targetRotation = GetQuaternionTo(from.position, to); ;
            targetRotation.z = from.rotation.z;
            targetRotation.x = from.rotation.x;
            return targetRotation.normalized;
        }

        public static Quaternion GetQuaternionTo(Vector3 from, Vector3 to)
        {
            Vector3 direct = to - from;
            Quaternion targetRotation = Quaternion.LookRotation(direct);
            return targetRotation.normalized;
        }


        public static float LookingAngle2D(Transform who, Vector3 lookingTo)
        {
            lookingTo.y = who.position.y;
            return LookingAngle(who, lookingTo);
        }

        public static float LookingAngle(Transform who, Vector3 lookingTo)
        {
            lookingTo = lookingTo - who.position;
            return Vector3.Angle(lookingTo, who.forward);
        }


        public static Vector3 GetDirection2D(Vector3 from, Vector3 to)
        {
            return new Vector3(to.x - from.x, 0, to.z - from.z).normalized;
        }


        public static Vector3 GetDirection(Vector3 from, Vector3 to)
        {
            return new Vector3(to.x - from.x, to.y - from.y, to.z - from.z).normalized;
        }



        public static float GetRealDistance(NavMeshAgent agent, Vector3 start, Vector3 goal)
        {
            NavMeshPath path = new NavMeshPath();

            agent.CalculatePath(goal, path);

            Vector3[] allWayPoints = new Vector3[path.corners.Length + 2];
            allWayPoints[0] = start;
            allWayPoints[allWayPoints.Length - 1] = goal;

            for (int i = 0; i < path.corners.Length; i++)
            {
                allWayPoints[i + 1] = path.corners[i];
            }

            float result = 0f;

            for (int i = 0; i < allWayPoints.Length - 1; i++)
            {
                result += Vector3.Distance(allWayPoints[i], allWayPoints[i + 1]);
            }

            return result;
        }
    }
}