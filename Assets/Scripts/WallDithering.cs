using UnityEngine;

namespace SuperKatanaTiger
{
    public class WallDithering : MonoBehaviour
    {
        [SerializeField] private float animationSpeed = 2f;
        [Range(0f,1f)] [SerializeField] private float minAlpha = 0.2f;
        private Renderer _renderer;
        private MaterialPropertyBlock _propertyBlock;
        private bool _fadeOut;
        private float _targetAlpha;
        private float _currentAlpha;
        private static readonly int Opacity = Shader.PropertyToID("_Opacity");

        private void Awake()
        {
            _renderer = GetComponent<Renderer>();
            _propertyBlock = new MaterialPropertyBlock();
            _renderer.GetPropertyBlock(_propertyBlock);
            _propertyBlock.SetFloat(Opacity, 1f);
            _renderer.SetPropertyBlock(_propertyBlock);
        }

        private void Update()
        {
            _renderer.GetPropertyBlock(_propertyBlock);
            _currentAlpha = _propertyBlock.GetFloat(Opacity);
        }

        private void LateUpdate()
        {
            _renderer.GetPropertyBlock(_propertyBlock);
            _targetAlpha = _fadeOut ? minAlpha : 1f;
            if (Mathf.Abs(_currentAlpha - _targetAlpha) < 0.1f)
            {
                _propertyBlock.SetFloat(Opacity, _targetAlpha);
            }
            else
            {
                float opacity = Mathf.Lerp(_propertyBlock.GetFloat(Opacity), _targetAlpha,
                    Time.deltaTime * animationSpeed);
                _propertyBlock.SetFloat(Opacity, opacity);
            }

            _fadeOut = false;
            _renderer.SetPropertyBlock(_propertyBlock);
        }

        public void FadeOut() => _fadeOut = true;
    }
}