﻿using System;
using UnityEngine;

namespace CustomUtils
{
    public static class Utils
    {
        public static Vector3 BallisticVelocity(Vector3 from, Vector3 to, float angle)
        {
            var dir = to - from;           // get target direction
            var h = dir.y;                 // get height difference
            dir.y = 0;                     // retain only the horizontal direction
            var dist = dir.magnitude;      // get horizontal distance
            var a = angle * Mathf.Deg2Rad; // convert angle to radians
            dir.y = dist * Mathf.Tan(a);   // set dir to the elevation angle
            dist += h / Mathf.Tan(a);      // correct for small height differences
            // calculate the velocity magnitude
            var vel = Mathf.Sqrt(dist * Physics.gravity.magnitude / Mathf.Sin(2 * a));
            return vel * dir.normalized;
        }

        public static Vector3[] GetFanPatternDirections(Transform source, int segments, float angle)
        {
            if (segments <= 0 || angle <= 0) return Array.Empty<Vector3>();

            angle = (angle / segments) * (segments - 1);
            var positions = new Vector3[segments];
            var proportion = angle / (segments - 1);
            int index = 0;
            for (float i = -angle / 2; i <= angle / 2; i += proportion)
            {
                var radI = Mathf.Deg2Rad * (i - source.localRotation.eulerAngles.y + 90f);
                var linePos = new Vector3(Mathf.Cos(radI), 0f, Mathf.Sin(radI));
                positions[index] = linePos.normalized;
                index++;
            }

            return positions;
        }

        public static Vector3 FlatDirection(Vector3 to, Vector3 from)
        {
            to.y = 0f;
            from.y = 0f;
            return to - from;
        }

        public static Vector3 NormalizedFlatDirection(Vector3 to, Vector3 from) => FlatDirection(to, from).normalized;
        
        public static float NormalizedDotProduct(Vector3 a, Vector3 b)
        {
            var dot = Vector3.Dot(a, b);
            return (dot + 1) / 2f;
        }
        
        public static Vector3? PointInPath(Vector3 origin, Vector3 target)
        {
            if (!UnityEngine.AI.NavMesh.SamplePosition(target, out UnityEngine.AI.NavMeshHit hit, 1f, UnityEngine.AI.NavMesh.AllAreas)) return null;

            var path = new UnityEngine.AI.NavMeshPath();
            UnityEngine.AI.NavMesh.CalculatePath(origin, hit.position, UnityEngine.AI.NavMesh.AllAreas, path);
            if (path.status != UnityEngine.AI.NavMeshPathStatus.PathComplete) return null;

            return hit.position;
        }
    }
}