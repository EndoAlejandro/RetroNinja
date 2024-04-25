using System;
using System.Collections;
using SuperKatanaTiger.EQS;
using SuperKatanaTiger.Pooling;
using UnityEngine;

namespace SuperKatanaTiger.Enemies
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(Collider))]
    public class Enemy : MonoBehaviour, ITakeDamage
    {
        public static event Action Death;
        public event Action DamageTaken;
        public Vector3 Direction { get; private set; }
        public float TakeDamageTime => takeDamageTime;
        public bool Stunned { get; private set; }
        public bool IsAlive => _health > 0f;
        public float Radius => radius;
        public EqsPoint CurrentPoint { get; private set; }
        public float DetectionRadius => detectionRadius;

        [SerializeField] private float maxHealth;
        [SerializeField] private float takeDamageTime = 1f;
        [SerializeField] private float velocity;
        [SerializeField] private float deceleration;
        [SerializeField] private float acceleration;
        [SerializeField] private float radius = 5f;
        [SerializeField] private float detectionRadius = 5f;
        [SerializeField] private new Renderer renderer;
        [SerializeField] private PoolAfterSeconds deathFx;

        private Collider _collider;
        private Rigidbody _rigidbody;
        public Vector3 TargetVelocity { get; private set; }
        private float _health;

        private void Awake()
        {
            _collider = GetComponent<Collider>();
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void OnEnable()
        {
            _health = maxHealth;
        }

        public void TakeDamage(Vector3 direction)
        {
            if (Stunned || !IsAlive) return;
            Direction = direction;
            Stunned = true;
            _health--;

            DamageTaken?.Invoke();
            if (_health <= 0f) StartCoroutine(DeathAsync());
        }

        private IEnumerator DeathAsync()
        {
            Death?.Invoke();
            renderer.enabled = false;
            _collider.enabled = false;
            deathFx.Get<PoolAfterSeconds>(transform.position + Vector3.up * .5f, Quaternion.identity);
            yield return new WaitForSeconds(2f);
            Destroy(gameObject);
        }

        public void EndStun() => Stunned = false;

        private void FixedUpdate() => _rigidbody.velocity = TargetVelocity;

        public void Move(Vector3 value)
        {
            TargetVelocity = value == Vector3.zero
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

        private void OnDrawGizmos() => Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}