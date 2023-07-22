using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public enum PropertyIconType { None = -1, Warning = 1, Error = 2, Confirm = 3 }

public static class EditorExtensions
{
    public static string PropertyIcon(PropertyIconType type) => type switch
    {
        PropertyIconType.Warning => "orangeLight",
        PropertyIconType.Error => "redLight",
        PropertyIconType.Confirm => "greenLight",
        _ => ""
    };

    public static bool PropertyField(this SerializedProperty property, PropertyIconType iconType = PropertyIconType.None,
    params GUILayoutOption[] options)
    {
        EditorGUILayout.BeginHorizontal();
        Color color = GUI.backgroundColor;
        if (iconType == PropertyIconType.Error) GUI.backgroundColor = Color.red;
        bool value = EditorGUILayout.PropertyField(property, options);
        GUI.backgroundColor = color;
        if (iconType != PropertyIconType.None)
            EditorGUILayout.LabelField(GUIContent.none, EditorGUIUtility.IconContent(PropertyIcon(iconType)), GUILayout.Width(20));
        EditorGUILayout.EndHorizontal();
        return value;
    }

    public static bool PropertyField(this SerializedProperty property, GUIContent content, PropertyIconType iconType = PropertyIconType.None,
    params GUILayoutOption[] options)
    {
        EditorGUILayout.BeginHorizontal();
        Color color = GUI.backgroundColor;
        if (iconType == PropertyIconType.Error) GUI.backgroundColor = Color.red;
        EditorGUILayout.LabelField(content, GUILayout.MaxWidth(100), GUILayout.MinWidth(50));
        GUILayout.FlexibleSpace();
        bool value = EditorGUILayout.PropertyField(property, GUIContent.none, options);
        GUI.backgroundColor = color;
        if (iconType != PropertyIconType.None)
            EditorGUILayout.LabelField(GUIContent.none, EditorGUIUtility.IconContent(PropertyIcon(iconType)), GUILayout.Width(20));
        EditorGUILayout.EndHorizontal();
        return value;
    }

    public static bool PropertyField(this SerializedProperty property, Rect position, GUIContent label,
    PropertyIconType iconType = PropertyIconType.None)
    {
        Color color = GUI.backgroundColor;
        if (iconType == PropertyIconType.Error) GUI.backgroundColor = Color.red;
        var oldPosition = position;
        position.size -= new Vector2(EditorGUIUtility.singleLineHeight, 0);
        bool value = EditorGUI.PropertyField(position, property, label);
        position = oldPosition;
        position.x = position.size.x;
        GUI.backgroundColor = color;
        if (iconType != PropertyIconType.None)
            EditorGUI.LabelField(position, EditorGUIUtility.IconContent(PropertyIcon(iconType)));
        return value;
    }

    public static bool PropertyField(this SerializedProperty property, PropertyIconType iconType,
    bool helpButtonValue, string helpMessage, params GUILayoutOption[] options)
    {
        EditorGUILayout.BeginHorizontal();
        Color color = GUI.backgroundColor;
        if (GUILayout.Button("?", GUILayout.Width(EditorGUIUtility.singleLineHeight),
        GUILayout.Height(EditorGUIUtility.singleLineHeight))) helpButtonValue = !helpButtonValue;
        if (iconType == PropertyIconType.Error) GUI.backgroundColor = Color.red;
        EditorGUILayout.PropertyField(property, options);
        GUI.backgroundColor = color;
        if (iconType != PropertyIconType.None)
            EditorGUILayout.LabelField(GUIContent.none, EditorGUIUtility.IconContent(PropertyIcon(iconType)), GUILayout.Width(20));
        EditorGUILayout.EndHorizontal();
        if (helpButtonValue) HelpBox(helpMessage, MessageType.None);
        return helpButtonValue;
    }

    public static bool PropertyField(this SerializedProperty property, PropertyIconType iconType,
    bool helpButtonValue, string helpMessage, Texture2D helpIcon, params GUILayoutOption[] options)
    {
        EditorGUILayout.BeginHorizontal();
        Color color = GUI.backgroundColor;
        if (GUILayout.Button("?", GUILayout.Width(EditorGUIUtility.singleLineHeight),
        GUILayout.Height(EditorGUIUtility.singleLineHeight))) helpButtonValue = !helpButtonValue;
        if (iconType == PropertyIconType.Error) GUI.backgroundColor = Color.red;
        EditorGUILayout.PropertyField(property, options);
        GUI.backgroundColor = color;
        if (iconType != PropertyIconType.None)
            EditorGUILayout.LabelField(GUIContent.none, EditorGUIUtility.IconContent(PropertyIcon(iconType)), GUILayout.Width(20));
        EditorGUILayout.EndHorizontal();
        if (helpButtonValue)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("", EditorStyles.helpBox, GUILayout.Height(64), GUILayout.Width(64));
            GUI.DrawTexture(GUILayoutUtility.GetLastRect(), helpIcon);
            HelpBox(helpMessage, MessageType.None);
            EditorGUILayout.EndHorizontal();
        }
        return helpButtonValue;
    }

    public static bool BooleanEditorIcon(this bool value)
    {
        EditorGUILayout.LabelField(GUIContent.none,
        EditorGUIUtility.IconContent(PropertyIcon(value ? PropertyIconType.Confirm : PropertyIconType.Error)), GUILayout.Width(20));
        return value;
    }

    public static bool IsNull(this Object obj) { return obj == null; }

    public static bool IsEmpty(this string value) { return string.IsNullOrEmpty(value); }

    public static bool IsEqual(this int value, int number) { return value == number; }
    public static bool IsEqual(this float value, int number) { return value == number; }
    public static bool IsGreater(this int value, int number) { return value > number; }
    public static bool IsGreater(this float value, int number) { return value > number; }
    public static bool IsGreaterEqual(this int value, int number) { return value >= number; }
    public static bool IsGreaterEqual(this float value, int number) { return value >= number; }
    public static bool IsLess(this int value, int number) { return value < number; }
    public static bool IsLess(this float value, int number) { return value < number; }
    public static bool IsLessEqual(this int value, int number) { return value <= number; }
    public static bool IsLessEqual(this float value, int number) { return value <= number; }

    private static void HelpBox(string message, MessageType type = MessageType.Info)
    { EditorGUILayout.HelpBox(message, type); }
}
