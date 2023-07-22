using System;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(ShowOnlyAttribute))]
public class ShowOnlyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty prop, GUIContent label)
    {
        string valueStr;

        switch (prop.propertyType)
        {
            case UnityEditor.SerializedPropertyType.Integer:
                valueStr = prop.intValue.ToString();
                break;
            case UnityEditor.SerializedPropertyType.Boolean:
                valueStr = prop.boolValue.ToString();
                break;
            case UnityEditor.SerializedPropertyType.Float:
                valueStr = prop.floatValue.ToString("0.00000");
                break;
            case UnityEditor.SerializedPropertyType.String:
                valueStr = prop.stringValue;
                break;
            case UnityEditor.SerializedPropertyType.Enum: 
                valueStr = prop.enumDisplayNames[prop.enumValueIndex]; 
                break;
            case UnityEditor.SerializedPropertyType.Vector2:
                valueStr = prop.vector2Value.ToString();
                break;
            case UnityEditor.SerializedPropertyType.Vector3:
                valueStr = prop.vector3Value.ToString();
                break;
            case UnityEditor.SerializedPropertyType.ObjectReference:
                try { valueStr = prop.objectReferenceValue.ToString ();} 
                catch (NullReferenceException) { valueStr = "None (Game Object)"; }
                break;
            default:
                valueStr = "(not supported)";
                break;
        }

        EditorGUI.LabelField(position,label.text, valueStr);
    }
}