using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.Reflection;
using UnityEngine.UIElements;

[CustomPropertyDrawer(typeof(StatusIconAttribute))]
public class StatusIconPropertyDrawer : PropertyDrawer
{

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var attr = attribute as StatusIconAttribute;
        if (attr.str != null)
            property.PropertyField(position, label,
            property.stringValue != attr.str ? PropertyIconType.Confirm : PropertyIconType.Error);
        else if (attr.num != null)
            property.PropertyField(position, label,
            property.intValue != attr.num ? PropertyIconType.Confirm : PropertyIconType.Error);
        else if (attr.flt != null)
            property.PropertyField(position, label,
            property.floatValue != attr.flt ? PropertyIconType.Confirm : PropertyIconType.Error);
        else if (attr.minFlt != null)
            property.PropertyField(position, label,
            property.floatValue > attr.minFlt && property.floatValue < attr.maxFlt ? PropertyIconType.Confirm : PropertyIconType.Error);
        else property.PropertyField(position, label,
        property.objectReferenceValue != null ? PropertyIconType.Confirm : PropertyIconType.Error);
    }

}
