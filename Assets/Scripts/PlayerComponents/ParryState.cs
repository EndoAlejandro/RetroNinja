using SuperKatanaTiger.StateMachineComponents;
using UnityEngine;
using AnimationState = SuperKatanaTiger.StateMachineComponents.AnimationState;

namespace SuperKatanaTiger.PlayerComponents
{
    public class ParryState : IState
    {
        public override string ToString() => "Parry";
        public AnimationState AnimationState => AnimationState.Defend;
        public bool CanTransitionToSelf => false;
        public bool Ended => _timer <= 0f;
        private readonly Player _player;
        private float _timer;

        public ParryState(Player player) => _player = player;
        public void Tick() => _timer -= Time.deltaTime;
        public void FixedTick() => _player.Move(Vector3.zero);
        public void OnEnter()
        {
            _timer = _player.ParryTime;
            _player.SetParryActive(true);
        }
        public void OnExit() => _player.SetParryActive(false);
    }
}