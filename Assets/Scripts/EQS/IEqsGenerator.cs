using UnityEngine;

namespace SuperKatanaTiger.EQS
{
    public interface IEqsGenerator
    {
        Transform transform { get; }
        Transform Target { get; }
        float Radius { get; }
        float Rate { get; }
        float DistanceBetweenPoints { get; }
        LayerMask DetectionLayerMask { get; }
    }
}