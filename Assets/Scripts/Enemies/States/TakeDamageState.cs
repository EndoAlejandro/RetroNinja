using SuperKatanaTiger.StateMachineComponents;
using UnityEngine;
using AnimationState = SuperKatanaTiger.StateMachineComponents.AnimationState;

namespace SuperKatanaTiger.Enemies.States
{
    public class TakeDamageState : IState
    {
        public override string ToString() => "Take Damage";
        public AnimationState AnimationState => AnimationState.TakeDamage;
        public bool CanTransitionToSelf => false;
        public bool Ended => _timer <= 0f;
        private readonly Enemy _enemy;
        private float _timer;
        public TakeDamageState(Enemy enemy) => _enemy = enemy;
        public void Tick() => _timer -= Time.deltaTime;
        public void FixedTick() => _enemy.Move(_enemy.Direction * 5f);
        public void OnEnter()
        {
            _timer = _enemy.TakeDamageTime;
            _enemy.EndStun();
        }

        public void OnExit() => _enemy.EndStun();
    }
}