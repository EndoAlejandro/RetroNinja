using UnityEngine;
using UnityEngine.UI;

namespace SuperKatanaTiger.UI
{
    public class GameOver : MonoBehaviour
    {
        [SerializeField] private Button gameOverButton;
        private void Awake() => gameOverButton.onClick.AddListener(GameManager.Instance.GoToMainMenu);
    }
}
