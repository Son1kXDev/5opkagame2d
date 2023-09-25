using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Enjine
{
    public class SceneLoadManager : MonoBehaviour
    {
        public static SceneLoadManager Instance { get; private set; }

        [SerializeField, StatusIcon] private GameObject _loaderPanel;
        [SerializeField, StatusIcon] private Animator _loaderAnimator;
        [SerializeField, StatusIcon] private UnityEngine.UI.Image _loadingIndicator;

        public void Initialize()
        {
            if (Instance) Destroy(gameObject);
            else
            {
                Instance = this;
                if (transform.parent != null)
                    transform.parent = null;

                //TODO: Load settings from bootstrap
                // Application.targetFrameRate = 120;
                //QualitySettings.antiAliasing 0 2 4 8

                SceneManager.sceneUnloaded += SceneUnloaded;
                DontDestroyOnLoad(gameObject);
            }
        }

        private void SceneUnloaded(Scene arg0) => Debug.Clear();

        public async void LoadScene(string sceneName, bool doNotUnloadCurrentScene = false)
        {
            LoadSceneMode mode = doNotUnloadCurrentScene ? LoadSceneMode.Additive : LoadSceneMode.Single;

            AsyncOperation currentScene = SceneManager.LoadSceneAsync(sceneName, mode);
            currentScene.allowSceneActivation = false;

            _loaderPanel.SetActive(true);
            _loaderAnimator.SetBool("loading", true);
            _loadingIndicator.fillAmount = 0;
            do
            {
                await Task.Delay(1000);
                _loadingIndicator.fillAmount = currentScene.progress;
            } while (currentScene.progress < 0.9f);

            currentScene.allowSceneActivation = true;
            _loaderAnimator.SetBool("loading", false);
            await Task.Delay(500);
            _loaderPanel.SetActive(false);
        }

        public async void LoadScene(int ID, bool doNotUnloadCurrentScene = false)
        {
            LoadSceneMode mode = doNotUnloadCurrentScene ? LoadSceneMode.Additive : LoadSceneMode.Single;

            AsyncOperation currentScene = SceneManager.LoadSceneAsync(ID, mode);
            currentScene.allowSceneActivation = false;

            _loaderPanel.SetActive(true);
            _loaderAnimator.SetBool("loading", true);
            _loadingIndicator.fillAmount = 0;
            do
            {
                await Task.Delay(1000);
                _loadingIndicator.fillAmount = currentScene.progress;
            } while (currentScene.progress < 0.9f);

            currentScene.allowSceneActivation = true;
            _loaderAnimator.SetBool("loading", false);
            await Task.Delay(500);
            _loaderPanel.SetActive(false);
        }

        public void ReloadCurrentScene()
        {
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            LoadScene(currentSceneIndex);
        }
    }
}