using System.Collections;
using System.Threading.Tasks;
using CustomUtils;
using SuperKatanaTiger.EQS;
using SuperKatanaTiger.PlayerComponents;
using SuperKatanaTiger.StateMachineComponents;
using UnityEngine;
using UnityEngine.AI;
using AnimationState = SuperKatanaTiger.StateMachineComponents.AnimationState;

namespace SuperKatanaTiger.Enemies.States
{
    public class GroundState : IState
    {
        public override string ToString() => "Ground";
        public AnimationState AnimationState => AnimationState.Ground;
        public bool CanTransitionToSelf => false;
        public bool Ended { get; private set; }

        private readonly Enemy _enemy;
        private NavMeshPath _path;

        private Vector3 _target;
        private Vector3 _distance;
        private bool _playerVisible;

        private bool _playerDetected;

        public GroundState(Enemy enemy)
        {
            _enemy = enemy;
            _path = new NavMeshPath();
        }

        public void Tick()
        {
            _distance = Player.Instance.transform.position - _enemy.transform.position;
            _playerVisible = Physics.Raycast(_enemy.transform.position + Vector3.up * .5f,
                                 _distance.normalized, out RaycastHit hit, _enemy.DetectionRadius) &&
                             hit.transform == Player.Instance.transform;
            if (!_playerVisible) return;
            if (_distance.magnitude <= _enemy.DetectionRadius) _playerDetected = true;
            if (_distance.magnitude <= _enemy.Radius) Ended = true;
        }

        public void FixedTick()
        {
            NavMesh.CalculatePath(_enemy.transform.position, _target, NavMesh.AllAreas, _path);
            var nextPoint = _path.corners.Length > 1 ? _path.corners[1] : _path.corners[0];
            var direction = nextPoint - _enemy.transform.position;
            _enemy.Move(direction.magnitude > .45f && _playerDetected ? direction.With(y: 0).normalized : Vector3.zero);

            if (direction.magnitude <= .5f) NextTarget();
        }

        private void NextTarget()
        {
            var target = EqsManager.Instance.GetClosestPointToPlayer(_enemy.transform, _enemy.Radius);
            if (target != null)
            {
                _enemy.SetNewPoint(target);
                _target = target.Position;
            }
            else
            {
                _enemy.SetNewPoint(null);
                _target = _enemy.transform.position;
            }
        }

        public void OnEnter()
        {
            Ended = false;
            _playerVisible = false;
            NextTarget();
        }

        public void OnExit() => _enemy.SetNewPoint(null);
    }
}