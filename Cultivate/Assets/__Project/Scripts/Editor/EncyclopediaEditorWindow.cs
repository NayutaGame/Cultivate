
using System.Linq;
using UnityEditor;
using UnityEngine;

public class EncyclopediaEditorWindow : EditorWindow
{
    private enum Tab
    {
        Buff,
        Skill
    }

    private Tab _currentTab = Tab.Buff;
    private Vector2 _listScrollPosition;
    private Vector2 _detailScrollPosition;
    private int _selectedBuffIndex = -1;
    private int _selectedSkillIndex = -1;

    [MenuItem("Tools/Encyclopedia Editor")]
    public static void ShowWindow()
    {
        GetWindow<EncyclopediaEditorWindow>("Encyclopedia Editor");
    }

    private void OnGUI()
    {
        // 绘制顶部标签页
        EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
        if (GUILayout.Toggle(_currentTab == Tab.Buff, "Buff", EditorStyles.toolbarButton))
            _currentTab = Tab.Buff;
        if (GUILayout.Toggle(_currentTab == Tab.Skill, "Skill", EditorStyles.toolbarButton))
            _currentTab = Tab.Skill;
        EditorGUILayout.EndHorizontal();

        // 绘制主要内容区域
        EditorGUILayout.BeginHorizontal();
        
        // 左侧列表 (1/3宽度)
        EditorGUILayout.BeginVertical(GUILayout.Width(position.width / 3));
        DrawListView();
        EditorGUILayout.EndVertical();
        
        // 分隔线
        EditorGUILayout.Space(2);
        
        // 右侧详情 (2/3宽度)
        EditorGUILayout.BeginVertical(GUILayout.Width(2 * position.width / 3 - 10));
        DrawDetailView();
        EditorGUILayout.EndVertical();
        
        EditorGUILayout.EndHorizontal();
    }

    private void DrawListView()
    {
        EditorGUILayout.BeginVertical("box");
        _listScrollPosition = EditorGUILayout.BeginScrollView(_listScrollPosition);

        if (_currentTab == Tab.Buff)
        {
            var buffs = Encyclopedia.BuffCategory.ToList();
            for (int i = 0; i < buffs.Count; i++)
            {
                if (GUILayout.Toggle(_selectedBuffIndex == i, buffs[i].GetName(), EditorStyles.radioButton))
                {
                    _selectedBuffIndex = i;
                }
            }
        }
        else
        {
            var skills = Encyclopedia.SkillCategory.ToList();
            for (int i = 0; i < skills.Count; i++)
            {
                if (GUILayout.Toggle(_selectedSkillIndex == i, skills[i].GetName(), EditorStyles.radioButton))
                {
                    _selectedSkillIndex = i;
                }
            }
        }

        EditorGUILayout.EndScrollView();
        EditorGUILayout.EndVertical();
    }

    private void DrawDetailView()
    {
        EditorGUILayout.BeginVertical("box");
        _detailScrollPosition = EditorGUILayout.BeginScrollView(_detailScrollPosition);

        if (_currentTab == Tab.Buff)
        {
            if (_selectedBuffIndex >= 0)
            {
                var buff = Encyclopedia.BuffCategory.ElementAt(_selectedBuffIndex);
                DrawBuffDetails(buff);
            }
        }
        else
        {
            if (_selectedSkillIndex >= 0)
            {
                var skill = Encyclopedia.SkillCategory.ElementAt(_selectedSkillIndex);
                DrawSkillDetails(skill);
            }
        }

        EditorGUILayout.EndScrollView();
        EditorGUILayout.EndVertical();
    }

    private void DrawBuffDetails(BuffEntry buff)
    {
        EditorGUILayout.LabelField("Buff Details", EditorStyles.boldLabel);
        EditorGUILayout.Space();
        
        EditorGUILayout.LabelField("Name", buff.GetName());
        EditorGUILayout.LabelField("Description", buff.GetLiteralDescription().ToString());
        // ... 添加更多 Buff 相关信息
    }

    private void DrawSkillDetails(SkillEntry skill)
    {
        EditorGUILayout.LabelField("基础信息", EditorStyles.boldLabel);
        EditorGUILayout.Space();
        
        // 基础属性
        EditorGUILayout.LabelField($"ID: {skill.GetId()}");
        EditorGUILayout.LabelField($"名称: {skill.GetName()}");
        EditorGUILayout.LabelField($"五行: {skill.GetWuXing()?.ToString() ?? "无"}");
        EditorGUILayout.LabelField($"境界范围: {skill.LowestJingJie} ~ {skill.HighestJingJie}");
        
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("技能类型", EditorStyles.boldLabel);
        // var types = skill.GetSkillTypeComposite().ToString().Split(',');
        // foreach (var type in types)
        // {
        //     EditorGUILayout.LabelField($"• {type.Trim()}");
        // }
        
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("消耗描述", EditorStyles.boldLabel);
        var costDesc = skill.GetLiteralCostDescription(skill.LowestJingJie);
        EditorGUILayout.LabelField($"类型: {costDesc.Type}");
        DrawStyledText($"描述: {costDesc}");
        
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("技能效果", EditorStyles.boldLabel);
        DrawStyledText(skill.GetLiteralDescription().ToString());
        
        // if (!string.IsNullOrEmpty(skill.GetCascadeAnnotated()))
        // {
        //     EditorGUILayout.Space();
        //     EditorGUILayout.LabelField("技能说明", EditorStyles.boldLabel);
        //     DrawStyledText(skill.GetCascadeAnnotated());
        // }
        
        if (!string.IsNullOrEmpty(skill.GetTrivia()))
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("背景故事", EditorStyles.boldLabel);
            DrawStyledText(skill.GetTrivia());
        }
        
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("技能图标", EditorStyles.boldLabel);
        var sprite = skill.GetSprite();
        if (sprite != null)
        {
            float aspectRatio = 285f / 311f;
            float width = 285;
            float height = width / aspectRatio;
            
            var rect = EditorGUILayout.GetControlRect(GUILayout.Width(width), GUILayout.Height(height));
            EditorGUI.DrawPreviewTexture(rect, sprite.texture, null, ScaleMode.ScaleToFit);
        }
    }
    
    private void DrawStyledText(string text)
    {
        if (string.IsNullOrEmpty(text)) return;

        var style = new GUIStyle(EditorStyles.label);
        style.richText = true;
        style.wordWrap = true;

        // 处理样式标签
        text = ProcessStyleTags(text);
        
        EditorGUILayout.LabelField(text, style);
    }

    private string ProcessStyleTags(string text)
    {
        // 处理样式标签
        text = System.Text.RegularExpressions.Regex.Replace(
            text,
            @"<style=""([^""]+)"">([^<]+)</style>",
            match =>
            {
                string styleType = match.Groups[1].Value;
                string content = match.Groups[2].Value;
                
                return styleType switch
                {
                    "Attack" => $"<color=red>{content}</color>",
                    "Highlight" => $"<color=yellow>{content}</color>",
                    "Defense" => $"<color=blue>{content}</color>",
                    _ => content
                };
            }
        );
        
        return text;
    }
}
