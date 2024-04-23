using DarkHavoc.StateMachineComponents;
using SuperKatanaTiger.Player.States;

namespace SuperKatanaTiger.Player
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