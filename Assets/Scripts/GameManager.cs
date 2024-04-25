using System.Collections;
using SuperKatanaTiger.CustomUtils;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SuperKatanaTiger
{
    public class GameManager : Singleton<GameManager>
    {
        protected override void SingletonAwake() => DontDestroyOnLoad(gameObject);
        public void GoToMainMenu() => StartCoroutine(GoToSceneAsync("MainMenu"));
        public void GoToCinematic() => StartCoroutine(GoToSceneAsync("Cinematic"));
        public void GoToBiome(Biome biome) => StartCoroutine(GoToSceneAsync(biome.ToString()));
        
        private IEnumerator GoToSceneAsync(string sceneName)
        {
            yield return SceneManager.LoadSceneAsync("Loading");
            yield return new WaitForSeconds(.2f);
            yield return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            yield return null;
            yield return SceneManager.UnloadSceneAsync("Loading");
        }
    }
}
