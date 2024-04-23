using DarkHavoc.StateMachineComponents;
using SuperKatanaTiger.Enemies.States;

namespace SuperKatanaTiger.Enemies
{
    public class EnemyStateMachine : FiniteStateBehaviour
    {
        private Enemy _enemy;

        protected override void References()
        {
            _enemy = GetComponent<Enemy>();
        }

        protected override void StateMachine()
        {
            var ground = new GroundState(_enemy);

            stateMachine.SetState(ground);
        }
    }
}