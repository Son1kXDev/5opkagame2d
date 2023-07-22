using System;
namespace NVWEditorButton
{
    [AttributeUsage(AttributeTargets.Method)]
    public class EditorButtonAttribute : Attribute
    {
        public string TextButton;

        public EditorButtonAttribute(string name)
        {
            this.TextButton = name;
        }
    }
}


