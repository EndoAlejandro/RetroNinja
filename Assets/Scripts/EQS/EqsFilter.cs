using System;
using System.Collections.Generic;
using UnityEngine;

namespace SuperKatanaTiger.EQS
{
    public static class EqsFilter
    {
        public static void FilterByVisibility(ref List<EqsPoint> points, Transform target, Vector3 heightOffset,
            bool inverse = false)
        {
            foreach (var point in points)
            {
                if (!point.IsAvailable) continue;

                var result = Physics.Raycast(point.Position + heightOffset, target.position + heightOffset,
                    out RaycastHit hit, 2f /*TODO: Assign vision radius*/) && hit.transform != target;

                if (inverse)
                    point.SetIsAvailable(result);
                else
                    point.SetIsAvailable(!result);
            }
        }

        [Obsolete]
        public static void FilterByVisibility(ref List<EqsPoint> points, IEqsGenerator generator, float height)
        {
            foreach (var point in points)
            {
                if (!point.IsAvailable) continue;

                var source = point.Position + Vector3.up * height;
                var target = generator.Target.position + Vector3.up * height;
                Physics.Linecast(source, target, out var hit);
                if (hit.transform != generator.Target && hit.transform != generator.transform)
                    point.SetIsAvailable(false);
            }
        }
    }
}