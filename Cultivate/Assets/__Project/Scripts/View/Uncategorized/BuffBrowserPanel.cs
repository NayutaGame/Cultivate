
using UnityEngine;
using UnityEngine.UI;

public class BuffBrowserPanel : Panel
{
    private Address _address;

    public ListView BuffList;

    [SerializeField] private Button ReturnButton;

    public override void AwakeFunction()
    {
        base.AwakeFunction();

        _address = new Address("App.Encyclopedia.BuffCategory.List");
        BuffList.SetAddress(_address);

        ReturnButton.onClick.RemoveAllListeners();
        ReturnButton.onClick.AddListener(Hide);
    }

    public override void Refresh()
    {
        BuffList.Refresh();
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
    
    private void OnEnable()
    {
        AppManager.Instance.PushEscFunc(Hide);
    }

    private void OnDisable()
    {
        AppManager.Instance.PopEscFunc();
    }
}