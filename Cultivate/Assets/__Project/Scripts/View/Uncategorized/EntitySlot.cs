using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public abstract class EntitySlot : MonoBehaviour
{
    public Transform EntityView;

    public abstract Tween GetAttackTween();
    public abstract Tween GetAttackedTween();
}
