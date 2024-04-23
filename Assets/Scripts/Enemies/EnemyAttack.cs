using CustomUtils;
using SuperKatanaTiger.PlayerComponents;
using UnityEngine;

namespace SuperKatanaTiger.Enemies
{
    public class EnemyAttack : MonoBehaviour
    {
        public bool OnCoolDown => _cooldownTimer > 0f;

        [SerializeField] private Bullet bulletPrefab;
        [SerializeField] private Transform aim;
        [SerializeField] private float aimVelocity = 5f;
        [SerializeField] private float bulletsPerSecond;

        private Player _player;
        private Vector3 _direction;
        private float _cooldownTimer;

        private void Start() => _player = Player.Instance;

        private void Update()
        {
            if (_cooldownTimer > 0f) _cooldownTimer -= Time.deltaTime;
            
            _direction = (_player.transform.position - transform.position).With(y: 0).normalized;
            aim.forward = Vector3.Lerp(aim.forward, _direction, Time.deltaTime * aimVelocity);
        }

        public void Attack()
        {
            if (OnCoolDown) return;
            var bullet = bulletPrefab.Get<Bullet>(aim.position + Vector3.up * .5f, Quaternion.identity);
            bullet.Setup(aim.forward, 3f);
            _cooldownTimer = 1f / bulletsPerSecond;
        }
    }
}