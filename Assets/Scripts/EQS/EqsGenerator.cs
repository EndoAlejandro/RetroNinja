using System;
using System.Collections.Generic;
using CustomUtils;
using UnityEngine;

namespace SuperKatanaTiger.EQS
{
    public static class EqsGenerator
    {
        /// <summary>
        /// Return the EQS point with the highest Score.
        /// </summary>
        /// <param name="points">List of EQS points to evaluate.</param>
        /// <returns></returns>
        public static EqsPoint BestPoint(List<EqsPoint> points)
        {
            EqsPoint bestPoint = null;

            foreach (var point in points)
            {
                if (!point.IsAvailable) continue;
                if ((bestPoint == null || point.Score > bestPoint.Score) && !point.AssignedTransform)
                    bestPoint = point;
            }

            return bestPoint;
        }

        /// <summary>
        /// Generate all the EQS points based on a target.
        /// </summary>
        /// <param name="origin">points will be generated around this target.</param>
        /// <param name="radius">Max distance that an EQS can have from origin.</param>
        /// <param name="distanceBetweenPoints">Distance between each EQS.</param>
        /// <param name="layerMask">Ground mask for height.</param>
        /// <returns>List of unfiltered EQS points.</returns>
        public static List<EqsPoint> GetPoints(Transform origin, float radius, float distanceBetweenPoints,
            LayerMask layerMask)
        {
            var points = new List<EqsPoint>();
            for (float i = -radius; i < radius; i += distanceBetweenPoints)
            {
                for (float j = -radius; j < radius; j += distanceBetweenPoints)
                {
                    // X Z Position.
                    var pointPosition = new Vector3(
                        -(distanceBetweenPoints * i),
                        0f,
                        -(distanceBetweenPoints * j));

                    pointPosition += origin.position;
                    var distance = Vector3.Distance(pointPosition, origin.position);
                    if (distance > radius) continue;

                    // Y Position.
                    var height = GetHeight(radius, pointPosition, layerMask);
                    pointPosition.y = height ?? pointPosition.y;

                    // Evaluate NavMesh for better position.
                    var navMeshPoint = Utils.PointInPath(origin.position, pointPosition);

                    // Add point to EQS.
                    var eqsPoint = new EqsPoint(navMeshPoint ?? pointPosition, navMeshPoint != null);
                    points.Add(eqsPoint);
                }
            }

            return points;
        }

        /// <summary>
        /// Search for the ground height in a particular position.
        /// </summary>
        /// <param name="radius"></param>
        /// <param name="pointPosition"></param>
        /// <param name="layerMask"></param>
        /// <returns></returns>
        private static float? GetHeight(float radius, Vector3 pointPosition, LayerMask layerMask)
        {
            var results = new RaycastHit[50];
            var size = Physics.RaycastNonAlloc(pointPosition.With(y: radius), Vector3.down, results,
                radius * 2, layerMask);
            float? groundHeight = null;

            for (int i = 0; i < size; i++)
            {
                if (results[i].transform.CompareTag("Terrain"))
                    groundHeight = results[i].point.y;
            }

            return groundHeight;
        }

        [Obsolete]
        public static List<EqsPoint> GetPoints(IEqsGenerator generator)
        {
            var lenght = Mathf.RoundToInt(generator.Radius / generator.DistanceBetweenPoints) + 1;
            var points = new List<EqsPoint>();

            for (int i = 0; i < lenght; i++)
            {
                for (int j = 0; j < lenght; j++)
                {
                    var pointPosition =
                        generator.transform.forward * -(generator.Radius / 2 - generator.DistanceBetweenPoints * i) +
                        generator.transform.right * -(generator.Radius / 2 - generator.DistanceBetweenPoints * j);

                    pointPosition += generator.transform.position;
                    var height = GetHeight(generator, pointPosition);
                    pointPosition.y = height ?? pointPosition.y;
                    var navMeshPoint = Utils.PointInPath(generator.transform.position, pointPosition);
                    points.Add(new EqsPoint(navMeshPoint ?? pointPosition, navMeshPoint != null));
                }
            }

            return points;
        }

        [Obsolete]
        public static List<EqsPoint> GetPointsAroundTarget(IEqsGenerator generator, Transform target)
        {
            var lenght = Mathf.RoundToInt(generator.Radius / generator.DistanceBetweenPoints) + 1;
            var points = new List<EqsPoint>();
            for (int i = 0; i < lenght; i++)
            {
                for (int j = 0; j < lenght; j++)
                {
                    var pointPosition = target.forward *
                                        -(generator.Radius / 2 - generator.DistanceBetweenPoints * i) +
                                        target.right *
                                        -(generator.Radius / 2 - generator.DistanceBetweenPoints * j);
                    pointPosition += target.position;
                    var height = GetHeight(generator, pointPosition);
                    pointPosition.y = height ?? pointPosition.y;
                    var navMeshPoint = Utils.PointInPath(generator.transform.position, pointPosition);
                    points.Add(new EqsPoint(navMeshPoint ?? pointPosition, navMeshPoint != null));
                }
            }

            return points;
        }

        [Obsolete]
        private static float? GetHeight(IEqsGenerator generator, Vector3 pointPosition)
        {
            RaycastHit[] results = new RaycastHit[50];
            var size = Physics.RaycastNonAlloc(pointPosition.With(y: generator.Radius), Vector3.down, results,
                generator.Radius * 2,
                generator.DetectionLayerMask);
            float? groundHeight = null;
            for (int i = 0; i < size; i++)
            {
                if (results[i].transform.CompareTag("Terrain"))
                    groundHeight = results[i].point.y;
            }

            return groundHeight;
        }
    }
}