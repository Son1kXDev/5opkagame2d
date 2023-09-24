using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
namespace Enjine
{
    public static class FocusOnObjectTriggerMenuItem
    {
        [MenuItem("GameObject/2D Object/Game/Focus on Object Trigger", priority = 0)]
        private static void CreateFocusOnObjectTrigger()
        {
            GameObject newTrigger = ObjectFactory.CreateGameObject("Focus on Object Trigger");

            SceneView lastView = SceneView.lastActiveSceneView;
            newTrigger.transform.position = lastView ? lastView.pivot : Vector3.zero;

            StageUtility.PlaceGameObjectInCurrentStage(newTrigger);
            GameObjectUtility.EnsureUniqueNameForSibling(newTrigger);

            var iconContent = EditorGUIUtility.IconContent("sv_label_7");
            EditorGUIUtility.SetIconForObject(newTrigger, (Texture2D)iconContent.image);

            Undo.RegisterCreatedObjectUndo(newTrigger, "Created new Focus on object trigger");
            Collider2D triggerCollider = newTrigger.AddComponent<BoxCollider2D>();
            triggerCollider.isTrigger = true;
            newTrigger.AddComponent<FocusOnObjectTrigger>();

            Selection.activeGameObject = newTrigger;

            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        }
    }
}
