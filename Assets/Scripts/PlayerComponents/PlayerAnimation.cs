using MoreMountains.Feedbacks;
using SuperKatanaTiger.Pooling;
using UnityEngine;

namespace SuperKatanaTiger.PlayerComponents
{
    public class PlayerAnimation : MonoBehaviour
    {
        [Header("Custom Vfx")]
        [SerializeField] private PoolAfterSeconds attackFx;
        
        [Header("MM Feedbacks")]
        [SerializeField] private MMF_Player takeDamageFeedback;
        [SerializeField] private MMF_Player deflectDamageFeedback;

        private Player _player;

        private void Awake() => _player = GetComponentInParent<Player>();

        private void Start()
        {
            _player.DamageTaken += PlayerOnDamageTaken;
            _player.DamageDeflected += PlayerOnDamageDeflected;
        }

        public void PlayAttackFx() => attackFx.Get<PoolAfterSeconds>(_player.AimObject.position, _player.AimObject.rotation);
        private void PlayerOnDamageDeflected() => deflectDamageFeedback?.PlayFeedbacks();
        private void PlayerOnDamageTaken() => takeDamageFeedback?.PlayFeedbacks();

        private void OnDestroy()
        {
            if (_player == null) return;
            _player.DamageTaken -= PlayerOnDamageTaken;
            _player.DamageDeflected -= PlayerOnDamageDeflected;
        }
    }
}