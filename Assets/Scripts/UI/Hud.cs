using SuperKatanaTiger.Enemies;
using SuperKatanaTiger.PlayerComponents;
using TMPro;
using UnityEngine;

namespace SuperKatanaTiger.UI
{
    public class Hud : MonoBehaviour
    {
        [SerializeField] private TMP_Text healthText;
        [SerializeField] private TMP_Text scoreText;
        private int _score;

        private void Start()
        {
            healthText.text = "HP: " + Player.Instance.Health.ToString("000");
            scoreText.text = "SCORE: " + _score.ToString("00000");
            Player.Instance.DamageTaken += PlayerOnDamageTaken;
            Enemy.Death += EnemyOnDeath;
        }

        private void EnemyOnDeath()
        {
            _score += Random.Range(70, 150);
            scoreText.text = "SCORE: " + _score.ToString("00000");
        }

        private void PlayerOnDamageTaken()
        {
            healthText.text = "HP: " + Player.Instance.Health.ToString("000");
        }


        private void OnDestroy()
        {
            StopAllCoroutines();
            if (Player.Instance != null) Player.Instance.DamageTaken += PlayerOnDamageTaken;
        }
    }
}