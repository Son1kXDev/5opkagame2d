using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using Enjine.Weapons;
using Enjine.Weapons.Components;
using System.Linq;

[CustomEditor(typeof(WeaponData))]
public class WeaponDataEditor : Editor
{
    private static List<Type> _dataComponentTypes = new List<Type>();
    private WeaponData _data;

    private bool _showForceUpdateButtons;
    private bool _showAddComponentButtons;

    private void OnEnable()
    {
        _data = target as WeaponData;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        _showAddComponentButtons = EditorGUILayout.Foldout(_showAddComponentButtons, "Add Components");

        if (_showAddComponentButtons)
        {
            foreach (var dataComponentType in _dataComponentTypes)
            {
                if (GUILayout.Button(dataComponentType.Name))
                {
                    var component = Activator.CreateInstance(dataComponentType) as ComponentData;

                    component.InitializeAttackData(_data.NumberOfAttacks);
                    _data.AddData(component);
                }
            }
        }

        _showForceUpdateButtons = EditorGUILayout.Foldout(_showForceUpdateButtons, "Force Update");

        if (_showForceUpdateButtons)
        {
            if (GUILayout.Button("Set number of attacks"))
                foreach (var item in _data.ComponentsData)
                    item.InitializeAttackData(_data.NumberOfAttacks);


            if (GUILayout.Button("Set names for attacks"))
                foreach (var item in _data.ComponentsData)
                    item.SetAttackDataNames();


            if (GUILayout.Button("Set names for attacks phases"))
                foreach (var item in _data.ComponentsData)
                    item.SetAttackDataPhasesNames();

        }
    }

    [DidReloadScripts]
    private static void OnRecompile()
    {
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();
        var types = assemblies.SelectMany(assembly => assembly.GetTypes());
        var filteredTypes = types.Where(
            type => type.IsSubclassOf(typeof(ComponentData)) && !type.ContainsGenericParameters && type.IsClass);

        _dataComponentTypes = filteredTypes.ToList();
    }
}
