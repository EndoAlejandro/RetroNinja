using SuperKatanaTiger.Enemies.States;
using SuperKatanaTiger.StateMachineComponents;

namespace SuperKatanaTiger.Enemies
{
    public class EnemyStateMachine : FiniteStateBehaviour
    {
        private Enemy _enemy;
        private EnemyAttack _attack;

        protected override void References()
        {
            _enemy = GetComponent<Enemy>();
            _attack = GetComponent<EnemyAttack>();
        }

        protected override void StateMachine()
        {
            var ground = new GroundState(_enemy);
            var attack = new AttackState(_enemy, _attack);
            var takeDamage = new TakeDamageState(_enemy);

            stateMachine.SetState(ground);

            stateMachine.AddTransition(takeDamage, ground, () => takeDamage.Ended);
            stateMachine.AddTransition(ground, attack, () => ground.Ended && !_attack.OnCoolDown);
            stateMachine.AddTransition(attack, ground, () => attack.Ended);

            stateMachine.AddAnyTransition(takeDamage, () => _enemy.Stunned);
        }
    }
}