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
            var takeDamage = new TakeDamageState(_enemy);

            stateMachine.SetState(ground);
            stateMachine.AddTransition(takeDamage, ground, () => takeDamage.Ended);

            stateMachine.AddAnyTransition(takeDamage, () => _enemy.Stunned);
        }
    }
}