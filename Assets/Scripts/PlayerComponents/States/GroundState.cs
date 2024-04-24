using SuperKatanaTiger.Input;
using SuperKatanaTiger.StateMachineComponents;

namespace SuperKatanaTiger.PlayerComponents.States
{
    public class GroundState : IState
    {
        public override string ToString() => "Ground";
        public AnimationState AnimationState => AnimationState.Ground;
        public bool CanTransitionToSelf => false;

        private readonly Player _player;

        public GroundState(Player player) => _player = player;

        public void Tick()
        {
        }

        public void FixedTick()
        {
            _player.Move(InputReader.Movement);
        }

        public void OnEnter()
        {
        }

        public void OnExit()
        {
        }
    }
}