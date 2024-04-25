using SuperKatanaTiger.Input;
using UnityEngine;
using UnityEngine.UI;

namespace SuperKatanaTiger.UI
{
    public class PauseMenu : MonoBehaviour
    {
        [SerializeField] private GameObject panel;
        [SerializeField] private Button exitButton;
        [SerializeField] private Button continueButton;

        private bool _isPaused;

        private void Start()
        {
            panel.SetActive(false);

            continueButton.onClick.AddListener(() =>
            {
                _isPaused = false;
                panel.SetActive(false);
                Time.timeScale = 1f;
            });
            exitButton.onClick.AddListener(()=>
            {
                _isPaused = false;
                panel.SetActive(false);
                Time.timeScale = 1f;
                GameManager.Instance.GoToMainMenu();
            });
        }

        private void Update()
        {
            if (!InputReader.Pause || _isPaused) return;
            _isPaused = true;
            panel.SetActive(true);
            Time.timeScale = 0f;
        }
    }
}