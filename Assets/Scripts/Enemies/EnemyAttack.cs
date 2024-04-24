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
        [SerializeField] private float bulletSpeed = 5f;
        [SerializeField] private int bulletsPerShot;
        [SerializeField] private float spreadAngle = 45f;

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
            ShootFanBullets();
            _cooldownTimer = 1f / bulletsPerSecond;
        }


        private void ShootFanBullets()
        {
            float startAngle = -spreadAngle / 2;
            float angleStep = spreadAngle / (bulletsPerShot - 1);

            for (int i = 0; i < bulletsPerShot; i++)
            {
                Quaternion rotation = Quaternion.LookRotation(aim.forward) *
                                      Quaternion.Euler(0f, startAngle + i * angleStep, 0f);

                var bullet = bulletPrefab.Get<Bullet>(aim.position + Vector3.up * .5f, rotation);
                bullet.Setup(bulletSpeed);
            }
        }
    }
}