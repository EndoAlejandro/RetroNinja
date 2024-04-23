using System;
using SuperKatanaTiger.EQS;
using UnityEngine;

namespace SuperKatanaTiger.Enemies
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(Collider))]
    public class Enemy : MonoBehaviour
    {
        public event Action DamageTaken;
        public Vector3 Direction { get; private set; }
        public float TakeDamageTime => takeDamageTime;
        public bool Stunned { get; private set; }
        public float Radius => radius;
        public EqsPoint CurrentPoint { get; private set; }

        [SerializeField] private float takeDamageTime = 1f;
        [SerializeField] private float velocity;
        [SerializeField] private float deceleration;
        [SerializeField] private float acceleration;
        [SerializeField] private float radius = 5f;

        private Collider _collider;
        private Rigidbody _rigidbody;
        private Vector3 _targetVelocity;


        private void Awake()
        {
            _collider = GetComponent<Collider>();
            _rigidbody = GetComponent<Rigidbody>();
        }

        public void TakeDamage(Vector3 direction)
        {
            if (Stunned) return;
            Direction = direction;
            Stunned = true;
            DamageTaken?.Invoke();
        }

        public void EndStun() => Stunned = false;

        private void FixedUpdate() => _rigidbody.velocity = _targetVelocity;

        public void Move(Vector3 value)
        {
            _targetVelocity = value == Vector3.zero
                ? Vector3.MoveTowards(_rigidbody.velocity, Vector3.zero,
                    Time.deltaTime * deceleration)
                : Vector3.MoveTowards(_rigidbody.velocity, value * velocity,
                    Time.deltaTime * acceleration);
        }

        public void SetNewPoint(EqsPoint newPoint)
        {
            CurrentPoint?.AssignTransform(null);
            CurrentPoint = newPoint;
            CurrentPoint?.AssignTransform(transform);
        }
    }
}