using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(RunSkillDescriptor))]
public class RunSkillDescriptorDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);
        
        Rect labelRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
        Rect contentRect = new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight + 2, position.width, EditorGUIUtility.singleLineHeight * 3);
        
        EditorGUI.LabelField(labelRect, label);
        
        // 由于RunSkillDescriptor是引用类型，我们显示一个只读的文本
        EditorGUI.BeginDisabledGroup(true);
        EditorGUI.TextArea(contentRect, "RunSkillDescriptor (引用类型，无法直接编辑)");
        EditorGUI.EndDisabledGroup();
        
        EditorGUI.EndProperty();
    }
    
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUIUtility.singleLineHeight * 4 + 6;
    }
}
