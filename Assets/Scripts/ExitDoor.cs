using SuperKatanaTiger.PlayerComponents;
using UnityEngine;

namespace SuperKatanaTiger
{
    public enum Biome
    {
        City,
        Jungle,
        Mountain,
        GameOver
    }
    public class ExitDoor : MonoBehaviour
    {
        [SerializeField] private Biome biome;

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Player player))
                GameManager.Instance.GoToBiome(biome);
        }
    }
}
