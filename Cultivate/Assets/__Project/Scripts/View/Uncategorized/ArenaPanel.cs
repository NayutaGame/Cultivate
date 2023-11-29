
using System;
using TMPro;
using UnityEngine.EventSystems;

public class ArenaPanel : Panel
{
    public SkillInventoryView SkillInventoryView;
    public ArenaEditorView ArenaEditorView;
    public ArenaScoreboardView ArenaScoreboardView;
    public TMP_Text ReportView;

    private InteractHandler _interactHandler;

    private Address _address;

    public override void Configure()
    {
        base.Configure();

        _address = new Address("Run.Arena");

        ConfigureInteractDelegate();

        SkillInventoryView.SetAddress(new Address("App.SkillInventory"));
        SkillInventoryView.SetHandler(_interactHandler);

        ArenaEditorView.SetAddress(_address);
        ArenaEditorView.SetHandler(_interactHandler);

        ArenaScoreboardView.Configure();
    }

    private void ConfigureInteractDelegate()
    {
        _interactHandler = new(2,
            getId: view =>
            {
                object item = view.GetComponent<IAddress>().Get<object>();
                if (item is RunSkill)
                    return 0;
                if (item is SkillSlot)
                    return 1;
                return null;
            },
            dragDropTable: new Action<InteractDelegate, InteractDelegate>[]
            {
                /*               RunSkill,   SkillSlot */
                /* RunSkill   */ null,       TryWrite,
                /* SkillSlot  */ null,       TryWrite,
            });
        _interactHandler.SetHandle(InteractHandler.POINTER_RIGHT_CLICK, 1, (v, d) => TryIncreaseJingJie(v, d));
    }

    public override void Refresh()
    {
        base.Refresh();
        SkillInventoryView.Refresh();
        ArenaEditorView.Refresh();
        ArenaScoreboardView.Refresh();

        ReportView.text = RunManager.Instance.Arena.Result?.ToString();
    }

    private void TryWrite(InteractDelegate from, InteractDelegate to)
    {
        Arena arena = _address.Get<Arena>();

        object fromItem = from.GetComponent<IAddress>().Get<object>();
        SkillSlot toSlot = to.GetComponent<IAddress>().Get<SkillSlot>();

        if (fromItem is RunSkill fromSkill)
        {
            arena.TryWrite(fromSkill, toSlot);
        }
        else if (fromItem is SkillSlot fromSlot)
        {
            arena.TryWrite(fromSlot, toSlot);
        }
    }

    private bool TryIncreaseJingJie(InteractDelegate view, PointerEventData eventData)
    {
        Arena arena = _address.Get<Arena>();
        SkillSlot slot = view.GetComponent<IAddress>().Get<SkillSlot>();
        return arena.TryIncreaseJingJie(slot);
    }
}
