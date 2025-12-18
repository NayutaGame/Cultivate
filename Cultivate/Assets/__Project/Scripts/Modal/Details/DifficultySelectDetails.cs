
public class DifficultySelectDetails
{
    public DifficultyProfile FromDifficulty;
    public DifficultyProfile ToDifficulty;

    public int FromIndex;
    public int ToIndex;

    public DifficultySelectDetails(DifficultyProfile toDifficulty)
    {
        ToDifficulty = toDifficulty;
    }
}