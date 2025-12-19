
using System;
using System.Collections.Generic;
using System.Linq;
using CLLibrary;
using UnityEngine;
using UnityEngine.Assertions;

public class PackRunConfigTabControl : RunConfigTabControl
{
    public Neuron<PackEquipDetails> EquipPackNeuron = new();
    public Neuron<PackUnequipDetails> UnequipPackNeuron = new();
    
    private ListModel<PackConstraint> _packConstraints;
    private ListModel<ConfigPack> _packSelections;
    public ListModel<ConfigPack> PackSelections => _packSelections;
    
    private PackPreset _recordedPackPreset;
    
    private static readonly Dictionary<string, Func<object, object>> Accessor = new()
    {
        // { "Slots",                      thisObject => ((RunConfigTabControl)thisObject)._filteredSlots },
        { "PackConstraints",            thisObject => ((PackRunConfigTabControl)thisObject)._packConstraints },
        { "PackSelections",             thisObject => ((PackRunConfigTabControl)thisObject)._packSelections },
    };
    public override object Get(string s) => Accessor[s](this);
    public PackRunConfigTabControl()
    {
        InitPack();
    }

    private void InitPack()
    {
        _packConstraints = new();
        _packConstraints.Add(new PackConstraint(PackDescriptor.FromWuXing(WuXing.Jin), 0));
        _packConstraints.Add(new PackConstraint(PackDescriptor.FromWuXing(WuXing.Shui), 1));
        _packConstraints.Add(new PackConstraint(PackDescriptor.FromWuXing(WuXing.Mu), 2));
        _packConstraints.Add(new PackConstraint(PackDescriptor.FromWuXing(WuXing.Huo), 3));
        _packConstraints.Add(new PackConstraint(PackDescriptor.FromWuXing(WuXing.Tu), 4));
        _packConstraints.Add(new PackConstraint(PackDescriptor.AnyPack(), 5));
        _packConstraints.Add(new PackConstraint(PackDescriptor.AnyPack(), 6));

        _packSelections = new();
        Encyclopedia.PackCategory.Do(pack => _packSelections.Add(new ConfigPack(pack)));
    }

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
        AppManager.Instance.ConfigManager.TryWriteRecord();
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
        AppManager.Instance.ConfigManager.TryWriteRecord();
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
        AppManager.Instance.ConfigManager.TryWriteRecord();
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
        CharacterEntry characterEntry = AppManager.Instance.ConfigManager.CharacterTabControl.GetSelectedCharacterProfile().GetEntry();
        return constraint.Descriptor.Contains(pack.Entry) &&
               profile.PackIsGenerallyUnlocked(characterEntry, pack.Entry, constraint.SlotIndex);
    }

    public bool PackIsGenerallyUnlocked(PackEntry pack)
    {
        CharacterEntry characterEntry = AppManager.Instance.ConfigManager.CharacterTabControl.GetSelectedCharacterProfile().GetEntry();
        return AppManager.Instance.ProfileManager.GetCurrProfile().PackIsGenerallyUnlocked(characterEntry, pack);
    }

    public Description GetPackUnlockCondition(PackEntry pack)
    {
        CharacterEntry characterEntry = AppManager.Instance.ConfigManager.CharacterTabControl.GetSelectedCharacterProfile().GetEntry();
        return AppManager.Instance.ProfileManager.GetCurrProfile().GetPackUnlockCondition(characterEntry, pack);
    }

    public bool ConstraintIsUnlocked(PackConstraint constraint)
    {
        CharacterEntry characterEntry = AppManager.Instance.ConfigManager.CharacterTabControl.GetSelectedCharacterProfile().GetEntry();
        return AppManager.Instance.ProfileManager.GetCurrProfile().SlotIsUnlocked(characterEntry, constraint.SlotIndex);
    }

    public Description GetConstraintUnlockCondition(PackConstraint constraint)
    {
        CharacterEntry characterEntry = AppManager.Instance.ConfigManager.CharacterTabControl.GetSelectedCharacterProfile().GetEntry();
        return AppManager.Instance.ProfileManager.GetCurrProfile().GetConstraintUnlockCondition(characterEntry, constraint.SlotIndex);
    }

    public List<PackEntry> GetEquippedPacks()
    {
        Assert.IsTrue(_packConstraints.All(c => c.Pack != null));
        return _packConstraints.Map(c => c.Pack.Entry).ToList();
    }

    public void SelectDefaultPackPresetFromCharacter(CharacterProfile character)
    {
        PackPreset preset = character.GetEntry().PackPreset;
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

    public override void WriteRecord()
    {
        _recordedPackPreset = WriteCurrentIntoPackPreset();
    }

    public override void ReadRecord()
    {
        LoadPackPreset(_recordedPackPreset);
    }

    public override bool IsValid()
    {
        PackConstraint firstInvalid = _packConstraints.First(constraint => constraint.IsEmpty || !constraint.Descriptor.Contains(constraint.Pack.Entry));
        return firstInvalid == null;
    }
}