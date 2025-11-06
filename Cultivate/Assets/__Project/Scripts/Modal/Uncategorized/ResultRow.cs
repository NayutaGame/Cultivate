
public class ResultRow
{
    private int _minValue;
    private int _maxValue;
    private string _rewardDescription;

    public bool IsActive;

    public ResultRow(int minValue, int maxValue, string rewardDescription)
    {
        _minValue = minValue;
        _maxValue = maxValue;
        _rewardDescription = rewardDescription;

        IsActive = false;
    }

    public bool IsInRange(int value)
    {
        return value >= _minValue && value <= _maxValue;
    }

    public int GetMinValue() => _minValue;
    public int GetMaxValue() => _maxValue;

    public Description GetDescription()
    {
        Description description = new Description();
        
        // 生成范围字符串
        string rangeString;
        if (_minValue == _maxValue)
        {
            rangeString = $"[{_minValue}]";
        }
        else
        {
            rangeString = $"[{_minValue}, {_maxValue}]";
        }
        
        description.Join(rangeString);
        description.Join(" ~ ");
        description.Join(_rewardDescription);
        
        return description;
    }
}