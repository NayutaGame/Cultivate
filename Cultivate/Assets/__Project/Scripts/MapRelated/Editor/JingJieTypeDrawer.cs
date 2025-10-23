using UnityEngine;
using UnityEditor;
using PuppyDragon.uNody;

[CustomPropertyDrawer(typeof(JingJieType))]
public class JingJieTypeDrawer : PropertyDrawer
{
    private static readonly string[] jingJieNames = { "练气", "筑基", "金丹", "元婴", "化神", "返虚" };
    
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);
        
        // 创建标签
        Rect labelRect = new Rect(position.x, position.y, EditorGUIUtility.labelWidth, position.height);
        EditorGUI.LabelField(labelRect, label);
        
        // 创建选择器区域
        Rect selectorRect = new Rect(position.x + EditorGUIUtility.labelWidth, position.y, 
                                   position.width - EditorGUIUtility.labelWidth, position.height);
        
        // 获取当前值
        int currentValue = property.enumValueIndex;
        
        // 使用 MultiTabSelectorDrawer 的样式绘制
        int newValue = MultiTabSelectorDrawer.DrawMultiTabSelector(selectorRect, jingJieNames, currentValue, false);
        
        // 如果值改变了，更新属性
        if (newValue != currentValue)
        {
            property.enumValueIndex = newValue;
            property.serializedObject.ApplyModifiedProperties();
        }
        
        EditorGUI.EndProperty();
    }
    
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUIUtility.singleLineHeight;
    }
}
