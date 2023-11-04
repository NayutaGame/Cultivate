
using System.Threading.Tasks;

/// <summary>
/// Buff
/// </summary>
public class Buff : CLEventListener
{
    private StageEntity _owner;
    public StageEntity Owner => _owner;

    private BuffEntry _entry;
    public BuffEntry Entry => _entry;

    public string GetName() => _entry.Name;

    private int _stack;
    public int Stack => _stack;
    public async Task SetStack(int stack)
    {
        await _eventDict.SendEvent(CLEventDict.STACK_WILL_CHANGE, new BuffStackChangeDetails(_stack, stack));
        _stack = stack;
        await _eventDict.SendEvent(CLEventDict.STACK_DID_CHANGE, new BuffStackChangeDetails(_stack, stack));

        if(_stack <= 0)
            await _owner.RemoveBuff(this);
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

    public CLEventDict _eventDict;

    public Buff(StageEntity owner, BuffEntry entry)
    {
        _owner = owner;
        _entry = entry;
        _stack = 0;

        _eventDict = new();
    }

    public void Register()
    {
        foreach (int eventId in _entry._eventDescriptorDict.Keys)
        {
            CLEventDescriptor eventDescriptor = _entry._eventDescriptorDict[eventId];
            int senderId = eventDescriptor.ListenerId;

            if (senderId == CLEventDict.STAGE_ENVIRONMENT)
                _owner.Env._eventDict.Register(this, eventDescriptor);
            else if (senderId == CLEventDict.STAGE_ENTITY)
                ;
            else if (senderId == CLEventDict.STAGE_BUFF)
                ;
            else if (senderId == CLEventDict.STAGE_FORMATION)
                _eventDict.Register(this, eventDescriptor);
        }
    }

    public void Unregister()
    {
        foreach (int eventId in _entry._eventDescriptorDict.Keys)
        {
            CLEventDescriptor eventDescriptor = _entry._eventDescriptorDict[eventId];
            int senderId = eventDescriptor.ListenerId;

            if (senderId == CLEventDict.STAGE_ENVIRONMENT)
                _owner.Env._eventDict.Unregister(this, eventDescriptor);
            else if (senderId == CLEventDict.STAGE_ENTITY)
                ;
            else if (senderId == CLEventDict.STAGE_BUFF)
                ;
            else if (senderId == CLEventDict.STAGE_FORMATION)
                _eventDict.Unregister(this, eventDescriptor);
        }
    }
}
