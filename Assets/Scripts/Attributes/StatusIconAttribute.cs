using System.Reflection;
using System;
using UnityEngine;

[AttributeUsage(AttributeTargets.Field)]
public class StatusIconAttribute : PropertyAttribute
{
    public string? str { get; private set; } = null;
    public int? num { get; private set; } = null;
    public float? flt { get; private set; } = null;
    public float? minInt { get; private set; } = null;
    public float? maxInt { get; private set; } = null;
    public float? minFlt { get; private set; } = null;
    public float? maxFlt { get; private set; } = null;

    public StatusIconAttribute() { }
    public StatusIconAttribute(string value) { str = value; }
    public StatusIconAttribute(int value) { num = value; }
    public StatusIconAttribute(int minValue, int maxValue = int.MaxValue) { minInt = minValue; maxInt = maxValue; }
    public StatusIconAttribute(float value) { flt = value; }
    public StatusIconAttribute(float minValue, float maxValue = float.MaxValue) { minFlt = minValue; maxFlt = maxValue; }
}

