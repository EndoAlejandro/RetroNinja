using UnityEngine;
using UnityEngine.UI;

namespace SuperKatanaTiger.UI
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private Button startGameButton;
        [SerializeField] private Button creditsButton;
        [SerializeField] private Button exitGameButton;

        private void Start()
        {
            startGameButton.onClick.AddListener(GameManager.Instance.GoToPlayground);
            creditsButton.onClick.AddListener(GameManager.Instance.GoToPlayground);
            exitGameButton.onClick.AddListener(Application.Quit);
        }
    }
}