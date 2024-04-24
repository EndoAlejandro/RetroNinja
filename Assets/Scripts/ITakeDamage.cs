using UnityEngine;

namespace SuperKatanaTiger
{
    public interface ITakeDamage
    {
        Transform transform { get; }
        void TakeDamage(Vector3 direction);
    }
}