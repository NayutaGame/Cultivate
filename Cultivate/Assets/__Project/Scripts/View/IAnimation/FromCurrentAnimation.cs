
using UnityEngine;

public abstract class FromCurrentAnimation : CLAnimation
{
    protected bool HasRecord;
    protected Configuration StartConfiguration;

    protected FromCurrentAnimation(RectTransform content) : base(content)
    {
        HasRecord = false;
    }

    protected void TryRecordConfiguration()
    {
        if (HasRecord)
            return;

        RecordConfiguration();
    }

    public FromCurrentAnimation RecordConfiguration(Configuration configuration)
    {
        HasRecord = true;
        StartConfiguration = configuration;
        ConfigurationIsSet();
        return this;
    }

    public virtual FromCurrentAnimation RecordConfiguration()
    {
        HasRecord = true;
        StartConfiguration = Configuration.FromRect(Content);
        ConfigurationIsSet();
        return this;
    }

    protected virtual void ConfigurationIsSet()
    {
        
    }
}