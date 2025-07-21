
using UnityEngine;

public class CharacterAnnotationAlignmentDetails : AnnotationAlignmentDetails
{
    public int CharacterIndex;
    public Rect CharacterRect;

    public CharacterAnnotationAlignmentDetails(int characterIndex, Rect characterRect)
    {
        CharacterIndex = characterIndex;
        CharacterRect = characterRect;
    }

    public override Vector3 GetProgressCirclePosition()
    {
        return new Vector3(CharacterRect.xMax, CharacterRect.yMax, 0);
    }

    public override Vector3 GetCenterPosition()
    {
        return CharacterRect.center;
    }
}