using SuperKatanaTiger.Input;
using UnityEngine;
using UnityEngine.Video;

namespace SuperKatanaTiger.UI
{
    public class CinematicController : MonoBehaviour
    {
        private VideoPlayer _videoPlayer;
        private void Awake() => _videoPlayer = GetComponent<VideoPlayer>();
        private void Start() => _videoPlayer.loopPointReached += VideoPlayerOnLoopPointReached;
        private void Update()
        {
            if (InputReader.Attack || InputReader.Parry || InputReader.Pause) GameManager.Instance.GoToBiome(Biome.City);
        }
        private void VideoPlayerOnLoopPointReached(VideoPlayer source) => GameManager.Instance.GoToBiome(Biome.City);
        private void OnDestroy() => _videoPlayer.loopPointReached -= VideoPlayerOnLoopPointReached;
    }
}