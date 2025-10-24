using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(RequireCellBehaviorType))]
public class RequireCellBehaviorTypeDrawer : PropertyDrawer
{
    private static readonly string[] behaviorNames = { "耗材", "移除", "提升至当前境界", "提升至下境界", "复制", "五行相生" };
    
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);
        
        Rect labelRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
        Rect selectorRect = new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight + 2, position.width, EditorGUIUtility.singleLineHeight);
        
        EditorGUI.LabelField(labelRect, label);
        
        int currentValue = property.enumValueIndex;
        int newValue = MultiTabSelectorDrawer.DrawMultiTabSelector(selectorRect, behaviorNames, currentValue, false);
        
        if (newValue != currentValue)
        {
            property.enumValueIndex = newValue;
            property.serializedObject.ApplyModifiedProperties();
        }
        
        EditorGUI.EndProperty();
    }
    
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUIUtility.singleLineHeight * 2 + 4;
    }
}
