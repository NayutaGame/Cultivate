
public class DifficultyRunConfigTabView : RunConfigTabView
{
    public override void Refresh()
    {
        base.Refresh();
        DifficultyRunConfigTabControl control = Get<DifficultyRunConfigTabControl>();
        DifficultyEntry difficultyEntry = control.GetSelectedDifficultyProfile().GetEntry();
        RowLabel.text = $"难度：{difficultyEntry.GetName()}";
    }

    public override void OnEnable()
    {
        base.OnEnable();
        AppManager.Instance.ConfigManager.DifficultyTabControl.DifficultySelectNeuron.Join(DifficultyChanged);
    }

    public override void OnDisable()
    {
        base.OnDisable();
        AppManager.Instance.ConfigManager.DifficultyTabControl.DifficultySelectNeuron.Remove(DifficultyChanged);
    }

    private void DifficultyChanged(DifficultySelectDetails d)
    {
        DifficultyEntry difficultyEntry = d.ToDifficulty.GetEntry();
        RowLabel.text = $"难度：{difficultyEntry.GetName()}";
    }
}