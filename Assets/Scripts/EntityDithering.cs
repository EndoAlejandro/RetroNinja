using System;
using UnityEngine;

namespace SuperKatanaTiger
{
    public class EntityDithering : MonoBehaviour
    {
        private Camera _camera;
        private Collider _collider;
        private Plane[] _cameraFrustum;
        private RaycastHit[] _results;

        private void Awake()
        {
            _results = new RaycastHit[50];
        }

        private void Start()
        {
            _camera = Camera.main;
            _collider = GetComponent<Collider>();
        }

        private void Update()
        {
            _cameraFrustum = GeometryUtility.CalculateFrustumPlanes(_camera);
            if(!GeometryUtility.TestPlanesAABB(_cameraFrustum, _collider.bounds)) return;

            var direction = transform.position - _camera.transform.position;
            var size = Physics.RaycastNonAlloc(_camera.transform.position, direction.normalized, _results, direction.magnitude);

            for (int i = 0; i < size; i++)
            {
                if(!_results[i].transform.TryGetComponent(out WallDithering wallDithering)) continue;
                wallDithering.FadeOut();
            }
        }
    }
}