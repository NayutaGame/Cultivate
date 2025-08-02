
using System;
using System.Collections.Generic;
using System.Linq;
using CLLibrary;
using UnityEngine;
using UnityEngine.Assertions;

public class ConfigManager : Addressable
{
    private CharacterProfile _character;

    private ListModel<PackConstraint> _packConstraints;
    private ListModel<ConfigPack> _packSelections;
    public ListModel<ConfigPack> PackSelections => _packSelections;
    
    private static readonly Dictionary<string, Func<object, object>> Accessor = new()
    {
        { "PackConstraints",            thisObject => ((ConfigManager)thisObject)._packConstraints },
        { "PackSelections",             thisObject => ((ConfigManager)thisObject)._packSelections },
    };
    public object Get(string s) => Accessor[s](this);
    public ConfigManager()
    {
        InitPack();
        SelectFirstCharacter();
    }

    private void InitPack()
    {
        _packConstraints = new();
        _packConstraints.Add(new PackConstraint(PackDescriptor.FromWuXing(WuXing.Jin), Encyclopedia.SpriteCategory.FromName("PackConstraints金"), 0));
        _packConstraints.Add(new PackConstraint(PackDescriptor.FromWuXing(WuXing.Shui), Encyclopedia.SpriteCategory.FromName("PackConstraints水"), 1));
        _packConstraints.Add(new PackConstraint(PackDescriptor.FromWuXing(WuXing.Mu), Encyclopedia.SpriteCategory.FromName("PackConstraints木"), 2));
        _packConstraints.Add(new PackConstraint(PackDescriptor.FromWuXing(WuXing.Huo), Encyclopedia.SpriteCategory.FromName("PackConstraints火"), 3));
        _packConstraints.Add(new PackConstraint(PackDescriptor.FromWuXing(WuXing.Tu), Encyclopedia.SpriteCategory.FromName("PackConstraints土"), 4));
        _packConstraints.Add(new PackConstraint(PackDescriptor.AnyPack(), Encyclopedia.SpriteCategory.FromName("PackConstraints任意"), 5));
        _packConstraints.Add(new PackConstraint(PackDescriptor.AnyPack(), Encyclopedia.SpriteCategory.FromName("PackConstraints任意"), 6));

        _packSelections = new();
        Encyclopedia.PackCategory.Do(pack => _packSelections.Add(new ConfigPack(pack)));
    }

    #region 角色配置

    public Neuron<CharacterSelectDetails> CharacterSelectNeuron = new();

    public CharacterProfile SelectedCharacter => _character;

    public void SelectFirstCharacter()
    {
        SelectCharacterProcedure(new CharacterSelectDetails(AppManager.Instance.ProfileManager.GetCurrProfile().FirstCharacterProfile()));
    }

    public void SelectCharacterProcedure(CharacterSelectDetails d)
    {
        _character = d.Character;

        LoadPackPresetFromCharacter(_character);
    }

    private void LoadPackPresetFromCharacter(CharacterProfile character)
    {
        PackPreset preset = character.GetEntry()._packPreset;
        LoadPackPreset(preset);
    }

    public PackPreset WriteCurrentIntoPackPreset()
    {
        List<PackEntry> packEntries = new();
        _packConstraints.Do(c => packEntries.Add(c.Pack.Entry));
        return new PackPreset(packEntries);
    }

    public void LoadPackPreset(PackPreset preset)
    {
        _packConstraints.Do(c => c.Pack = null);
        _packSelections.Do(p => p.IsEquipped = false);

        for(int i = 0; i < preset.PackEntries.Count; i++)
        {
            PackEntry pack = preset.PackEntries[i];
            ConfigPack configPack = _packSelections.First(p => p.Entry == pack);
            configPack.IsEquipped = true;

            _packConstraints[i].Pack = configPack;
        }
    }

    #endregion

    #region 卡包配置

    public Neuron<PackEquipDetails> EquipPackNeuron = new();
    public Neuron<PackUnequipDetails> UnequipPackNeuron = new();

    public void PackSelectionClickedProcedure(PackSelectionClickedDetails d)
    {
        if (d.Pack.IsEquipped)
        {
            UnequipPack(d);
        }
        else
        {
            TryEquipPack(d);
        }
    }

    public void PackConstraintClickedProcedure(PackConstraintClickedDetails d)
    {
        if (d.Constraint.Pack != null)
        {
            UnequipPack(d);
        }
    }

    public void TryEquipPack(PackSelectionClickedDetails d)
    {
        ConfigPack pack = d.Pack;
        if (pack.IsEquipped)
        {
            Debug.Log($"卡包 {pack.Entry.GetName()} 已经被装备");
            return;
        }
        
        PackConstraint firstMatch = GetFirstValidUnlockedSlot(pack);
        if (firstMatch == null)
        {
            Debug.Log("没有合适的空槽位");
            return;
        }

        pack.IsEquipped = true;
        firstMatch.Pack = pack;
        
        int selectionIndex = _packSelections.IndexOf(pack);
        int constraintIndex = _packConstraints.IndexOf(firstMatch);
        EquipPackNeuron.Invoke(new PackEquipDetails(pack, firstMatch, selectionIndex, constraintIndex));
    }

    public void UnequipPack(PackSelectionClickedDetails d)
    {
        ConfigPack pack = d.Pack;
        // 检查卡包是否已装备
        if (!pack.IsEquipped)
        {
            Debug.Log($"卡包 {pack.Entry.GetName()} 未被装备");
            return;
        }

        // 检查pack是否解锁
        bool packIsUnlocked = PackIsGenerallyUnlocked(pack.Entry);
        if (!packIsUnlocked)
        {
            Debug.Log($"卡包 {pack.Entry.GetName()} 未解锁");
            return;
        }
        
        // 寻找装备该卡包的槽位
        PackConstraint constraint = _packConstraints.First(c => c.Pack == pack);
        if (constraint == null)
        {
            Debug.Log("未找到装备该卡包的槽位");
            return;
        }

        // 检查slot是否解锁
        bool slotIsUnlocked = ConstraintIsUnlocked(constraint);
        if (!slotIsUnlocked)
        {
            Debug.Log($"槽位 {constraint.SlotIndex} 未解锁");
            return;
        }

        pack.IsEquipped = false;
        constraint.Pack = null;
        
        int constraintIndex = _packConstraints.IndexOf(constraint);
        int selectionIndex = _packSelections.IndexOf(pack);
        UnequipPackNeuron.Invoke(new PackUnequipDetails(constraint, pack, constraintIndex, selectionIndex));
    }

    public void UnequipPack(PackConstraintClickedDetails d)
    {
        PackConstraint constraint = d.Constraint;
        // 检查槽位是否有装备
        if (constraint.IsEmpty)
        {
            Debug.Log("此槽位没有装备卡包");
            return;
        }

        // 检查pack是否解锁
        bool packIsUnlocked = PackIsGenerallyUnlocked(constraint.Pack.Entry);
        if (!packIsUnlocked)
        {
            Debug.Log($"卡包 {constraint.Pack.Entry.GetName()} 未解锁");
            return;
        }

        // 检查slot是否解锁
        bool slotIsUnlocked = ConstraintIsUnlocked(constraint);
        if (!slotIsUnlocked)
        {
            Debug.Log($"槽位 {constraint.SlotIndex} 未解锁");
            return;
        }

        ConfigPack pack = constraint.Pack;
        constraint.Pack.IsEquipped = false;
        constraint.Pack = null;
        
        
        int constraintIndex = _packConstraints.IndexOf(constraint);
        int selectionIndex = _packSelections.IndexOf(pack);
        UnequipPackNeuron.Invoke(new PackUnequipDetails(constraint, pack, constraintIndex, selectionIndex));
    }

    public PackConstraint GetFirstValidUnlockedSlot(ConfigPack pack)
    {
        return _packConstraints.First(constraint => 
            IsCompatible(pack, constraint) && 
            constraint.IsEmpty);
    }

    public bool IsCompatible(ConfigPack pack, PackConstraint constraint)
    {
        var profile = AppManager.Instance.ProfileManager.GetCurrProfile();
        return constraint.Descriptor.Contains(pack.Entry) && 
               profile.PackIsGenerallyUnlocked(_character.GetEntry(), pack.Entry, constraint.SlotIndex);
    }

    public bool PackIsGenerallyUnlocked(PackEntry pack)
    {
        return AppManager.Instance.ProfileManager.GetCurrProfile().PackIsGenerallyUnlocked(_character.GetEntry(), pack);
    }

    public Description GetPackUnlockCondition(PackEntry pack)
    {
        return AppManager.Instance.ProfileManager.GetCurrProfile().GetPackUnlockCondition(_character.GetEntry(), pack);
    }

    public bool ConstraintIsUnlocked(PackConstraint constraint)
    {
        return AppManager.Instance.ProfileManager.GetCurrProfile().SlotIsUnlocked(_character.GetEntry(), constraint.SlotIndex);
    }

    public Description GetConstraintUnlockCondition(PackConstraint constraint)
    {
        return AppManager.Instance.ProfileManager.GetCurrProfile().GetConstraintUnlockCondition(_character.GetEntry(), constraint.SlotIndex);
    }

    public List<PackEntry> GetEquippedPacks()
    {
        Assert.IsTrue(_packConstraints.All(c => c.Pack != null));
        return _packConstraints.Map(c => c.Pack.Entry).ToList();
    }

    public bool IsConfigurationValid()
    {
        PackConstraint firstInvalid = _packConstraints.First(constraint => constraint.IsEmpty || !constraint.Descriptor.Contains(constraint.Pack.Entry));
        return firstInvalid == null;
    }

    #endregion

    #region MyRegion

    public void Notify()
    {
    }

    #endregion
}
