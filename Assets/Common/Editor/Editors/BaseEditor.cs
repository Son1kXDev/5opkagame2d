using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Text.RegularExpressions;

[CustomEditor(typeof(MonoBehaviour), editorForChildClasses: true)]
public class BaseEditor : Editor
{
    protected static GUIStyle titleStyle = null;
    protected ComponentAttribute componentAttribute = null;
    protected bool isUnityNamespace = false;

    protected virtual void OnEnable()
    {
        string targetNamespace = target.GetType().Namespace;
        if (!string.IsNullOrEmpty(targetNamespace))
            isUnityNamespace = targetNamespace.StartsWith("Unity");

        if (componentAttribute == null)
            componentAttribute = GetComponentAttribute(target);

    }

    public override void OnInspectorGUI()
    {
        if (!isUnityNamespace)
            HeaderGUI(componentAttribute);

        base.OnInspectorGUI();
    }

    public static void HeaderGUI(ComponentAttribute componentAttribute)
    {
        GUILayout.Space(5f);

        if (titleStyle == null)
        {
            titleStyle = new(GUI.skin.label);
            titleStyle.fontSize = 15;
            titleStyle.fontStyle = FontStyle.Bold;
            titleStyle.alignment = TextAnchor.MiddleCenter;
        }

        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.Label(componentAttribute.Name, titleStyle);
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();

        if (!string.IsNullOrEmpty(componentAttribute.Description))
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Box(componentAttribute.Description, GUILayout.Width(Screen.width * .8f));
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
        }

        GUILayout.Space(10f);
    }

    public static ComponentAttribute GetComponentAttribute(Object obj)
    {
        return obj.GetType().GetCustomAttribute<ComponentAttribute>() ??
        new ComponentAttribute(Regex.Replace(obj.GetType().Name.ToString(), @"\B[A-Z]", m => " " + m));
    }
}

