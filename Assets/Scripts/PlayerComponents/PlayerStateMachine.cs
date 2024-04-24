using SuperKatanaTiger.Input;
using SuperKatanaTiger.PlayerComponents.States;
using SuperKatanaTiger.StateMachineComponents;

namespace SuperKatanaTiger.PlayerComponents
{
    public class PlayerStateMachine : FiniteStateBehaviour
    {
        private Player _player;
        private PlayerAnimation _animation;

        protected override void References()
        {
            _player = GetComponent<Player>();
            _animation = GetComponentInChildren<PlayerAnimation>();
        }

        protected override void StateMachine()
        {
            var ground = new GroundState(_player);
            var attack = new AttackState(_player, _animation);
            var parry = new ParryState(_player);

            stateMachine.SetState(ground);

            stateMachine.AddTransition(ground, attack, () => InputReader.Attack && !_player.HitBox.OnCooldown);
            stateMachine.AddTransition(attack, ground, () => attack.Ended);

            stateMachine.AddTransition(ground, parry, () => InputReader.Parry);
            stateMachine.AddTransition(parry, ground, () => parry.Ended);
        }
    }
}