
using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class MMDMLayer : MonoBehaviour
{
    public MapPanel MapPanel;
    public MaskView MaskAboveDeck;
    public DeckPanel DeckPanel;
    public MaskView MaskUnderDeck;

    public Button MapToggle;

    private Panel _currentPanel;

    /*  N   U   L   L
     *          Deck
     *  Map MaskDeck
     *          DeckMask
     *  Map         Mask
     */
    public enum MMDMState
    {
        N = 0,
        D = 1,
        MMD = 2,
        DM = 3,
        MM = 4,
    }

    private Func<Tween>[] TransitionTable;
    private MMDMState _state;
    public MMDMState State => _state;
    public Tween SetState(MMDMState state)
    {
        int index = 5 * (int)_state + (int)state;
        _state = state;
        DeckPanel.SetLocked(_state == MMDMState.D || _state == MMDMState.MMD);
        Refresh();
        return TransitionTable[index]?.Invoke() ?? DOTween.Sequence().SetAutoKill();
    }

    public void Configure()
    {
        TransitionTable = new Func<Tween>[]
        {
            null, NToD, null, NToDM, NToMM,
            DToN, null, DToMMD, null, DToMM,
            null, MMDToD, null, null, null,
            DMToN, null, null, null, DMToMM,
            MMToN, MMToD, null, MMToDM, null,
        };

        _state = MMDMState.N;

        MapPanel.SetAddress(new Address("Run.Environment.Map"));
        DeckPanel.Configure();

        DeckPanel.ToggleButton.onClick.RemoveAllListeners();
        DeckPanel.ToggleButton.onClick.AddListener(ToggleDeck);

        MaskAboveDeck.GetComponent<Button>().onClick.RemoveAllListeners();
        MaskAboveDeck.GetComponent<Button>().onClick.AddListener(Close);

        MaskUnderDeck.GetComponent<Button>().onClick.RemoveAllListeners();
        MaskUnderDeck.GetComponent<Button>().onClick.AddListener(Close);

        MapToggle.onClick.RemoveAllListeners();
        MapToggle.onClick.AddListener(ToggleMap);
    }

    public void Close()
    {
        switch (_state)
        {
            case MMDMState.N:
                break;
            case MMDMState.D:
                break;
            case MMDMState.MMD:
                SetState(MMDMState.D).Restart();
                break;
            case MMDMState.DM:
                SetState(MMDMState.N).Restart();
                break;
            case MMDMState.MM:
                SetState(MMDMState.N).Restart();
                break;
        }
    }

    public void ToggleMap()
    {
        switch (_state)
        {
            case MMDMState.N:
                SetState(MMDMState.MM).Restart();
                break;
            case MMDMState.D:
                SetState(MMDMState.MMD).Restart();
                break;
            case MMDMState.MMD:
                SetState(MMDMState.D).Restart();
                break;
            case MMDMState.DM:
                SetState(MMDMState.MM).Restart();
                break;
            case MMDMState.MM:
                SetState(MMDMState.N).Restart();
                break;
        }
    }

    public void ToggleDeck()
    {
        switch (_state)
        {
            case MMDMState.N:
                SetState(MMDMState.DM).Restart();
                break;
            case MMDMState.D:
                break;
            case MMDMState.MMD:
                break;
            case MMDMState.DM:
                SetState(MMDMState.N).Restart();
                break;
            case MMDMState.MM:
                SetState(MMDMState.DM).Restart();
                break;
        }
    }

    public void Refresh()
    {
        switch (_state)
        {
            case MMDMState.N:
                break;
            case MMDMState.D:
                DeckPanel.Refresh();
                break;
            case MMDMState.MMD:
                MapPanel.Refresh();
                break;
            case MMDMState.DM:
                DeckPanel.Refresh();
                break;
            case MMDMState.MM:
                MapPanel.Refresh();
                break;
        }
    }

    private Tween NToD()
        => DeckPanel.GetShowTween();

    private Tween NToDM()
        => DOTween.Sequence().SetAutoKill()
            .Join(DeckPanel.GetShowTween())
            .Join(MaskUnderDeck.GetShowTween());

    private Tween NToMM()
        => DOTween.Sequence().SetAutoKill()
            .Join(MapPanel.GetShowTween())
            .Join(MaskUnderDeck.GetShowTween());

    private Tween DToN()
        => DeckPanel.GetHideTween();

    private Tween DToMMD()
        => DOTween.Sequence().SetAutoKill()
            .Join(MapPanel.GetShowTween())
            .Join(MaskAboveDeck.GetShowTween());

    private Tween DToMM()
        => DOTween.Sequence().SetAutoKill()
            .Join(DeckPanel.GetHideTween())
            .AppendInterval(0.25f)
            .Join(MapPanel.GetShowTween())
            .Join(MaskUnderDeck.GetShowTween());

    private Tween MMDToD()
        => DOTween.Sequence().SetAutoKill()
            .Join(MapPanel.GetHideTween())
            .Join(MaskAboveDeck.GetHideTween());

    private Tween DMToN()
        => DOTween.Sequence().SetAutoKill()
            .Join(DeckPanel.GetHideTween())
            .Join(MaskUnderDeck.GetHideTween());

    private Tween DMToMM()
        => DOTween.Sequence().SetAutoKill()
            .Join(DeckPanel.GetHideTween())
            .AppendInterval(0.25f)
            .Join(MapPanel.GetShowTween());

    private Tween MMToN()
        => DOTween.Sequence().SetAutoKill()
            .Join(MapPanel.GetHideTween())
            .Join(MaskUnderDeck.GetHideTween());

    private Tween MMToD()
        => DOTween.Sequence().SetAutoKill()
            .Join(MapPanel.GetHideTween())
            .AppendInterval(0.25f)
            .Join(MaskUnderDeck.GetHideTween())
            .Join(DeckPanel.GetShowTween());

    private Tween MMToDM()
        => DOTween.Sequence().SetAutoKill()
            .Join(MapPanel.GetHideTween())
            .AppendInterval(0.25f)
            .Join(DeckPanel.GetShowTween());
}
