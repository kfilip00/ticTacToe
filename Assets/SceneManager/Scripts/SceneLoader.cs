using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnitySceneManager = UnityEngine.SceneManagement.SceneManager;

namespace SceneManagement
{
    public class SceneLoader : MonoBehaviour
    {
        public static Action OnLoadedScene;
        [SerializeField] private LoadingCanvas loadingPrefab;
        [SerializeField] private float minimumLoadingDuration = 2f;
        [Tooltip("Defines the maximum percentage of the progress bar that the scene loading will occupy when the minimum loading duration is specified. The remaining percentage up to 100% will be completed during the minimum wait time.")]
        [SerializeField] private float capLoadingScenePercentage = 90f;

        private LoadingCanvas loadingDisplay;
        private float totalProgress;

        private float TotalProgress
        {
            get => totalProgress;
            set
            {
                totalProgress = Mathf.Clamp01(value);
                loadingDisplay?.UpdateProgress(totalProgress);
            }
        }

        protected void LoadScene(string _key, bool _useAsyncLoading)
        {
            if (_useAsyncLoading)
            {
                LoadAsync(_key);
            }
            else
            {
                UnitySceneManager.LoadScene(_key);
            }
        }

        private void LoadAsync(string _key)
        {
            TotalProgress = 0;
            Scene _startingScene = UnitySceneManager.GetActiveScene();
            float _startingTime = Time.time;
            if (loadingPrefab)
            {
                loadingDisplay = Instantiate(loadingPrefab);
            }

            AsyncOperation _loadingOperation = UnitySceneManager.LoadSceneAsync(_key, LoadSceneMode.Additive);
            if (_loadingOperation == null)
            {
                LoadScene(_key, false);
                return;
            }
            _loadingOperation.allowSceneActivation = false;

            StartCoroutine(LoadScene(_loadingOperation, _startingScene, _startingTime));
        }

        private IEnumerator LoadScene(AsyncOperation _operation, Scene _previousScene, float _startingTime)
        {
            float _capPercentage = capLoadingScenePercentage / 100f;
            bool _finishedLoading = false;

            while (!_operation.isDone)
            {
                if (!_finishedLoading && _operation.progress >= 0.9f)
                {
                    _operation.allowSceneActivation = true;
                    _finishedLoading = true;
                    yield return StartCoroutine(UnloadScene(_previousScene, _operation));
                }

                float _currentProgress = _operation.progress / 0.9f;
                TotalProgress = Mathf.Lerp(0, _capPercentage, _currentProgress);

                yield return null;
            }

            TotalProgress = _capPercentage;

            float _elapsedTime = Time.time - _startingTime;
            if (_elapsedTime < minimumLoadingDuration)
            {
                yield return StartCoroutine(DoWaitForMinimumTime(_elapsedTime));
            }
            else
            {
                TotalProgress = 1f;
            }

            StartCoroutine(FinishLoading());
        }

        private IEnumerator UnloadScene(Scene _previousScene, AsyncOperation _loadingOperation)
        {
            while (!_loadingOperation.isDone)
            {
                yield return null;
            }

            AsyncOperation _unloadingOperation = UnitySceneManager.UnloadSceneAsync(_previousScene);
            if (_unloadingOperation == null)
            {
                yield break;
            }

            float _capPercentage = capLoadingScenePercentage / 100f;
            float _startProgress = TotalProgress;

            while (!_unloadingOperation.isDone)
            {
                float _unloadProgress = _unloadingOperation.progress;
                TotalProgress = Mathf.Lerp(_startProgress, _capPercentage, _unloadProgress);
                yield return null;
            }
        }

        private IEnumerator DoWaitForMinimumTime(float _elapsedTime)
        {
            float _capPercentage = capLoadingScenePercentage / 100f;
            float _remainingTime = minimumLoadingDuration - _elapsedTime;
            float _startTime = Time.time;

            while (Time.time < _startTime + _remainingTime)
            {
                float _progress = (Time.time - _startTime) / _remainingTime;
                TotalProgress = Mathf.Lerp(_capPercentage, 1f, _progress);
                yield return null;
            }

            TotalProgress = 1f;
        }

        private IEnumerator FinishLoading()
        {
            if (loadingDisplay)
            {
                Destroy(loadingDisplay.gameObject);
            }
            OnLoadedScene?.Invoke();
            yield return null;
        }
    }
}