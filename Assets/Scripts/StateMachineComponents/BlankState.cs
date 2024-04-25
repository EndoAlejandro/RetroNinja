namespace SuperKatanaTiger.StateMachineComponents
{
    public class BlankState : IState
    {
        public override string ToString() => "Idle";
        public AnimationState AnimationState { get; }
        public bool CanTransitionToSelf => false;
        public BlankState(AnimationState animationState = AnimationState.Ground) => AnimationState = animationState;

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