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

    public override void DidAwake()
    {
        base.DidAwake();

        KeywordCategory = new KeywordCategory();
        BuffCategory = new BuffCategory();
        ChipCategory = new ChipCategory();
        TileResourceCategory = new TileResourceCategory();
        TerrainCategory = new TerrainCategory();
        ProductCategory = new ProductCategory();
        EnemyCategory = new EnemyCategory();
        TechCategory = new TechCategory();

        TechCategory.Init();
    }
}
