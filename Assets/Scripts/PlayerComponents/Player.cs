using CustomUtils;
using SuperKatanaTiger.CustomUtils;
using SuperKatanaTiger.Input;
using UnityEngine;

namespace SuperKatanaTiger.PlayerComponents
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(CapsuleCollider))]
    public class Player : Singleton<Player>
    {
        public HitBox HitBox => hitBox;

        [SerializeField] private float acceleration = 50f;
        [SerializeField] private float deceleration = 100f;
        [SerializeField] private float velocity = 4f;
        [SerializeField] private float rotationSpeed = 10f;

        [SerializeField] private Transform aimObject;
        [SerializeField] private HitBox hitBox;

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

        public void Move(Vector3 value) => _targetVelocity = value == Vector3.zero
            ? Vector3.MoveTowards(_rigidbody.velocity, Vector3.zero,
                Time.deltaTime * deceleration)
            : Vector3.MoveTowards(_rigidbody.velocity, value * velocity,
                Time.deltaTime * acceleration);


        public void TakeDamage()
        {
            Debug.Log("Take Damage.");
        }
    }
}