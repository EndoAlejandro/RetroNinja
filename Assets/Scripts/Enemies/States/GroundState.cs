using SuperKatanaTiger.StateMachineComponents;
using UnityEngine;
using AnimationState = SuperKatanaTiger.StateMachineComponents.AnimationState;

namespace SuperKatanaTiger.Enemies.States
{
    public class GroundState : IState
    {
        private readonly Enemy _enemy;
        public override string ToString() => "Ground";
        public AnimationState AnimationState => AnimationState.Ground;
        public bool CanTransitionToSelf => false;
        public bool Ended { get; private set; }

        public GroundState(Enemy enemy)
        {
            _enemy = enemy;
        }

        public void Tick()
        {
        }

        public void FixedTick() => _enemy.Move(Vector3.zero);

        public void OnEnter()
        {
            Ended = false;
        }

        public void OnExit()
        {
        }
    }
}