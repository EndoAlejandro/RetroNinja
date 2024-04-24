using SuperKatanaTiger.StateMachineComponents;
using UnityEngine;
using AnimationState = SuperKatanaTiger.StateMachineComponents.AnimationState;

namespace SuperKatanaTiger.PlayerComponents.States
{
    public class AttackState : IState
    {
        public override string ToString() => "Attack";
        public AnimationState AnimationState => AnimationState.Attack;
        public bool CanTransitionToSelf => false;
        public bool Ended => _timer <= Time.deltaTime;
        private readonly Player _player;
        private readonly PlayerAnimation _animation;
        private float _timer;

        public AttackState(Player player, PlayerAnimation animation)
        {
            _player = player;
            _animation = animation;
        }

        public void Tick() => _timer -= Time.deltaTime;
        public void FixedTick() => _player.Move(Vector3.zero);

        public void OnEnter()
        {
            _timer = _player.AttackDuration;
            _animation.PlayAttackFx();
            _player.HitBox.TryToAttack();
        }

        public void OnExit()
        {
        }
    }
}