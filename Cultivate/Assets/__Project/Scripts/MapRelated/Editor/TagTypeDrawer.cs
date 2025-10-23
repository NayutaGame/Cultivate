using UnityEngine;
using UnityEditor;
using PuppyDragon.uNody;

[CustomPropertyDrawer(typeof(TagType))]
public class TagTypeDrawer : PropertyDrawer
{
    private static readonly string[] tagNames = { "攻击", "防御", "灵气", "气血", "二动", "自指", "升华", "一次性" };
    
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);
        
        // 创建标签
        Rect labelRect = new Rect(position.x, position.y, EditorGUIUtility.labelWidth, position.height);
        EditorGUI.LabelField(labelRect, label);
        
        // 创建选择器区域
        Rect selectorRect = new Rect(position.x + EditorGUIUtility.labelWidth, position.y, 
                                   position.width - EditorGUIUtility.labelWidth, position.height);
        
        // 获取当前值（作为TagType枚举）
        TagType currentValue = (TagType)property.intValue;
        
        // 使用 MultiTabSelectorDrawer 的样式绘制多选
        int newMask = MultiTabSelectorDrawer.DrawMultiSelectTabs(selectorRect, tagNames, (int)currentValue);
        
        // 如果值改变了，更新属性
        if (newMask != (int)currentValue)
        {
            property.intValue = newMask;
            property.serializedObject.ApplyModifiedProperties();
        }
        
        EditorGUI.EndProperty();
    }
    
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUIUtility.singleLineHeight;
    }
}
