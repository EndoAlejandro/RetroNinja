using System.Collections.Generic;
using SuperKatanaTiger.CustomUtils;
using SuperKatanaTiger.PlayerComponents;
using UnityEngine;

namespace SuperKatanaTiger.EQS
{
    public class EqsManager : Singleton<EqsManager>
    {
        [SerializeField] private Player player;
        [SerializeField] private float refreshRate = 1f;
        [SerializeField] private float radius = 50f;
        [SerializeField] private float distanceBetweenPoints = 1f;
        [SerializeField] private LayerMask layerMask;
        [SerializeField] private bool debug;

        public EqsPoint GetClosestPointToPlayer(Transform source, float maxDistance)
        {
            var points = GetPoints();
            EqsFilter.FilterByVisibility(ref points, player.transform, Vector3.up * .5f);

            EqsScorer.ScoreByRadiusDistance(ref points, player.transform.position, maxDistance, weight: 1);
            EqsScorer.ScoreByPathDistance(ref points, source.position, weight: 2);

            var bestPoint = EqsGenerator.BestPoint(points);
            return bestPoint;
        }

        public Vector3 GetClosestPositionToPlayer(Transform source, float maxDistance) =>
            GetClosestPointToPlayer(source, maxDistance)?.Position ?? source.position;

        private List<EqsPoint> GetPoints() =>
            EqsGenerator.GetPoints(player.transform, radius, distanceBetweenPoints, layerMask);

        private void OnDrawGizmos()
        {
            if (!debug) return;
            var points = GetPoints();
            EqsFilter.FilterByVisibility(ref points, player.transform, Vector3.up * .5f);
            foreach (var point in points)
            {
                Gizmos.color = point.IsAvailable ? Color.Lerp(Color.red, Color.green, point.Score) : Color.black;
                Gizmos.DrawSphere(point.Position + Vector3.up, .25f);
            }
        }
    }
}