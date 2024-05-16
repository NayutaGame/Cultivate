
using System.Threading.Tasks;
using CLLibrary;

/// <summary>
/// Buff
/// </summary>
public class Buff : StageEventListener
{
    private StageEntity _owner;
    public StageEntity Owner => _owner;

    private BuffEntry _entry;
    public BuffEntry GetEntry() => _entry;

    public Neuron StackChangedNeuron;
    public Neuron PingNeuron;

    public string GetName() => _entry.GetName();
    public string GetExplanation() => _entry.GetExplanation();
    public string GetTrivia() => _entry.GetTrivia();

    private int _stack;
    public int Stack => _stack;
    public async Task SetStack(int stack)
    {
        await _eventDict.SendEvent(StageEventDict.WIL_CHANGE_STACK, new BuffStackChangeDetails(_stack, stack));
        _stack = stack;
        await _eventDict.SendEvent(StageEventDict.DID_CHANGE_STACK, new BuffStackChangeDetails(_stack, stack));

        if(_stack <= 0)
            await _owner.RemoveBuff(this);
        else
            StackChangedNeuron.Invoke();
            
    }

    public async Task SetDStack(int dStack)
        => await SetStack(_stack + dStack);

    /// <summary>
    /// 是否有益
    /// </summary>
    public bool Friendly => _entry.Friendly;

    /// <summary>
    /// 是否可驱散
    /// </summary>
    public bool Dispellable => _entry.Dispellable;

    public StageEventDict _eventDict;

    public Buff(StageEntity owner, BuffEntry entry)
    {
        _owner = owner;
        _entry = entry;
        _stack = 0;

        _eventDict = new();
        PingNeuron = new();
        StackChangedNeuron = new();
    }

    public async void PlayPingAnimation()
    {
        PingNeuron.Invoke();
    }

    public void Register()
    {
        foreach (int eventId in _entry._eventDescriptorDict.Keys)
        {
            StageEventDescriptor eventDescriptor = _entry._eventDescriptorDict[eventId];
            int senderId = eventDescriptor.ListenerId;

            if (senderId == StageEventDict.STAGE_ENVIRONMENT)
                _owner.Env.EventDict.Register(this, eventDescriptor);
            else if (senderId == StageEventDict.STAGE_ENTITY)
                ;
            else if (senderId == StageEventDict.STAGE_BUFF)
                ;
            else if (senderId == StageEventDict.STAGE_FORMATION)
                _eventDict.Register(this, eventDescriptor);
        }
    }

    public void Unregister()
    {
        foreach (int eventId in _entry._eventDescriptorDict.Keys)
        {
            StageEventDescriptor eventDescriptor = _entry._eventDescriptorDict[eventId];
            int senderId = eventDescriptor.ListenerId;

            if (senderId == StageEventDict.STAGE_ENVIRONMENT)
                _owner.Env.EventDict.Unregister(this, eventDescriptor);
            else if (senderId == StageEventDict.STAGE_ENTITY)
                ;
            else if (senderId == StageEventDict.STAGE_BUFF)
                ;
            else if (senderId == StageEventDict.STAGE_FORMATION)
                _eventDict.Unregister(this, eventDescriptor);
        }
    }
}
