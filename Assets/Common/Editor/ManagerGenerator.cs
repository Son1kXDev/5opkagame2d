using System.ComponentModel.Design;
using UnityEngine;
using UnityEditor;
using System;
using UnityEditor.SceneManagement;

namespace Enjine.Plugins
{
    public static class ManagerGenerator
    {

        [MenuItem("GameObject/Managers/UIManager", priority = 0)]
        private static void CreateUIManager()
        {
            if (IsAnyInstanceOfObject(typeof(UIManager)))
            {
                Debug.LogError("UIManager already exists on this scene");
                return;
            }

            GameObject uiManager = new();
            Undo.RegisterCreatedObjectUndo(uiManager, "Create UIManager");
            uiManager.name = "UIManager";
            uiManager.AddComponent(typeof(UIManager));
            CheckLayer("Organize");
            uiManager.layer = LayerMask.NameToLayer("Organize");
            uiManager.isStatic = true;
            Selection.activeGameObject = uiManager;
        }

        private static bool IsAnyInstanceOfObject(Type type)
        {
            var objectsOfTypeInScene = Resources.FindObjectsOfTypeAll(type);
            return objectsOfTypeInScene != null || objectsOfTypeInScene.Length != 0;
        }

        private static void CheckLayer(string name)
        {
            SerializedObject tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);

            SerializedProperty layers = tagManager.FindProperty("layers");
            if (layers == null || !layers.isArray)
            {
                Debug.LogWarning("Can't set up the layers. It's possible the format of the layers and tags data has changed in this version of Unity.");
                Debug.LogError("Layers is null: " + (layers == null));
                return;
            }

            bool layerExist = false;

            for (int i = 6; i < layers.arraySize; i++)
            {
                SerializedProperty layer = layers.GetArrayElementAtIndex(i);

                if (layer.stringValue == name)
                {
                    layerExist = true;
                    break;
                }
            }

            if (layerExist) return;

            for (int i = 6; i < layers.arraySize; i++)
            {
                SerializedProperty layer = layers.GetArrayElementAtIndex(i);

                if (layer.stringValue == "")
                {
                    layer.stringValue = name;
                    tagManager.ApplyModifiedProperties();
                    break;
                }
            }
        }
    }
}