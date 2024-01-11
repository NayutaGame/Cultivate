
using CLLibrary;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ArenaPanel : Panel
{
    public ListView SkillInventoryView;
    public ListView ArenaEditorView;
    public ArenaScoreboardView ArenaScoreboardView;
    public TMP_Text ReportView;

    public Button[] SortButtons;

    public Button RandomButton;
    public Button CompeteButton;

    public override void Configure()
    {
        base.Configure();

        ConfigureInteractDelegate();

        SkillInventoryView.SetAddress(new Address("App.SkillInventory"));

        ArenaEditorView.SetAddress(new Address("Run.Arena"));

        ArenaScoreboardView.Configure();

        RandomButton.onClick.RemoveAllListeners();
        RandomButton.onClick.AddListener(Random);

        CompeteButton.onClick.RemoveAllListeners();
        CompeteButton.onClick.AddListener(Compete);

        SortButtons.Length.Do(i =>
        {
            int comparisonId = i;
            SortButtons[i].onClick.RemoveAllListeners();
            SortButtons[i].onClick.AddListener(() => SortByComparisonId(comparisonId));
        });
    }

    private void ConfigureInteractDelegate()
    {
        // _interactHandler = new(2,
        //     getId: view =>
        //     {
        //         object item = view.GetComponent<LegacyAddressBehaviour>().Get<object>();
        //         if (item is RunSkill)
        //             return 0;
        //         if (item is SkillSlot)
        //             return 1;
        //         return null;
        //     },
        //     dragDropTable: new Action<InteractBehaviour, InteractBehaviour, PointerEventData>[]
        //     {
        //         /*               RunSkill,   SkillSlot */
        //         /* RunSkill   */ null,       TryWrite,
        //         /* SkillSlot  */ null,       TryWrite,
        //     });
        // _interactHandler.SetHandle(InteractHandler.POINTER_RIGHT_CLICK, 1, (ib, d) => TryIncreaseJingJie(ib, d));
    }

    public override void Refresh()
    {
        base.Refresh();
        SkillInventoryView.Refresh();
        ArenaEditorView.Refresh();
        ArenaScoreboardView.Refresh();

        ReportView.text = RunManager.Instance.Arena.Result?.ToString();
    }

    private void TryWrite(InteractBehaviour from, InteractBehaviour to, PointerEventData eventData)
    {
        Arena arena = new Address("Run.Arena").Get<Arena>();

        object fromItem = from.GetComponent<SimpleView>().Get<object>();
        SkillSlot toSlot = to.GetComponent<SimpleView>().Get<SkillSlot>();

        if (fromItem is RunSkill fromSkill)
        {
            arena.TryWrite(fromSkill, toSlot);
        }
        else if (fromItem is SkillSlot fromSlot)
        {
            arena.TryWrite(fromSlot, toSlot);
        }
    }

    private bool TryIncreaseJingJie(InteractBehaviour ib, PointerEventData eventData)
    {
        Arena arena = new Address("Run.Arena").Get<Arena>();
        SkillSlot slot = ib.GetComponent<SimpleView>().Get<SkillSlot>();
        return arena.TryIncreaseJingJie(slot);
    }

    private void Random()
    {
        // ActivePool.Do(v => ((MutableEntityView)v).RandomButton.onClick.Invoke());
        // multiple refreshes
    }

    private void Compete()
    {
        Arena arena = new Address("Run.Arena").Get<Arena>();
        arena.Compete();
        CanvasManager.Instance.RunCanvas.Refresh();
    }

    private void SortByComparisonId(int i)
    {
        SkillInventory inventory = new Address("App.SkillInventory").Get<SkillInventory>();
        inventory.SortByComparisonId(i);
        CanvasManager.Instance.RunCanvas.Refresh();
    }
}
