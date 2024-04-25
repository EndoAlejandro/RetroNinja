using SuperKatanaTiger.Pooling;
using UnityEngine;

namespace SuperKatanaTiger
{
    public class Prop : MonoBehaviour, ITakeDamage
    {
        [SerializeField] private PoolAfterSeconds propDestructionFx;

        public void TakeDamage(Vector3 direction)
        {
            propDestructionFx.Get<PoolAfterSeconds>(transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}