using System.Collections;
using System.Collections.Generic;
using SuperKatanaTiger.Enemies;
using UnityEngine;

namespace SuperKatanaTiger.Player
{
    [RequireComponent(typeof(BoxCollider))]
    public sealed class HitBox : MonoBehaviour
    {
        [SerializeField] private float cooldown = 1f;
        [SerializeField] private bool debug;

        private Collider[] _results;
        private Collider _collider;
        private bool _onCooldown;

        private void Awake()
        {
            _collider = GetComponent<Collider>();
            _results = new Collider[50];
        }

        private List<Enemy> OverlapHitBox()
        {
            Bounds bounds = _collider.bounds;
            var size = Physics.OverlapBoxNonAlloc(bounds.center, bounds.size * .5f, _results,
                Quaternion.Euler(transform.forward));
            var enemies = new List<Enemy>();

            for (int i = 0; i < size; i++)
            {
                if (!_results[i].TryGetComponent(out Enemy enemy)) continue;
                enemies.Add(enemy);
            }

            return enemies;
        }

        public DamageResult TryToAttack()
        {
            DamageResult result = DamageResult.Failed;
            if (_onCooldown) return result;

            var enemies = OverlapHitBox();
            if (enemies.Count > 0) result = DamageResult.Success;
            foreach (var enemy in enemies)
            {
                var direction = enemy.transform.position - transform.parent.position;
                enemy.TakeDamage(direction.normalized);
            }

            StartCoroutine(CooldownAsync());
            return result;
        }

        private IEnumerator CooldownAsync()
        {
            _onCooldown = true;
            yield return new WaitForSeconds(cooldown);
            _onCooldown = false;
        }

        private void OnDrawGizmos()
        {
            if (!debug) return;
            _collider = GetComponent<Collider>();
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(transform.position, _collider.bounds.size);
        }
    }
}