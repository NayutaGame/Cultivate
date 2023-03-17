using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Encyclopedia : CLLibrary.Singleton<Encyclopedia>
{
    public static KeywordCategory KeywordCategory;
    public static BuffCategory BuffCategory;
    public static ChipCategory ChipCategory;
    public static TileResourceCategory TileResourceCategory;
    public static TerrainCategory TerrainCategory;
    public static ProductCategory ProductCategory;
    public static EnemyCategory EnemyCategory;
    public static TechCategory TechCategory;
    public static NodeCategory NodeCategory;

    public override void DidAwake()
    {
        base.DidAwake();

        KeywordCategory = new();
        BuffCategory = new();
        ChipCategory = new();
        TileResourceCategory = new();
        TerrainCategory = new();
        ProductCategory = new();
        EnemyCategory = new();
        TechCategory = new();
        NodeCategory = new();

        TechCategory.Init();
    }
}
