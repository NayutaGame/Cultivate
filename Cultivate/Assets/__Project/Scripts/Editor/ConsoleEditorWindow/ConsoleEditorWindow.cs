
using UnityEditor;
using System.Linq;
using UnityEngine;

public class ConsoleEditorWindow : EditorWindow
{
    private enum Tab
    {
        UnlockContent,  // 角色/槽位/卡包
        Achievements    // 成就列表
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
        EditorGUILayout.EndHorizontal();
        
        EditorGUILayout.Space(10);
        
        _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);
        
        // 根据当前Tab绘制不同内容
        switch (_currentTab)
        {
            case Tab.UnlockContent:
                DrawUnlockContentTab();
                break;
            case Tab.Achievements:
                DrawAchievementsTab();
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
