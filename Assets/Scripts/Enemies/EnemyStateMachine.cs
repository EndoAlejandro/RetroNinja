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
            var knockBack = new KnockBackState(_enemy);
            var stun = new StunState(_enemy);

            stateMachine.SetState(ground);

            stateMachine.AddTransition(ground, attack, () => ground.Ended && !_attack.OnCoolDown);
            stateMachine.AddTransition(attack, ground, () => attack.Ended);

            stateMachine.AddTransition(knockBack, stun, () => knockBack.Ended);
            stateMachine.AddTransition(stun, ground, () => stun.Ended);
            stateMachine.AddAnyTransition(knockBack, () => _enemy.Stunned);
        }
    }
}