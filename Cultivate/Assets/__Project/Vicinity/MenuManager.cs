
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuManager : XView, Addressable
{
    [SerializeField] private XView Background;
    [SerializeField] private MenuView MenuView;

    [SerializeField] private RectTransform ParentFitterRect;
    [SerializeField] private RectTransform ChildFitterRect;

    private MenuDetails _menuDetails;

    private static readonly Dictionary<string, Func<object, object>> Accessor = new()
    {
        { "MenuDetails",            thisObject => ((MenuManager)thisObject)._menuDetails },
    };
    public object Get(string s) => Accessor[s](this);
    protected override void AwakeFunction()
    {
        base.AwakeFunction();
        
        InteractBehaviour backgroundIb = Background.GetInteractBehaviour();
        backgroundIb.LeftClickNeuron.Join(HideMenuWithBackgroundClick);
        MenuView.CheckAwake();
    }

    public void CreateMenu(MenuDetails menuDetails)
    {
        _menuDetails = menuDetails;
        
        MenuView.SetAddress("Canvas.MenuManager.MenuDetails");
        MenuView.gameObject.SetActive(true);
        Background.gameObject.SetActive(true);
        
        Canvas.ForceUpdateCanvases();
        ForceRebuildContentSizeFitters();
        Align();
    }
    
    private void ForceRebuildContentSizeFitters()
    {
        LayoutRebuilder.ForceRebuildLayoutImmediate(ChildFitterRect);
        LayoutRebuilder.ForceRebuildLayoutImmediate(ParentFitterRect);
    }

    public void HideMenu()
    {
        MenuView.gameObject.SetActive(false);
        Background.gameObject.SetActive(false);
    }

    private void HideMenuWithBackgroundClick(InteractBehaviour ib, PointerEventData d)
    {
        HideMenu();
    }

    private void Align()
    {
        Vector2 mouseScreenPosition = Input.mousePosition;
        Vector3 targetPosition = CameraManager.UI2World(mouseScreenPosition);
        targetPosition = CameraManager.Instance.ClampToScreenBounds(MenuView.GetRect(), targetPosition);
        
        MenuView.GetRect().position = targetPosition;
        MenuView.ListView.RefreshPivots();
    }
}