using System;
using SuperKatanaTiger.CustomUtils;
using SuperKatanaTiger.Input;
using UnityEngine;

namespace SuperKatanaTiger.PlayerComponents
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(CapsuleCollider))]
    public class Player : Singleton<Player>
    {
        public event Action DamageTaken;
        public event Action DamageDeflected;
        public HitBox HitBox => hitBox;
        public Transform AimObject => aimObject;
        public float AttackDuration => attackDuration;
        public float ParryTime => parryTime;
        public bool ParryActive { get; private set; }

        [SerializeField] private float acceleration = 50f;
        [SerializeField] private float deceleration = 100f;
        [SerializeField] private float velocity = 4f;
        [SerializeField] private float rotationSpeed = 10f;
        [SerializeField] private float attackDuration = .5f;

        [SerializeField] private Transform aimObject;
        [SerializeField] private HitBox hitBox;
        [SerializeField] private float parryTime = 1f;

        private Collider _collider;
        private Rigidbody _rigidbody;
        private Vector3 _targetVelocity;

        protected override void Awake()
        {
            base.Awake();
            _collider = GetComponent<Collider>();
            _rigidbody = GetComponent<Rigidbody>();

            _targetVelocity = Vector2.zero;
        }

        private void FixedUpdate()
        {
            AimDirection();
            ApplyVelocity();
        }

        private void ApplyVelocity()
        {
            _rigidbody.velocity = _targetVelocity;
        }

        private void AimDirection()
        {
            Vector2 playerViewPort = Camera.main.WorldToViewportPoint(transform.position);
            var viewportDirection = (InputReader.Aim - playerViewPort).normalized;
            var aimDirection = new Vector3(viewportDirection.x, 0f, viewportDirection.y);

            aimObject.forward =
                Vector3.Lerp(aimObject.forward, aimDirection, Time.deltaTime * rotationSpeed);
        }

        public void Move(Vector3 value)
        {
            if (value == Vector3.zero)
            {
                _targetVelocity = Vector3.MoveTowards(_rigidbody.velocity, Vector3.zero, Time.deltaTime * deceleration);
            }
            else
            {
                var speedMultiplier = InputReader.Run ? 2f : 1f;
                _targetVelocity = Vector3.MoveTowards(_rigidbody.velocity, value * (velocity * speedMultiplier),
                    Time.deltaTime * acceleration);
            }
        }


        public void SetParryActive(bool value) => ParryActive = value;

        public DamageResult TakeDamage()
        {
            if (ParryActive)
            {
                DamageDeflected?.Invoke();
                return DamageResult.Blocked;
            }
            else
            {
                DamageTaken?.Invoke();
                return DamageResult.Success;
            }
        }
    }
}