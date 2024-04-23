using SuperKatanaTiger.PlayerComponents.States;
using SuperKatanaTiger.StateMachineComponents;

namespace SuperKatanaTiger.PlayerComponents
{
    public class PlayerStateMachine : FiniteStateBehaviour
    {
        private Player _player;

        protected override void References()
        {
            _player = GetComponent<Player>();
        }

        protected override void StateMachine()
        {
            var ground = new GroundState(_player, _player.HitBox);
            
            stateMachine.SetState(ground);
        }
    }
}