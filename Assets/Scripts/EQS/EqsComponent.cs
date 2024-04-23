using System.Collections.Generic;
using UnityEngine;

namespace SuperKatanaTiger.EQS
{
    [RequireComponent(typeof(LineRenderer))]
    public class EqsComponent : MonoBehaviour, IEqsGenerator
    {
        [SerializeField] private float radius = 1;
        [SerializeField] private float rate;
        [SerializeField] private float distanceBetweenPoints = 1f;
        [SerializeField] private LayerMask detectionLayerMask;
        [SerializeField] private Transform target;
        [SerializeField] private bool gridAtTarget;

        public float Radius => radius;
        public float Rate => rate;
        public float DistanceBetweenPoints => distanceBetweenPoints;
        public LayerMask DetectionLayerMask => detectionLayerMask;
        public Transform Target => target;

        private LineRenderer _lineRenderer;

        private bool GenerateAtTarget => Target != null && gridAtTarget;

        private void Awake()
        {
            _lineRenderer = GetComponent<LineRenderer>();
        }

        public Vector3 GetBestPoint(Transform t = null)
        {
            var points = GeneratePoints(t);
            EqsPoint bestPoint = points[0];

            foreach (var point in points)
            {
                if (point.Score > bestPoint.Score) bestPoint = point;
            }

            return bestPoint.Position;
        }

        private List<EqsPoint> GeneratePoints(Transform t = null)
        {
            var points = t != null ? EqsGenerator.GetPointsAroundTarget(this, t) : EqsGenerator.GetPoints(this);
            //EqsFilter.FilterByVisibility(ref points, this, 1f, ~LayerMask.GetMask("Ignore Raycast"));
            EqsScorer.WeightScoreByHeight(ref points, t != null ? t.position.y : transform.position.y);
            return points; 
        }

        private void OnDrawGizmos()
        {
            _lineRenderer = GetComponent<LineRenderer>();

            var points = GenerateAtTarget ? GeneratePoints(Target) : GeneratePoints();
            
            var bestPoint = points[0];
            foreach (var point in points)
            {
                if (point.Score > bestPoint.Score) bestPoint = point;
                Gizmos.color = point.IsAvailable ? Color.Lerp(Color.red, Color.green, point.Score) : Color.grey;
                Gizmos.DrawWireSphere(point.Position, 0.5f);
            }

            _lineRenderer.SetPosition(0, bestPoint.Position);
            _lineRenderer.SetPosition(1, bestPoint.Position + Vector3.up * 10f);
        }
    }
}