using UnityEditor;
using UnityEngine;

namespace App.AppLibs
{
    /// <summary>
    /// 編集不可属性
    /// </summary>
    public class DisabledAttribute : PropertyAttribute {}

#if UNITY_EDITOR
    /// <summary>
    /// 編集不可
    /// </summary>
    [CustomPropertyDrawer(typeof(DisabledAttribute))]
    public class DisabledAttributeDrawer : PropertyDrawer
    {
        /// <inheritdoc/>
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, label, true);
        }

        /// <inheritdoc/>
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            GUI.enabled = false;
            EditorGUI.PropertyField(position, property, label, true);
            GUI.enabled = true;
        }
    }
#endif
}
