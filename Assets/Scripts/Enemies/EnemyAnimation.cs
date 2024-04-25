using System;
using MoreMountains.Feedbacks;
using SuperKatanaTiger.StateMachineComponents;
using UnityEngine;
using AnimationState = SuperKatanaTiger.StateMachineComponents.AnimationState;

namespace SuperKatanaTiger.Enemies
{
    public class EnemyAnimation : MonoBehaviour
    {
        [SerializeField] private MMF_Player takeDamageFeedback;
        [SerializeField] private MMF_Player attackFeedback;

        private AnimationState _previousState;
        private Animator _animator;
        private SpriteRenderer _spriteRenderer;
        private Enemy _enemy;
        private EnemyStateMachine _stateMachine;
        private static readonly int Velocity = Animator.StringToHash("Velocity");

        private void Awake()
        {
            _previousState = AnimationState.Ground;
            _animator = GetComponent<Animator>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _enemy = GetComponentInParent<Enemy>();
            _stateMachine = GetComponentInParent<EnemyStateMachine>();
        }

        private void Start()
        {
            _enemy.DamageTaken += EnemyOnDamageTaken;
            _stateMachine.OnEntityStateChanged += StateMachineOOnEntityStateChanged;
        }

        private void Update()
        {
            var targetVelocity = _enemy.TargetVelocity;
            if (targetVelocity.x != 0f) _spriteRenderer.flipX = targetVelocity.x < 0f;
            _animator.SetFloat(Velocity, Mathf.Min(targetVelocity.magnitude, 1f));
        }

        private void StateMachineOOnEntityStateChanged(IState state)
        {
            if(state.AnimationState == AnimationState.Attack) attackFeedback?.PlayFeedbacks();
            _animator.ResetTrigger(_previousState.ToString());
            _animator.SetTrigger(state.AnimationState.ToString());
            _previousState = state.AnimationState;
        }

        private void EnemyOnDamageTaken() => takeDamageFeedback?.PlayFeedbacks();

        private void OnDestroy()
        {
            _enemy.DamageTaken -= EnemyOnDamageTaken;
            _stateMachine.OnEntityStateChanged -= StateMachineOOnEntityStateChanged;
        }
    }
}