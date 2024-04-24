using UnityEngine;
using SuperKatanaTiger.PlayerComponents;
using SuperKatanaTiger.Pooling;

namespace SuperKatanaTiger.Enemies
{
    [RequireComponent(typeof(Collider))]
    [RequireComponent(typeof(Rigidbody))]
    public class Bullet : PooledMonoBehaviour, ITakeDamage
    {
        [SerializeField] private float lifeTime = 10f;

        private Collider _collider;
        private Rigidbody _rigidbody;
        private float _timer;
        private bool _deflected;

        public void Setup(float velocity)
        {
            _timer = lifeTime;
            _deflected = false;
            _collider ??= GetComponent<Collider>();
            _rigidbody ??= GetComponent<Rigidbody>();

            _rigidbody.velocity = transform.forward * velocity;
            _collider.enabled = true;
        }

        private void Update()
        {
            _timer -= Time.deltaTime;
            if (_timer <= 0f) ReturnToPool();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Enemy enemy) && !_deflected) return;

            if (other.TryGetComponent(out Player player) && !_deflected)
            {
                var result = player.TakeDamage();
                if (result == DamageResult.Blocked)
                {
                    _rigidbody.velocity = -_rigidbody.velocity;
                    _deflected = true;
                    return;
                }
            } else if (_deflected && enemy != null)
            {
                enemy.TakeDamage(_rigidbody.velocity);
            }

            _collider.enabled = false;
            ReturnToPool();
        }

        public void TakeDamage(Vector3 direction) => ReturnToPool();

        protected override void ReturnToPool(float delay = 0f)
        {
            _collider.enabled = false;
            base.ReturnToPool(delay);
        }
    }
}