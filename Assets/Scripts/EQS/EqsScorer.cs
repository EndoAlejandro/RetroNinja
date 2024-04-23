using System.Collections.Generic;
using System.Linq;
using CustomUtils;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.AI;

namespace EQS
{
    public static class EqsScorer
    {
        public static void ScoreByPathDistance(ref List<EqsPoint> points, Vector3 origin, bool inverse = false,
            int weight = 1)
        {
            var maxDistance = 1f;
            foreach (var point in points)
            {
                var path = new NavMeshPath();
                NavMesh.CalculatePath(origin, point.Position, NavMesh.AllAreas, path);
                var pathLenght = path.GetPathLenght();
                if (pathLenght > maxDistance)
                    maxDistance = pathLenght;
            }

            foreach (var point in points)
            {
                var path = new NavMeshPath();
                NavMesh.CalculatePath(origin, point.Position, NavMesh.AllAreas, path);
                var normalizedPathLenght = path.GetPathLenght() / maxDistance;
                
                if (inverse)
                    point.AddScore(normalizedPathLenght, weight);
                else
                    point.AddScore(1 - normalizedPathLenght, weight);
            }
        }

        public static void ScoreByDistance(ref List<EqsPoint> points, Vector3 origin, bool inverse = false,
            int weight = 1)
        {
            var maxDistance = points.Select(point => Vector3.Distance(point.Position, origin)).Prepend(0f).Max();
            foreach (var point in points)
            {
                if (!point.IsAvailable) continue;

                var distance = Vector3.Distance(point.Position, origin);
                var normalizedDistance = distance / maxDistance;
                if (inverse)
                    point.AddScore(1 - normalizedDistance, weight);
                else
                    point.AddScore(normalizedDistance, weight);
            }
        }
        
        public static void WeightScoreByHeight(ref List<EqsPoint> points, float height)
        {
            var maxHeight = float.MinValue;
            var minHeight = float.MaxValue;
            foreach (var point in points.Where(point => point.IsAvailable))
            {
                if (point.Position.y > maxHeight) maxHeight = point.Position.y;
                if (point.Position.y < minHeight) minHeight = point.Position.y;
            }

            foreach (var point in points)
            {
                if (!point.IsAvailable)
                {
                    point.SetScore(0f);
                    continue;
                }

                var weight = point.Position.y;
                var normalizedWeight = (weight - height) / (maxHeight - height);
                if (float.IsNaN(normalizedWeight)) normalizedWeight = 0.5f;
                point.SetScore(normalizedWeight);
            }
        }
    }
}