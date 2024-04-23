using System;
using UnityEngine;
using SuperKatanaTiger.PlayerComponents;
using SuperKatanaTiger.Pooling;

namespace SuperKatanaTiger.Enemies
{
    [RequireComponent(typeof(Collider))]
    [RequireComponent(typeof(Rigidbody))]
    public class Bullet : PooledMonoBehaviour
    {
        [SerializeField] private float lifeTime = 10f;

        private Collider _collider;
        private Rigidbody _rigidbody;
        private float _timer;

        public void Setup(Vector3 direction, float velocity)
        {
            _timer = lifeTime;
            _collider ??= GetComponent<Collider>();
            _rigidbody ??= GetComponent<Rigidbody>();

            _rigidbody.velocity = direction * velocity;
            _collider.enabled = true;
        }

        private void Update()
        {
            _timer -= Time.deltaTime;
            if (_timer <= 0f) ReturnToPool();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Player player))
                player.TakeDamage();
            _collider.enabled = false;
            ReturnToPool();
        }
    }
}