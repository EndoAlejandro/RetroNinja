using MoreMountains.Feedbacks;
using UnityEngine;

namespace SuperKatanaTiger.Enemies
{
    public class EnemyAnimation : MonoBehaviour
    {
        [SerializeField] private MMF_Player takeDamageFeedback;

        private Enemy _enemy;

        private void Awake() => _enemy = GetComponentInParent<Enemy>();

        private void Start()
        {
            _enemy.DamageTaken += EnemyOnDamageTaken;
        }

        private void EnemyOnDamageTaken()
        {
            takeDamageFeedback?.PlayFeedbacks();
        }
    }
}