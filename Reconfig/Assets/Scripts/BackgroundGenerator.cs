using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[System.Serializable]
public struct WeightedTile
{
    public TileBase tile;
    public float weight;
}

public class BackgroundGenerator : MonoBehaviour
{
    [SerializeField] private BSPMapGenerator mapGenerator;
    [SerializeField] private Tilemap backgroundTilemap;
    [SerializeField] private List<WeightedTile> tiles = new List<WeightedTile>();

    private void Awake()
    {
        if (backgroundTilemap == null)
        {
            backgroundTilemap = GetComponent<Tilemap>();
        }
    }

    private void Start()
    {
        GenerateBackground();
    }

    [ContextMenu("Generate Background")]
    public void GenerateBackground()
    {
        if (mapGenerator == null || backgroundTilemap == null)
        {
            Debug.LogWarning($"{nameof(BackgroundGenerator)}: Missing mapGenerator or backgroundTilemap reference.");
            return;
        }

        int width = mapGenerator.m_width;
        int height = mapGenerator.m_height;

        float totalWeight = 0f;
        foreach (var wt in tiles)
        {
            if (wt.tile != null && wt.weight > 0f)
            {
                totalWeight += wt.weight;
            }
        }

        if (totalWeight <= 0f)
        {
            Debug.LogWarning($"{nameof(BackgroundGenerator)}: No valid weighted tiles configured.");
            return;
        }

        const float weightTolerance = 0.001f;
        if (Mathf.Abs(totalWeight - 1f) > weightTolerance)
        {
            Debug.LogWarning($"{nameof(BackgroundGenerator)}: Sum of weights is {totalWeight:F3}, expected 1. Tiles will be normalized implicitly during selection.");
        }

        backgroundTilemap.ClearAllTiles();

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                TileBase chosen = ChooseTile(totalWeight);
                if (chosen == null)
                {
                    continue;
                }

                backgroundTilemap.SetTile(new Vector3Int(x, y, 0), chosen);
            }
        }

        backgroundTilemap.RefreshAllTiles();
    }

    private TileBase ChooseTile(float totalWeight)
    {
        // Treat weights as probabilities that sum to 1. If they do not, we still roll in [0,1] and fall back to the last valid entry.
        float roll = Random.value;
        float accumulator = 0f;
        TileBase lastValid = null;

        foreach (var wt in tiles)
        {
            if (wt.tile == null || wt.weight <= 0f)
            {
                continue;
            }

            accumulator += wt.weight;
            lastValid = wt.tile;
            if (roll <= accumulator)
            {
                return wt.tile;
            }
        }

        // If weights sum to less than 1, roll could exceed accumulator; fall back to last valid tile.
        return lastValid;
    }
}
