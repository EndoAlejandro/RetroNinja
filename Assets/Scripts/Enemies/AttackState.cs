using SuperKatanaTiger.StateMachineComponents;
using UnityEngine;
using AnimationState = SuperKatanaTiger.StateMachineComponents.AnimationState;

namespace SuperKatanaTiger.Enemies
{
    public class AttackState : IState
    {
        public override string ToString() => "Attack";
        public AnimationState AnimationState => AnimationState.Attack;
        public bool CanTransitionToSelf => false;
        public bool Ended => _timer <= 0f;

        private readonly Enemy _enemy;
        private readonly EnemyAttack _attack;
        private float _timer;
        public AttackState(Enemy enemy,EnemyAttack attack)
        {
            _enemy = enemy;
            _attack = attack;
        }
        public void Tick() => _timer -= Time.deltaTime;
        public void FixedTick() => _enemy.Move(Vector3.zero);
        public void OnEnter()
        {
            _timer = 2f;
            _attack.Attack();
        }

        public void OnExit()
        {
            
        }
    }
}