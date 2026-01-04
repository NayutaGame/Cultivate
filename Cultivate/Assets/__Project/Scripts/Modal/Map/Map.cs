
using System;
using System.Collections.Generic;
using System.Linq;
using CLLibrary;
using UnityEngine;

[Serializable]
public class Map : Addressable, ISerializationCallbackReceiver
{
    public static int[,] StepCount = new int[6, 2]
    {
        { 5, 3 },
        { 7, 4 },
        { 9, 5 },
        { 11, 6 },
        { 13, 7 },
        { 13, 7 },
    };

    public void InitNeurons()
    {
        JingJieChangedNeuron = new();
        LevelChangedNeuron = new();
        RoomChangedNeuron = new();
        CellChangedNeuron = new();
    }
    
    public Neuron<JingJieChangedDetails> JingJieChangedNeuron;
    public Neuron LevelChangedNeuron;
    public Neuron<RoomChangedDetails> RoomChangedNeuron;
    public Neuron<CellChangedDetails> CellChangedNeuron;
    
    private MapNodeListModel _mapNodes;
    [SerializeField] private JingJie _jingJie;
    public RoomEnvironment _room;
    [NonSerialized] private ICellAdapter _cell;
    private MapState _mapState;
    [SerializeField] private int _availableStepCount;
    [SerializeField] private int _totalStepCount;
    [SerializeField] private int _totalChoiceCount;

    public string GetStepText()
        => $"{_availableStepCount} / {_totalStepCount}";
    
    private static readonly Dictionary<string, Func<object, object>> Accessor = new()
    {
        { "MapNodes",                   thisObject => ((Map)thisObject)._mapNodes },
        // { "CurrLevel",                  thisObject => ((Map)thisObject).GetCurrLevel() },
    };
    public object Get(string s) => Accessor[s](this);
    public Map()
    {
        InitNeurons();
        _jingJie = JingJie.LianQi;
        _mapNodes = new();
    }

    #region Accessors
    
    public JingJie JingJie => _jingJie;

    public ICellAdapter Cell
    {
        get => _cell;
        private set
        {
            if (_cell == value)
                return;
            CellChangedDetails cellChangedDetails = new(_cell, value);
        
            if (_cell != null)
                RunManager.Instance.Environment.SendEvent(RunClosureDict.WIL_CHANGE_CELL, cellChangedDetails);
            _cell?.Exit();
            _cell = value;
            _cell?.Enter();
            CellChangedNeuron.Invoke(cellChangedDetails);
        }
    }

    public bool IsFinalJingJie()
        => _jingJie == RunManager.Instance.Environment.GetRunConfig().DifficultyProfile.GetEntry().FinalJingJie;

    public bool IsLastSelecting() => _mapState == MapState.LastSelecting;
    
    public int GetIndexOfMapNode(MapNode mapNode)
        => _mapNodes.IndexOf(mapNode);

    public static int GetTotalChoiceCountFromJingJie(JingJie jingJie)
        => StepCount[jingJie.GetIndex(), 0];

    public static int GetAvailableStepCountFromJingJie(JingJie jingJie)
        => StepCount[jingJie.GetIndex(), 1];
    
    private void SelectedMapNodeWithRoomEntry(MapNode mapNode, RoomEntry roomEntry)
    {
        ReceiveSignalProcedure(SelectedMapNodeSignal.FromMapNodeAndRoomEntry(mapNode, roomEntry));
    }

    private bool IsOverHalf(int maxStep, int restStep)
    {
        // 6, 6, 0 -> false
        // 6, 5, 1 -> false
        // 6, 4, 2 -> false
        // 6, 3, 3 -> true
        // 6, 2, 4 -> true
        // 6, 1, 5 -> true
        
        // 5, 5, 0 -> false
        // 5, 4, 1 -> false
        // 5, 3, 2 -> true
        // 5, 2, 3 -> true
        // 5, 1, 4 -> true
        
        return maxStep / 2 <= maxStep - restStep;
    }

    private int CalcLadder(JingJie jingJie, bool isLastRoom)
    {
        int maxStep = GetAvailableStepCountFromJingJie(jingJie);
        int restStep = _availableStepCount + 1;
        bool isOverHalf = IsOverHalf(maxStep, restStep);
        
        int lastRoomBonus = isLastRoom ? 1 : 0;
        int overHalfBonus = isOverHalf ? 1 : 0;
        switch (jingJie.GetIndex())
        {
            case 0: // 0 1
                return 0 + lastRoomBonus;
            case 1: // 2 3 4
                return 2 + overHalfBonus + lastRoomBonus;
            case 2: // 5 6 7
                return 5 + overHalfBonus + lastRoomBonus;
            case 3: // 8 9 10
                return 8 + overHalfBonus + lastRoomBonus;
            case 4: // 11 12 13
                return 11 + overHalfBonus + lastRoomBonus;
            case 5: // 14
                return 14;
        }

        throw new Exception("CL:Unexpected pathway");
    }

    #endregion

    #region Procedures
    
    public void SetJingJieProcedure(JingJie toJingJie)
        => SetJingJieProcedure(new JingJieChangedDetails(JingJie, toJingJie));
    
    public void SetJingJieProcedure(JingJieChangedDetails d)
    {
        RunManager.Instance.Environment.SendEvent(RunClosureDict.WIL_JINGJIE_CHANGE, d);
        if (d.Cancel)
            return;

        _jingJie = d.ToJingJie;
        
        RunManager.Instance.Environment.Home.SetJingJie(d.ToJingJie);

        RunManager.Instance.Environment.SendEvent(RunClosureDict.DID_JINGJIE_CHANGE, d);
        JingJieChangedNeuron.Invoke(d);
        
        AudioManager.Play(d.ToJingJie.GetAudio());
    }
    
    public void ResetMapProgressProcedure()
    {
        int totalChoiceCount = GetTotalChoiceCountFromJingJie(JingJie);
        int availableStepCount = GetAvailableStepCountFromJingJie(JingJie);
        ResetMapProgressProcedure(totalChoiceCount, availableStepCount);
    }
    
    public void ResetMapProgressProcedure(int totalChoiceCount, int availableStepCount)
    {
        _mapState = MapState.Selecting;
        
        _totalChoiceCount = totalChoiceCount;
        _totalStepCount = availableStepCount;
        _availableStepCount = availableStepCount;

        int[] indices = Numeric.GetCombination(_mapNodes.Count(), _totalChoiceCount);
        _mapNodes.Do(mapNode => mapNode.IsAccessible = false);
        foreach (int index in indices)
            _mapNodes[index].IsAccessible = true;
    }

    private RoomEntry CalcLastRoomEntryFromJingJie(JingJie jingJie)
    {
        return null;
    }
    
    public void ReceiveSignalProcedure(Signal signal)
    {
        switch (_mapState)
        {
            case MapState.Room:
                _room.ReceiveSignal(signal);
                if (_mapState == MapState.Committed)
                    return;

                if (!_room.IsFinished())
                {
                    Cell = _room.CurrentCell;
                    return;
                }
            
                ExitRoomProcedure();
                return;
            case MapState.Selecting:
                if (signal is SelectedMapNodeSignal selectedMapNodeSignal)
                {
                    _availableStepCount -= 1;
                    EnterRoomProcedure(selectedMapNodeSignal.MapNode, selectedMapNodeSignal.RoomEntry, CalcLadder(JingJie, false));
                }
                return;
            case MapState.LastRoom:
                _room.ReceiveSignal(signal);
                if (_mapState == MapState.Committed)
                    return;

                if (!_room.IsFinished())
                {
                    Cell = _room.CurrentCell;
                    return;
                }

                ExitLastRoomProcedure();
                return;
            case MapState.LastSelecting:
                if (signal is SelectLastRoomSignal selectLastRoomSignal)
                {
                    RoomEntry roomEntry = CalcLastRoomEntryFromJingJie(JingJie);
                    EnterLastRoomProcedure(roomEntry, CalcLadder(JingJie, true));
                }
                return;
            case MapState.Committed:
                return;
        }
    }

    public void EnterRoomProcedure(MapNode mapNode, RoomEntry roomEntry, int ladder)
    {
        if (_availableStepCount == 0)
            _mapNodes.Do(n => n.IsAccessible = false);
        
        if (mapNode != null)
            mapNode.IsAccessible = false;
        
        _mapState = MapState.Room;
        _room = RoomEnvironment.CreateRoom(mapNode, roomEntry, ladder);
        _room.Step();
        Cell = _room.CurrentCell;
        
        RoomChangedNeuron.Invoke(new(_room));
    }

    public void EnterLastRoomProcedure(RoomEntry roomEntry, int ladder)
    {
        _mapState = MapState.LastRoom;
        _room = RoomEnvironment.CreateRoom(roomEntry, ladder);
        _room.Step();
        Cell = _room.CurrentCell;
        
        RoomChangedNeuron.Invoke(new(_room));
    }

    public void ExitRoomProcedure()
    {
        if (_availableStepCount == 0)
            _mapState = MapState.LastSelecting;
        else
            _mapState = MapState.Selecting;
        _room = null;
        Cell = null;
        
        RoomChangedNeuron.Invoke(new(_room));
    }

    public void ExitLastRoomProcedure()
    {
        // bool isFinalJingJie = IsFinalJingJie();
        bool shouldCommit = false;
        if (shouldCommit)
        {
            CommitRunProcedure(RunResult.RunOutcome.Victorious);
            return;
        }
        
        SetJingJieProcedure(JingJie + 1);
        ResetMapProgressProcedure();
        
        if (_availableStepCount == 0)
            _mapState = MapState.LastSelecting;
        else
            _mapState = MapState.Selecting;
        _room = null;
        Cell = null;
        
        RoomChangedNeuron.Invoke(new(_room));
    }

    public void GuideProcedure(SkillMovedDetails d)
        => GuideProcedure(new DeckChangedSignal(d.FromIndex.Reify(), d.ToIndex.Reify()));

    public void GuideProcedure(Signal signal)
    {
        // TODO
        // Guide guide = _cell.GetGuideDescriptor();
        // guide?.ReceiveSignal(_cell, signal);
        // if (guide != null)
        //     CanvasManager.Instance.RefreshGuide();
    }

    public void CommitRunProcedure(RunResult.RunOutcome state)
    {
        if (_mapState == MapState.Committed)
            return;

        _mapState = MapState.Committed;
        RunManager.Instance.Environment._result.SetOutcome(state);
        RunManager.Instance.Environment._runFinishedTime = RunManager.Instance.Environment.GetPassedTime();

        RunManager.Instance.Environment.SendEvent(RunClosureDict.DID_COMMIT_RUN, new RunCommitDetails(RunManager.Instance.Environment));
        
        RunResultCell resultPanel = new RunResultCell(RunManager.Instance.Environment);
        Cell = resultPanel;
    }

    public void InitPanelFromCreation()
    {
        SetJingJieProcedure(JingJie.LianQi);
    }

    public void InitPanelFromLoad()
    {
        SetJingJieProcedure(_jingJie);
        // serialize room environment
        // and recover data from there
        
        // RoomChangedNeuron.Invoke(new(null, newRoom));
    }

    public MenuDetails GetMenuDetailsFromMapNode(MapNode mapNode)
    {
        List<RoomOption> roomOptions = mapNode.GetRoomOptions();

        List<MenuOption> menuOptions = new List<MenuOption>();
        roomOptions.Do(roomOption =>
        {
            if (roomOption.RoomEntry == null)
                return;

            MenuOption menuOption = new(roomOption.Description, ClickAction);
            menuOptions.Add(menuOption);
            return;
            
            void ClickAction() => SelectedMapNodeWithRoomEntry(mapNode, roomOption.RoomEntry);
        });
        
        return new MenuDetails(menuOptions);
    }

    #endregion

    
    public void OnBeforeSerialize() { }

    public void OnAfterDeserialize()
    {
        InitNeurons();
        _jingJie = string.IsNullOrEmpty(_jingJie.GetId()) ? null : Encyclopedia.JingJieCategory.FromId(_jingJie.GetId());
    }
}
