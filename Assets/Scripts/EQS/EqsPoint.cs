using System.Collections.Generic;
using UnityEngine;

namespace EQS
{
    public class EqsPoint
    {
        public float Score { get; private set; }
        public Vector3 Position { get; private set; }
        public bool IsAvailable { get; private set; }

        private float _rawScore;
        private int _scorersAmount;
        private List<PointScore> _pointScores;

        public EqsPoint(Vector3 position, bool isAvailable)
        {
            Position = position;
            IsAvailable = isAvailable;

            _pointScores = new List<PointScore>();
        }

        public void AddScore(float value, int weight)
        {
            _rawScore += value * weight;
            _scorersAmount += weight;
            SetScore(_rawScore / _scorersAmount);
        }

        public void SetScore(float value) => Score = value;
        public void SetIsAvailable(bool value) => IsAvailable = value;

        private class PointScore
        {
            public readonly float score;
            public readonly float weight;

            public PointScore(float score, float weight)
            {
                this.score = score;
                this.weight = weight;
            }
        }
    }
}