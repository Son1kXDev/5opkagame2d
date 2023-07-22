using UnityEngine;
using UnityEditor;


namespace Editors
{
    public class TextEntryDialog : EditorWindow
    {
        private string m_entryFieldResult;

        private static string _label;

        public static string Show(string windowName, string label)
        {
            TextEntryDialog dialog = CreateInstance<TextEntryDialog>();
            dialog.titleContent = new GUIContent(windowName);

            _label = label;

            dialog.maxSize = new Vector2(320, 120);
            dialog.minSize = new Vector2(320, 120);

            dialog.m_entryFieldResult = string.Empty;

            dialog.ShowModal();

            return dialog.m_entryFieldResult;
        }

        private void OnGUI()
        {
            GUILayout.Space(20);

            GUILayout.Label(_label);

            m_entryFieldResult = EditorGUILayout.TextField(m_entryFieldResult, GUILayout.Width(310));

            GUILayout.FlexibleSpace();

            GUILayout.BeginHorizontal();

            if (GUILayout.Button("Cancel", GUILayout.Width(100)))
            {
                Cancel();
            }

            if (GUILayout.Button("Apply", GUILayout.Width(100)))
            {
                Accept();
            }

            GUILayout.EndHorizontal();

            GUILayout.Space(20);
        }

        private void Accept()
        {
            if (!m_entryFieldResult.IsEmpty())
            {
                Close();
            }
        }

        private void Cancel()
        {
            m_entryFieldResult = string.Empty;
            Close();
        }
    }
}