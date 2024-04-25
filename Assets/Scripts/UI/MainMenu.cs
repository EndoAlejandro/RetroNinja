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
            startGameButton.onClick.AddListener(() => GameManager.Instance.GoToBiome(Biome.City));
            creditsButton.onClick.AddListener(() => GameManager.Instance.GoToBiome(Biome.City));
            exitGameButton.onClick.AddListener(Application.Quit);
        }
    }
}