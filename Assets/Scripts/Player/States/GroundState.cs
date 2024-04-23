using DarkHavoc.StateMachineComponents;
using SuperKatanaTiger.Input;

namespace SuperKatanaTiger.Player.States
{
    public class GroundState : IState
    {
        public override string ToString() => "Ground";
        public AnimationState AnimationState => AnimationState.Ground;
        public bool CanTransitionToSelf => false;

        private readonly Player _player;
        private readonly HitBox _hitBox;

        public GroundState(Player player, HitBox hitBox)
        {
            _player = player;
            _hitBox = hitBox;
        }

        public void Tick()
        {
            if (InputReader.Attack)
            {
                var result =_hitBox.TryToAttack();
            }
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