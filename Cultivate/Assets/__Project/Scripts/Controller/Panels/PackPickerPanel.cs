
using System;
using CLLibrary;
using UnityEngine;

public class PackPickerPanel : Panel
{
    public Neuron<Predicate<ConfigPack>> HighlightPacksNeuron = new();
    public Neuron UnhighlightPacksNeuron = new();
    public Neuron<Predicate<PackConstraint>> HighlightConstraintsNeuron = new();
    public Neuron UnhighlightConstraintsNeuron = new();
    
    [SerializeField] private ListView ConstraintListView;
    [SerializeField] private ListView SelectionListView;
    
    public override void AwakeFunction()
    {
        base.AwakeFunction();
        ConstraintListView.SetAddress(new Address("Config.PackTabControl.PackConstraints"));
        ConstraintListView.SetPrefabProvider(model =>
        {
            PackConstraint packConstraint = model as PackConstraint;
            return packConstraint.SlotIndex <= 5 ? packConstraint.SlotIndex : 5;
        });
        
        SelectionListView.SetAddress(new Address("Config.PackTabControl.PackSelections"));
    }

    public override void Refresh()
    {
        ConstraintListView.Refresh();
        SelectionListView.Refresh();
    }

    private void OnEnable()
    {
        AppManager.Instance.ConfigManager.PackTabControl.EquipPackNeuron.Add(EquipPackStaging);
        AppManager.Instance.ConfigManager.PackTabControl.UnequipPackNeuron.Add(UnequipPackStaging);
        Refresh();
        
        ConstraintListView.ForceLayoutRebuild();
        ConstraintListView.RefreshPivots();
        
        SelectionListView.RefreshPivots();
    }

    private void OnDisable()
    {
        AppManager.Instance.ConfigManager.PackTabControl.EquipPackNeuron.Remove(EquipPackStaging);
        AppManager.Instance.ConfigManager.PackTabControl.UnequipPackNeuron.Remove(UnequipPackStaging);
    }
    
    private void EquipPackStaging(PackEquipDetails d)
    {
        void SetPosition(SlotView view, XView otherView)
        {
            view.GetAnimator().SetState(3);
            view.GetContentView().GetRect().position = otherView.GetRect().position;
            view.GetContentView().GetRect().localScale = otherView.GetRect().localScale;
        }

        void SetIdle(SlotView view)
        {
            view.GetAnimator().SetStateAsync(SlotView.IDLE);
            // AudioManager.Play("CardPlacement");
        }
        
        // ConstraintListView 设定动画
        SlotView constraintView = ConstraintListView.ViewFromIndex(d.ConstraintIndex);
        SlotView selectionView = SelectionListView.ViewFromIndex(d.SelectionIndex);
        
        constraintView.Refresh();
        selectionView.Refresh();
        
        SetPosition(constraintView, selectionView.GetContentView());
        constraintView.GetAnimator().SetStateAsync(SlotView.IDLE);
        selectionView.GetAnimator().SetState(SlotView.IDLE);
        
        // SelectionListView 归零 变暗
    }

    private void UnequipPackStaging(PackUnequipDetails d)
    {
        void SetPosition(SlotView view, XView otherView)
        {
            view.GetAnimator().SetState(3);
            view.GetContentView().GetRect().position = otherView.GetRect().position;
            view.GetContentView().GetRect().localScale = otherView.GetRect().localScale;
        }

        void SetIdle(SlotView view)
        {
            view.GetAnimator().SetStateAsync(SlotView.IDLE);
            // AudioManager.Play("CardPlacement");
        }
        
        // ConstraintListView 设定动画
        SlotView constraintView = ConstraintListView.ViewFromIndex(d.ConstraintIndex);
        SlotView selectionView = SelectionListView.ViewFromIndex(d.SelectionIndex);
        
        constraintView.Refresh();
        selectionView.Refresh();
        
        SetPosition(selectionView, constraintView.GetContentView());
        selectionView.GetAnimator().SetStateAsync(1);
        constraintView.GetAnimator().SetState(1);
    }
}
