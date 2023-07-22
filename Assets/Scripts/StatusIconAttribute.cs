using System.Reflection;
using System;
using UnityEngine;

[AttributeUsage(AttributeTargets.Field)]
public class StatusIconAttribute : PropertyAttribute
{
    public string? str { get; private set; } = null;
    public int? num { get; private set; } = null;
    public float? flt { get; private set; } = null;
    public StatusIconAttribute() { }
    public StatusIconAttribute(string value) { str = value; }
    public StatusIconAttribute(int value) { num = value; }
    public StatusIconAttribute(float value) { flt = value; }
}

