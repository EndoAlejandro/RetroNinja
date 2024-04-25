using UnityEngine;

namespace SuperKatanaTiger
{
    public class EntityDithering : MonoBehaviour
    {
        private Collider _collider;
        private Plane[] _cameraFrustum;
        private RaycastHit[] _results;

        private void Awake() => _results = new RaycastHit[50];
        private void Start() => _collider = GetComponent<Collider>();

        private void LateUpdate()
        {
            _cameraFrustum = GeometryUtility.CalculateFrustumPlanes(Camera.main);
            if(!GeometryUtility.TestPlanesAABB(_cameraFrustum, _collider.bounds)) return;

            var direction = transform.position - Camera.main.transform.position;
            var size = Physics.RaycastNonAlloc(Camera.main.transform.position, direction.normalized, _results, direction.magnitude);

            for (int i = 0; i < size; i++)
            {
                if(!_results[i].transform.TryGetComponent(out WallDithering wallDithering)) continue;
                wallDithering.FadeOut();
            }
        }
    }
}