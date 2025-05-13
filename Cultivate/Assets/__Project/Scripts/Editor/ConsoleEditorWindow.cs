
using System;
using System.Collections.Generic;
using UnityEditor;
using System.Linq;
using UnityEngine;

public class ConsoleEditorWindow : EditorWindow
{
    private enum Tab
    {
        UnlockContent,  // 角色/槽位/卡包
        Achievements,   // 成就列表
        Analytics      // 数据分析
    }
    
    private Vector2 _scrollPosition;
    private int _selectedCharacterIndex;
    private CharacterEntry _selectedCharacter;
    private Profile _currentProfile;
    
    private Tab _currentTab = Tab.UnlockContent;

    [MenuItem("Tools/ConsoleEditorWindow")]
    public static void ShowWindow()
    {
        GetWindow<ConsoleEditorWindow>("ConsoleEditorWindow");
    }
    
    public ConsoleEditorWindow()
    {
        _filterTabs = new[]
        {
            new FilterTab("法力消耗", s => 
                s.GetLiteralCostDescription(s.LowestJingJie).Type == CostType.Mana),
            new FilterTab("气血消耗", s => 
                s.GetLiteralCostDescription(s.LowestJingJie).Type == CostType.Health),
            new FilterTab("引导消耗", s => 
                s.GetLiteralCostDescription(s.LowestJingJie).Type == CostType.Channel),
            new FilterTab("灵气牌", s => 
                s.GetSkillTypeComposite().Contains(SkillType.Mana)),
        };
    }

    private void OnEnable()
    {
        if (Application.isPlaying)
        {
            _currentProfile = AppManager.Instance.ProfileManager.GetCurrProfile();
        }
    }

    private void DrawCharacterDropdown()
    {
        var characters = Encyclopedia.CharacterCategory.Traversal.ToList();
        var characterNames = characters.Select(c => c.GetName()).ToArray();
        
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("选择角色", GUILayout.Width(60));
        
        int newIndex = EditorGUILayout.Popup(_selectedCharacterIndex, characterNames);
        if (newIndex != _selectedCharacterIndex)
        {
            _selectedCharacterIndex = newIndex;
            _selectedCharacter = characters[newIndex];
        }
        
        EditorGUILayout.EndHorizontal();
    }

    private void DrawSlotToggles()
    {
        EditorGUILayout.LabelField("槽位解锁状态", EditorStyles.boldLabel);
        
        EditorGUILayout.BeginHorizontal();
        for (int i = 0; i < 7; i++)
        {
            bool isUnlocked = _currentProfile.SlotIsUnlocked(_selectedCharacter, i);
            bool newValue = EditorGUILayout.Toggle($"槽位{i}", isUnlocked);
            
            if (newValue != isUnlocked)
            {
                _currentProfile.SetSlotUnlockedQuietly(_selectedCharacter, i, newValue);
                
                if (AppManager.Instance.ConfigManager != null)
                    AppManager.Instance.ConfigManager.Notify();
            }
        }
        EditorGUILayout.EndHorizontal();
    }

    private void DrawPackToggles()
    {
        EditorGUILayout.LabelField("卡包解锁状态", EditorStyles.boldLabel);
        
        var packs = Encyclopedia.PackCategory.Traversal.ToList();
        int columns = 4; // 每行显示的数量
        
        for (int i = 0; i < packs.Count; i += columns)
        {
            EditorGUILayout.BeginHorizontal();
            
            for (int j = 0; j < columns && i + j < packs.Count; j++)
            {
                var pack = packs[i + j];
                bool isUnlocked = _currentProfile.PackIsUnlocked(pack);
                bool newValue = EditorGUILayout.Toggle(pack.Name, isUnlocked);
                
                if (newValue != isUnlocked)
                {
                    _currentProfile.SetPackUnlockedQuietly(pack, newValue);
                    
                    if (AppManager.Instance.ConfigManager != null)
                        AppManager.Instance.ConfigManager.Notify();
                }
            }
            
            EditorGUILayout.EndHorizontal();
        }
    }

    private SkillDistributionKey? _selectedKey = null;  // 当前选中的分组

    private class FilterTab
    {
        public string Label { get; }
        public Func<SkillEntry, bool> Predicate { get; }

        public FilterTab(string label, Func<SkillEntry, bool> predicate)
        {
            Label = label;
            Predicate = predicate;
        }
    }

    private readonly FilterTab[] _filterTabs;
    private int _currentFilterTab = 0;
    
    private void DrawAnalyticsTab()
    {
        EditorGUILayout.LabelField("分布", EditorStyles.boldLabel);
        EditorGUILayout.Space(5);
    
        // 绘制过滤器Tab组
        EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
        for (int i = 0; i < _filterTabs.Length; i++)
        {
            if (GUILayout.Toggle(_currentFilterTab == i, _filterTabs[i].Label, EditorStyles.toolbarButton))
                _currentFilterTab = i;
        }
        EditorGUILayout.EndHorizontal();
        
        EditorGUILayout.Space(5);
        
        // 使用当前选中的过滤器
        var skills = Encyclopedia.SkillCategory.Traversal
            .Where(_filterTabs[_currentFilterTab].Predicate);

        var distribution = skills
            .GroupBy(s => new SkillDistributionKey(
                s.LowestJingJie,
                s.GetWuXing()
            ))
            .ToDictionary(
                g => g.Key,
                g => g.ToList()
            );

        // 绘制表格
        EditorGUILayout.BeginVertical("box");
        DrawAnalyticsTableHeader();
        
        foreach (var jingJie in JingJie.Traversal)
        {
            DrawAnalyticsTableRow(jingJie, distribution);
        }
        
        EditorGUILayout.EndVertical();

        // 显示选中分组的详细信息
        if (_selectedKey.HasValue && distribution.ContainsKey(_selectedKey.Value))
        {
            EditorGUILayout.Space(10);
            EditorGUILayout.BeginVertical("box");
            
            var selectedSkills = distribution[_selectedKey.Value];
            EditorGUILayout.LabelField(
                $"境界：{_selectedKey.Value.JingJie} 五行：{_selectedKey.Value.WuXing} " +
                $"(共{selectedSkills.Count}个)",
                EditorStyles.boldLabel
            );

            foreach (var skill in selectedSkills)
            {
                EditorGUILayout.LabelField(skill.GetName());
            }
            
            EditorGUILayout.EndVertical();
        }
    }

    private void DrawAnalyticsTableHeader()
    {
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("境界/五行", GUILayout.Width(100));
        foreach (WuXing wuxing in WuXing.Traversal)
        {
            GUILayout.Label(wuxing.ToString(), GUILayout.Width(50));
        }
        EditorGUILayout.EndHorizontal();
    }
    
    private void DrawAnalyticsTableRow(JingJie jingJie, Dictionary<SkillDistributionKey, List<SkillEntry>> distribution)
    {
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label(jingJie.ToString(), GUILayout.Width(100));
        
        foreach (WuXing wuxing in WuXing.Traversal)
        {
            var key = new SkillDistributionKey(jingJie, wuxing);
            int count = distribution.GetValueOrDefault(key)?.Count ?? 0;
            
            var originalColor = GUI.backgroundColor;
            GUI.backgroundColor = count == 0 ? Color.gray : Color.white;
            
            if (GUILayout.Button(count.ToString(), GUILayout.Width(50)))
            {
                _selectedKey = count > 0 ? key : null;
            }
            
            GUI.backgroundColor = originalColor;
        }
        
        EditorGUILayout.EndHorizontal();
    }

    private void OnGUI()
    {
        if (!Application.isPlaying)
        {
            EditorGUILayout.HelpBox("请在运行时打开此窗口", MessageType.Info);
            return;
        }

        // 绘制Tab按钮
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Toggle(_currentTab == Tab.UnlockContent, "解锁内容", EditorStyles.toolbarButton))
            _currentTab = Tab.UnlockContent;
        if (GUILayout.Toggle(_currentTab == Tab.Achievements, "成就列表", EditorStyles.toolbarButton))
            _currentTab = Tab.Achievements;
        if (GUILayout.Toggle(_currentTab == Tab.Analytics, "数据分析", EditorStyles.toolbarButton))
            _currentTab = Tab.Analytics;
        EditorGUILayout.EndHorizontal();
        
        EditorGUILayout.Space(10);
        
        _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);
        
        switch (_currentTab)
        {
            case Tab.UnlockContent:
                DrawUnlockContentTab();
                break;
            case Tab.Achievements:
                DrawAchievementsTab();
                break;
            case Tab.Analytics:
                DrawAnalyticsTab();
                break;
        }
        
        EditorGUILayout.EndScrollView();
    }
    
    private void DrawUnlockContentTab()
    {
        // 原有的角色/槽位/卡包解锁UI
        DrawCharacterDropdown();
        EditorGUILayout.Space(10);
        
        if (_selectedCharacter != null)
        {
            DrawSlotToggles();
            EditorGUILayout.Space(10);
        }
        
        DrawPackToggles();
    }
    
    private void DrawAchievementsTab()
    {
        var profile = AppManager.Instance.ProfileManager.GetCurrProfile();
        
        foreach (var achievementProfile in profile.AchievementProfileList.Traversal())
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            
            // 成就名称和描述
            EditorGUILayout.LabelField(achievementProfile.GetEntry().GetName(), EditorStyles.boldLabel);
            EditorGUILayout.LabelField(achievementProfile.GetEntry().GetConditionDescription());
            
            // 解锁状态
            bool isUnlocked = achievementProfile.IsUnlocked();
            bool newValue = EditorGUILayout.Toggle("已解锁", isUnlocked);
            if (newValue != isUnlocked)
            {
                achievementProfile.SetUnlockedQuietly(newValue);
                if (AppManager.Instance.ConfigManager != null)
                    AppManager.Instance.ConfigManager.Notify();
            }
            
            // 显示解锁效果
            EditorGUILayout.LabelField($"解锁效果：{achievementProfile.GetEntry().GetRewardDescription()}");
            
            EditorGUILayout.EndVertical();
            EditorGUILayout.Space(5);
        }
    }
}
