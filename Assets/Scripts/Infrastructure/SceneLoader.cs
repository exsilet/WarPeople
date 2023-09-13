using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Infrastructure
{
    public class SceneLoader
    {
        private readonly ICoroutineRunner _coroutineRunner;

        public SceneLoader(ICoroutineRunner coroutineRunner) => 
            _coroutineRunner = coroutineRunner;

        public void Load(string name, Action onLoader = null) =>
            _coroutineRunner.StartCoroutine(LoadScene(name, onLoader));
        
        private IEnumerator LoadScene(string nextScene, Action onLoader = null)
        {
            yield return
                new WaitForEndOfFrame();
            
            if (SceneManager.GetActiveScene().name == nextScene)
            {
                onLoader?.Invoke();
                yield break;
            }
            
            AsyncOperation waitNextScene = SceneManager.LoadSceneAsync(nextScene);

            while (!waitNextScene.isDone)
                yield return null;
            
            onLoader?.Invoke();
        }
    }
}