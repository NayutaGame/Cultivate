
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TopBar : MonoBehaviour
{
    public TMP_Text MingYuanText;
    public TMP_Text GoldText;
    public TMP_Text HealthText;
    public TMP_Text JingJieText;

    [SerializeField] private PropagatePointer MingYuanIcon;
    [SerializeField] private PropagatePointer MingYuanHintIcon;
    [SerializeField] private PropagatePointer HealthIcon;
    [SerializeField] private PropagatePointer GoldIcon;

    public Button MenuButton;

    public void Configure()
    {
        MenuButton.onClick.RemoveAllListeners();
        MenuButton.onClick.AddListener(OpenMenu);

        ConfigurePropagators();
    }

    private void OnEnable()
    {
        if (RunManager.Instance != null)
            RunManager.Instance.Environment.ResourceChangedEvent += Refresh;
    }

    private void OnDisable()
    {
        if (RunManager.Instance != null)
            RunManager.Instance.Environment.ResourceChangedEvent -= Refresh;
    }

    private void ConfigurePropagators()
    {
        MingYuanIcon._onPointerEnter = PointerEnterMingYuan;
        MingYuanIcon._onPointerExit = PointerExit;
        MingYuanIcon._onPointerMove = PointerMove;
        MingYuanHintIcon._onPointerEnter = PointerEnterMingYuanHint;
        MingYuanHintIcon._onPointerExit = PointerExit;
        MingYuanHintIcon._onPointerMove = PointerMove;
        HealthIcon._onPointerEnter = PointerEnterHealth;
        HealthIcon._onPointerExit = PointerExit;
        HealthIcon._onPointerMove = PointerMove;
        GoldIcon._onPointerEnter = PointerEnterGold;
        GoldIcon._onPointerExit = PointerExit;
        GoldIcon._onPointerMove = PointerMove;
    }

    private void PointerEnterMingYuan(PointerEventData eventData)
    {
        if (eventData.dragging) return;
        CanvasManager.Instance.TextHint.SetText("命元\n归零游戏失败");
    }

    private void PointerEnterMingYuanHint(PointerEventData eventData)
    {
        if (eventData.dragging) return;
        CanvasManager.Instance.TextHint.SetText(RunManager.Instance.Environment.Home.MingYuan.GetMingYuanPenaltyText());
    }

    private void PointerEnterHealth(PointerEventData eventData)
    {
        if (eventData.dragging) return;
        CanvasManager.Instance.TextHint.SetText("生命值上限");
    }

    private void PointerEnterGold(PointerEventData eventData)
    {
        if (eventData.dragging) return;
        CanvasManager.Instance.TextHint.SetText("金钱");
    }

    private void PointerExit(PointerEventData eventData)
    {
        if (eventData.dragging) return;
        CanvasManager.Instance.TextHint.SetText(null);
    }

    private void PointerMove(PointerEventData eventData)
    {
        if (eventData.dragging) return;
        CanvasManager.Instance.TextHint.UpdateMousePos(eventData.position);
    }

    public void Refresh()
    {
        EntityModel entity = RunManager.Instance.Environment.Home;

        MingYuanText.text = RunManager.Instance.Environment.GetMingYuan().ToString();
        GoldText.text = RunManager.Instance.Environment.Gold.ToString();
        HealthText.text = entity.GetFinalHealth().ToString();
        JingJieText.text = $"{entity.GetJingJie().ToString()}期";
    }

    private void OpenMenu()
    {
        AppManager.Push(new MenuAppS());
    }
}
