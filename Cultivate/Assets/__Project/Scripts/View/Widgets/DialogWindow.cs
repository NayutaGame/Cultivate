
using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class DialogWindow : MonoBehaviour
{
    [SerializeField] private TMP_Text Description;
    [SerializeField] private CLButtonPatternA Confirm;

    private Action _onConfirm;

    private void Awake()
    {
        Confirm.CheckAwake();
        Confirm.LeftClickNeuron.Join(CloseDialog);
    }

    public void ShowDialog(string message, Action onConfirm)
    {
        Description.text = message;
        _onConfirm = onConfirm;
    }

    private void CloseDialog(InteractBehaviour ib, PointerEventData d)
    {
        _onConfirm?.Invoke();
        _onConfirm = null;
        
        gameObject.SetActive(false);
    }
}
