using System;
using MoreMountains.Feedbacks;
using SuperKatanaTiger.Input;
using SuperKatanaTiger.Pooling;
using SuperKatanaTiger.StateMachineComponents;
using UnityEngine;
using AnimationState = SuperKatanaTiger.StateMachineComponents.AnimationState;

namespace SuperKatanaTiger.PlayerComponents
{
    public class PlayerAnimation : MonoBehaviour
    {
        [Header("Custom Vfx")]
        [SerializeField] private PoolAfterSeconds attackFx;

        [Header("MM Feedbacks")]
        [SerializeField] private MMF_Player takeDamageFeedback;

        [SerializeField] private MMF_Player deflectDamageFeedback;
        [SerializeField] private MMF_Player attackFeedback;

        private static readonly int Velocity = Animator.StringToHash("Velocity");

        private AnimationState _previousState;
        private Player _player;
        private PlayerStateMachine _stateMachine;
        private Animator _animator;
        private SpriteRenderer _spriteRenderer;

        private void Awake()
        {
            _previousState = AnimationState.Ground;
            
            _animator = GetComponent<Animator>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _player = GetComponentInParent<Player>();
            _stateMachine = GetComponentInParent<PlayerStateMachine>();
        }

        private void Start()
        {
            _player.DamageTaken += PlayerOnDamageTaken;
            _player.DamageDeflected += PlayerOnDamageDeflected;
            _stateMachine.OnEntityStateChanged += StateMachineOnEntityStateChanged;
        }

        private void Update()
        {
            var movement = InputReader.Movement;
            if (movement.x != 0f) _spriteRenderer.flipX = movement.x < 0f;
            _animator.SetFloat(Velocity, Mathf.Min(InputReader.Movement.magnitude, 1f));
        }

        private void StateMachineOnEntityStateChanged(IState state)
        {
            if(state.AnimationState == AnimationState.Attack) attackFeedback?.PlayFeedbacks();
            _animator.ResetTrigger(_previousState.ToString());
            _animator.SetTrigger(state.AnimationState.ToString());
            _previousState = state.AnimationState;
        }

        public void PlayAttackFx() =>
            attackFx.Get<PoolAfterSeconds>(_player.AimObject.position, _player.AimObject.rotation);

        private void PlayerOnDamageDeflected() => deflectDamageFeedback?.PlayFeedbacks();
        private void PlayerOnDamageTaken() => takeDamageFeedback?.PlayFeedbacks();

        private void OnDestroy()
        {
            if (_player == null) return;
            _player.DamageTaken -= PlayerOnDamageTaken;
            _player.DamageDeflected -= PlayerOnDamageDeflected;
        }
    }
}