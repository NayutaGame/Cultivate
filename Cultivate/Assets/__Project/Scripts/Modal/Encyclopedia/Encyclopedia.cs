using System.Collections;
using System.Collections.Generic;
using CLLibrary;
using UnityEngine;

public class Encyclopedia : Singleton<Encyclopedia>
{
    public static BuildingCategory BuildingCategory;
    public static OperationCategory OperationCategory;
    public static TerrainCategory TerrainCategory;
    public static TileResourceCategory TileResourceCategory;
    public static ChipCategory ChipCategory;
    public static BuffCategory BuffCategory;
    public static KeywordCategory KeywordCategory;

    public override void DidAwake()
    {
        base.DidAwake();

        KeywordCategory = new KeywordCategory();
        BuffCategory = new BuffCategory();
        ChipCategory = new ChipCategory();
        TileResourceCategory = new TileResourceCategory();
        TerrainCategory = new TerrainCategory();
        OperationCategory = new OperationCategory();
        BuildingCategory = new BuildingCategory();
    }
}
