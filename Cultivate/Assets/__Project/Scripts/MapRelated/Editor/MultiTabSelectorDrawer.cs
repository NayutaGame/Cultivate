using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(MultiTabSelector))]
public class MultiTabSelectorDrawer : PropertyDrawer
{
    private const float BUTTON_SPACING = 2f;
    private const float LABEL_HEIGHT = 18f;
    private const float BUTTON_HEIGHT = 20f;
    
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);
        
        var tabNamesProp = property.FindPropertyRelative("_tabNames");
        var selectedMaskProp = property.FindPropertyRelative("_mask");
        var allowMultipleProp = property.FindPropertyRelative("_allowMultiple");
        
        // 获取标签名称数组
        string[] tabNames = new string[tabNamesProp.arraySize];
        for (int i = 0; i < tabNames.Length; i++)
        {
            tabNames[i] = tabNamesProp.GetArrayElementAtIndex(i).stringValue;
        }
        
        int selectedMask = selectedMaskProp.intValue;
        bool allowMultiple = allowMultipleProp.boolValue;
        
        // 绘制标签
        Rect labelRect = new Rect(position.x, position.y, position.width, LABEL_HEIGHT);
        EditorGUI.LabelField(labelRect, label);
        
        // 绘制 Tab 按钮
        Rect buttonRect = new Rect(position.x, position.y + LABEL_HEIGHT + 2, 
                                 position.width, BUTTON_HEIGHT);
        
        DrawTabButtons(buttonRect, tabNames, selectedMaskProp, allowMultiple);
        
        EditorGUI.EndProperty();
    }
    
    private void DrawTabButtons(Rect position, string[] tabNames, SerializedProperty selectedMaskProp, bool allowMultiple)
    {
        if (tabNames.Length == 0)
        {
            EditorGUI.LabelField(position, "No tabs configured");
            return;
        }
        
        int buttonWidth = Mathf.FloorToInt((position.width - (tabNames.Length - 1) * BUTTON_SPACING) / tabNames.Length);
        int selectedMask = selectedMaskProp.intValue;
        
        for (int i = 0; i < tabNames.Length; i++)
        {
            Rect buttonRect = new Rect(position.x + i * (buttonWidth + BUTTON_SPACING), position.y, buttonWidth, position.height);
            
            bool isSelected = (selectedMask & (1 << i)) != 0;
            
            // 设置按钮样式
            GUIStyle buttonStyle = new GUIStyle(GUI.skin.button);
            if (isSelected)
            {
                // 选中状态：蓝色背景
                buttonStyle.normal.background = MakeTex(2, 2, new Color(0.2f, 0.4f, 0.8f, 1f));
                buttonStyle.normal.textColor = Color.white;
                buttonStyle.hover.background = MakeTex(2, 2, new Color(0.3f, 0.5f, 0.9f, 1f));
                buttonStyle.hover.textColor = Color.white;
            }
            else
            {
                // 未选中状态：默认样式
                buttonStyle.normal.background = MakeTex(2, 2, new Color(0.8f, 0.8f, 0.8f, 1f));
                buttonStyle.normal.textColor = Color.black;
                buttonStyle.hover.background = MakeTex(2, 2, new Color(0.9f, 0.9f, 0.9f, 1f));
                buttonStyle.hover.textColor = Color.black;
            }
            
            // 设置按钮文本样式
            buttonStyle.fontSize = 11;
            buttonStyle.fontStyle = FontStyle.Bold;
            buttonStyle.alignment = TextAnchor.MiddleCenter;
            
            if (GUI.Button(buttonRect, tabNames[i], buttonStyle))
            {
                if (allowMultiple)
                {
                    // 多选：切换状态
                    selectedMask ^= (1 << i);
                }
                else
                {
                    // 单选：清除其他选择
                    selectedMask = (1 << i);
                }
                
                selectedMaskProp.intValue = selectedMask;
                selectedMaskProp.serializedObject.ApplyModifiedProperties();
            }
        }
    }
    
    /// <summary>
    /// 绘制多标签选择器并返回选中的索引
    /// </summary>
    /// <param name="position">绘制位置</param>
    /// <param name="tabNames">标签名称数组</param>
    /// <param name="selectedIndex">当前选中的索引（单选模式）</param>
    /// <param name="allowMultiple">是否允许多选</param>
    /// <returns>新的选中索引（单选模式）或选中的第一个索引（多选模式）</returns>
    public static int DrawMultiTabSelector(Rect position, string[] tabNames, int selectedIndex, bool allowMultiple)
    {
        if (tabNames == null || tabNames.Length == 0)
        {
            EditorGUI.LabelField(position, "No tabs configured");
            return selectedIndex;
        }
        
        const float BUTTON_SPACING = 2f;
        const float BUTTON_HEIGHT = 20f;
        
        int buttonWidth = Mathf.FloorToInt((position.width - (tabNames.Length - 1) * BUTTON_SPACING) / tabNames.Length);
        
        for (int i = 0; i < tabNames.Length; i++)
        {
            Rect buttonRect = new Rect(position.x + i * (buttonWidth + BUTTON_SPACING), position.y, buttonWidth, position.height);
            
            bool isSelected = allowMultiple ? false : (i == selectedIndex);
            
            // 设置按钮样式
            GUIStyle buttonStyle = new GUIStyle(GUI.skin.button);
            if (isSelected)
            {
                // 选中状态：蓝色背景
                buttonStyle.normal.background = MakeTex(2, 2, new Color(0.2f, 0.4f, 0.8f, 1f));
                buttonStyle.normal.textColor = Color.white;
                buttonStyle.hover.background = MakeTex(2, 2, new Color(0.3f, 0.5f, 0.9f, 1f));
                buttonStyle.hover.textColor = Color.white;
            }
            else
            {
                // 未选中状态：默认样式
                buttonStyle.normal.background = MakeTex(2, 2, new Color(0.8f, 0.8f, 0.8f, 1f));
                buttonStyle.normal.textColor = Color.black;
                buttonStyle.hover.background = MakeTex(2, 2, new Color(0.9f, 0.9f, 0.9f, 1f));
                buttonStyle.hover.textColor = Color.black;
            }
            
            // 设置按钮文本样式
            buttonStyle.fontSize = 11;
            buttonStyle.fontStyle = FontStyle.Bold;
            buttonStyle.alignment = TextAnchor.MiddleCenter;
            
            if (GUI.Button(buttonRect, tabNames[i], buttonStyle))
            {
                return i; // 返回点击的索引
            }
        }
        
        return selectedIndex; // 如果没有点击，返回原值
    }
    
    /// <summary>
    /// 绘制多选标签页并返回新的位掩码
    /// </summary>
    /// <param name="position">绘制位置</param>
    /// <param name="tabNames">标签名称数组</param>
    /// <param name="selectedMask">当前选中的位掩码</param>
    /// <returns>新的位掩码</returns>
    public static int DrawMultiSelectTabs(Rect position, string[] tabNames, int selectedMask)
    {
        if (tabNames == null || tabNames.Length == 0)
        {
            EditorGUI.LabelField(position, "No tabs configured");
            return selectedMask;
        }
        
        const float BUTTON_SPACING = 2f;
        const float BUTTON_HEIGHT = 20f;
        
        int buttonWidth = Mathf.FloorToInt((position.width - (tabNames.Length - 1) * BUTTON_SPACING) / tabNames.Length);
        int newMask = selectedMask;
        
        for (int i = 0; i < tabNames.Length; i++)
        {
            Rect buttonRect = new Rect(position.x + i * (buttonWidth + BUTTON_SPACING), position.y, buttonWidth, position.height);
            
            bool isSelected = (selectedMask & (1 << i)) != 0;
            
            // 设置按钮样式
            GUIStyle buttonStyle = new GUIStyle(GUI.skin.button);
            if (isSelected)
            {
                // 选中状态：蓝色背景
                buttonStyle.normal.background = MakeTex(2, 2, new Color(0.2f, 0.4f, 0.8f, 1f));
                buttonStyle.normal.textColor = Color.white;
                buttonStyle.hover.background = MakeTex(2, 2, new Color(0.3f, 0.5f, 0.9f, 1f));
                buttonStyle.hover.textColor = Color.white;
            }
            else
            {
                // 未选中状态：默认样式
                buttonStyle.normal.background = MakeTex(2, 2, new Color(0.8f, 0.8f, 0.8f, 1f));
                buttonStyle.normal.textColor = Color.black;
                buttonStyle.hover.background = MakeTex(2, 2, new Color(0.9f, 0.9f, 0.9f, 1f));
                buttonStyle.hover.textColor = Color.black;
            }
            
            // 设置按钮文本样式
            buttonStyle.fontSize = 11;
            buttonStyle.fontStyle = FontStyle.Bold;
            buttonStyle.alignment = TextAnchor.MiddleCenter;
            
            if (GUI.Button(buttonRect, tabNames[i], buttonStyle))
            {
                // 切换选择状态
                newMask ^= (1 << i);
            }
        }
        
        return newMask;
    }
    
    private static Texture2D MakeTex(int width, int height, Color col)
    {
        Color[] pix = new Color[width * height];
        for (int i = 0; i < pix.Length; i++)
            pix[i] = col;
        Texture2D result = new Texture2D(width, height);
        result.SetPixels(pix);
        result.Apply();
        return result;
    }
    
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return LABEL_HEIGHT + BUTTON_HEIGHT + 4; // 标签 + 按钮 + 间距
    }
}
