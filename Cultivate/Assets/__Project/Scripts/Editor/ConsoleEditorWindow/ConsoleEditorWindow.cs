
using UnityEditor;
using System.Linq;
using UnityEngine;

public class ConsoleEditorWindow : EditorWindow
{
    private Vector2 _scrollPosition;
    private int _selectedCharacterIndex;
    private CharacterEntry _selectedCharacter;
    private Profile _currentProfile;

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

    private void OnGUI()
    {
        if (!Application.isPlaying)
        {
            EditorGUILayout.HelpBox("请在运行时打开此窗口", MessageType.Info);
            return;
        }

        _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);

        DrawCharacterDropdown();
        EditorGUILayout.Space(10);
        
        if (_selectedCharacter != null)
        {
            DrawSlotToggles();
            EditorGUILayout.Space(10);
            DrawPackToggles();
        }

        EditorGUILayout.EndScrollView();
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
                _currentProfile.SetSlotUnlocked(_selectedCharacter, i, newValue);
                
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
                    _currentProfile.SetPackUnlocked(pack, newValue);
                    
                    if (AppManager.Instance.ConfigManager != null)
                        AppManager.Instance.ConfigManager.Notify();
                }
            }
            
            EditorGUILayout.EndHorizontal();
        }
    }
}
