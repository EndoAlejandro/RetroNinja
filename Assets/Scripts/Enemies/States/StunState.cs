using SuperKatanaTiger.StateMachineComponents;
using UnityEngine;
using AnimationState = SuperKatanaTiger.StateMachineComponents.AnimationState;

namespace SuperKatanaTiger.Enemies.States
{
    public class StunState : IState
    {
        public override string ToString() => "Stun";
        public AnimationState AnimationState => AnimationState.Ground;
        public bool CanTransitionToSelf => false;
        public bool Ended => _timer <= 0f;
        private readonly Enemy _enemy;
        private float _timer;
        public StunState(Enemy enemy) => _enemy = enemy;
        public void Tick() => _timer -= Time.deltaTime;
        public void FixedTick() => _enemy.Move(Vector3.zero);
        public void OnEnter()
        {
            _enemy.EndStun();
            _timer = 2f;
        }

        public void OnExit() => _enemy.EndStun();
    }
}