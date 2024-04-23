using DarkHavoc.StateMachineComponents;

namespace SuperKatanaTiger.Enemies.States
{
    public class GroundState : IState
    {
        public override string ToString() => "Ground";
        public AnimationState AnimationState => AnimationState.Ground;
        public bool CanTransitionToSelf => false;

        private readonly Enemy _enemy;

        public GroundState(Enemy enemy)
        {
            _enemy = enemy;
        }

        public void Tick()
        {
        }

        public void FixedTick()
        {
        }

        public void OnEnter()
        {
        }

        public void OnExit()
        {
        }
    }
}