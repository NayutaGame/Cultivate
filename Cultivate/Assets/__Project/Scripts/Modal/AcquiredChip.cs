using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcquiredChip : RunChip
{
    public Tile Tile { get; private set; }

    public AcquiredChip(RunChip chip, Tile tile) : base(chip._entry, chip.Level)
    {
        Tile = tile;
    }
}
