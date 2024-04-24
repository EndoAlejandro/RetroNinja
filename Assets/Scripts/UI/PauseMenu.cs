using UnityEngine;
using UnityEngine.UI;

namespace SuperKatanaTiger.UI
{
    public class PauseMenu : MonoBehaviour
    {
        [SerializeField] private Button continueButton;
        [SerializeField] private Button exitButton;
        
        private void Start()
        {
            continueButton.onClick.AddListener(()=> {});
            exitButton.onClick.AddListener(GameManager.Instance.GoToMainMenu);
        }
    }
}