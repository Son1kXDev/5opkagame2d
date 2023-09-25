using UnityEngine;

namespace Enjine.Data
{
    public class Bootstrap : MonoBehaviour
    {
        [SerializeField, StatusIcon] private SceneLoadManager _sceneLoadManagerPrefab;

        private void OnEnable()
        {
            // Creating an instance of SceneLoadManager
            var sceneLoadManager = Instantiate(_sceneLoadManagerPrefab);
            sceneLoadManager.name = "[Bootstrap] SceneLoadManager";
            sceneLoadManager.transform.position = Vector3.zero;
            sceneLoadManager.Initialize();

            //TODO: Load settings

            //TODO: Load saved data

            //Loading first scene:
            sceneLoadManager.LoadScene(1);
        }
    }
}