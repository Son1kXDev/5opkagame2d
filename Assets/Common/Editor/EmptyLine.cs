using UnityEngine;
using UnityEditor;

using System.Collections;

namespace Plugins
{
    public static class EmptyLine
    {
        [MenuItem("GameObject/Property Line", priority = 0)]
        private static void EmptyLineVoid()
        {
            string name = Editors.TextEntryDialog.Show("PropertyLine", "Введите название поля");
            if (name.IsEmpty()) return;
            GameObject newLine = new()
            {
                name = $"======{name}======"
            };
            CheckLayer("Organize");
            newLine.layer = LayerMask.NameToLayer("Organize");
            newLine.isStatic = true;
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