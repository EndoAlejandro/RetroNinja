using SuperKatanaTiger.StateMachineComponents;
using UnityEngine;
using AnimationState = SuperKatanaTiger.StateMachineComponents.AnimationState;

namespace SuperKatanaTiger.Enemies.States
{
    public class KnockBackState : IState
    {
        public override string ToString() => "KnockBack";
        public AnimationState AnimationState => AnimationState.TakeDamage;
        public bool CanTransitionToSelf => true;
        public bool Ended => _timer <= 0f;
        private readonly Enemy _enemy;
        private float _timer;
        public KnockBackState(Enemy enemy) => _enemy = enemy;
        public void Tick() => _timer -= Time.deltaTime;
        public void FixedTick() => _enemy.Move(_enemy.Direction * 5f);
        public void OnEnter() => _timer = _enemy.TakeDamageTime;
        public void OnExit() => _enemy.EndStun();
    }
}