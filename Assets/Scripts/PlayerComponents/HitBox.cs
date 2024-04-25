using System.Collections;
using System.Collections.Generic;
using CustomUtils;
using SuperKatanaTiger.Pooling;
using UnityEngine;

namespace SuperKatanaTiger.PlayerComponents
{
    [RequireComponent(typeof(BoxCollider))]
    public sealed class HitBox : MonoBehaviour
    {
        public bool OnCooldown { get; private set; }
        
        [SerializeField] private float cooldown = 1f;
        [SerializeField] private bool debug;

        [SerializeField] private PoolAfterSeconds slashFx;

        private Collider[] _results;
        private Collider _collider;

        private void Awake()
        {
            _collider = GetComponent<Collider>();
            _results = new Collider[50];
        }

        private List<ITakeDamage> OverlapHitBox()
        {
            Bounds bounds = _collider.bounds;
            var size = Physics.OverlapBoxNonAlloc(bounds.center, bounds.extents, _results,
                Quaternion.Euler(transform.forward));
            var takeDamages = new List<ITakeDamage>();

            for (int i = 0; i < size; i++)
            {
                if (!_results[i].TryGetComponent(out ITakeDamage takeDamage)) continue;
                takeDamages.Add(takeDamage);
            }

            return takeDamages;
        }

        public DamageResult TryToAttack()
        {
            DamageResult result = DamageResult.Failed;
            if (OnCooldown) return result;

            var takeDamages = OverlapHitBox();
            if (takeDamages.Count > 0) result = DamageResult.Success;
            foreach (var takeDamage in takeDamages)
            {
                var direction = takeDamage.transform.position - transform.parent.position;
                takeDamage.TakeDamage(direction.normalized);
                slashFx.Get<PoolAfterSeconds>(takeDamage.transform.position.With(y:.5f), Quaternion.identity);
            }

            StartCoroutine(CooldownAsync());
            return result;
        }

        private IEnumerator CooldownAsync()
        {
            OnCooldown = true;
            yield return new WaitForSeconds(cooldown);
            OnCooldown = false;
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