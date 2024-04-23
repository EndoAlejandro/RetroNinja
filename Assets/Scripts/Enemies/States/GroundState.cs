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

        public GroundState(Enemy enemy)
        {
            _enemy = enemy;
            _path = new NavMeshPath();
        }

        public void Tick()
        {
            var direction = Player.Instance.transform.position - _enemy.transform.position;
            var result = Physics.Raycast(_enemy.transform.position + Vector3.up * .5f,
                             direction.normalized, out RaycastHit hit, _enemy.Radius) &&
                         hit.transform == Player.Instance.transform;
            if (/*direction.magnitude <= _enemy.Radius * 1.5f && */result) 
                Ended = true;
        }

        public void FixedTick()
        {
            NavMesh.CalculatePath(_enemy.transform.position, _target, NavMesh.AllAreas, _path);
            var nextPoint = _path.corners.Length > 1 ? _path.corners[1] : _path.corners[0];
            var direction = nextPoint - _enemy.transform.position;
            _enemy.Move(direction.magnitude > 0.33f ? direction.With(y: 0).normalized : Vector3.zero);

            if (direction.magnitude <= 0.33f) NextTarget();
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
            NextTarget();
        }

        public void OnExit() => _enemy.SetNewPoint(null);
    }
}