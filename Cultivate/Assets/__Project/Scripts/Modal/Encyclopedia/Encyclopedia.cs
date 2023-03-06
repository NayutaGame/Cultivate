using System.Collections;
using System.Collections.Generic;
using CLLibrary;
using UnityEngine;

public class Encyclopedia : Singleton<Encyclopedia>
{
    public static OperationCategory OperationCategory;
    public static BuildingCategory BuildingCategory;
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
        BuildingCategory = new BuildingCategory();
        OperationCategory = new OperationCategory();
    }
}
