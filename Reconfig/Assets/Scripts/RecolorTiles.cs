using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ReColorTiles : MonoBehaviour
{
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private TileBase basic;
    [SerializeField] private TileBase fallbackTexture;
    [Header("Edge Textures")]
    [SerializeField] private TileBase topTexture;
    [SerializeField] private TileBase bottomTexture;
    [SerializeField] private TileBase leftTexture;
    [SerializeField] private TileBase rightTexture;

    [Header("Corner Textures")]
    [SerializeField] private TileBase topLeftTexture;
    [SerializeField] private TileBase topRightTexture;
    [SerializeField] private TileBase bottomLeftTexture;
    [SerializeField] private TileBase bottomRightTexture;

    private void Awake()
    {
        if (tilemap == null)
        {
            tilemap = GetComponent<Tilemap>();
        }
    }


    [ContextMenu("Replace Exposed Tiles")]
    public void ReplaceExposedTiles()
    {
        if (tilemap == null)
        {
            Debug.LogWarning($"{nameof(ReColorTiles)}: Missing tilemap reference.");
            return;
        }

        // Tighten bounds to the occupied area so we don't walk huge empty regions.
        tilemap.CompressBounds();

        int checkedTiles = 0;
        int replacedTiles = 0;
        int occupiedTiles = 0;
        int skippedMissingTexture = 0;

        BoundsInt bounds = tilemap.cellBounds;
        Debug.Log($"{nameof(ReColorTiles)}: Processing bounds {bounds.position} size {bounds.size}.");

        foreach (Vector3Int pos in bounds.allPositionsWithin)
        {
            if (!tilemap.HasTile(pos))
            {
                continue;
            }
            occupiedTiles++;

            // Only replace the basic tile when there is exposure to air.
            if (basic != null && tilemap.GetTile(pos) != basic)
            {
                continue;
            }

            // Check exposure on all four sides (and corners) in priority order.
            Vector3Int above = new Vector3Int(pos.x, pos.y + 1, pos.z);
            Vector3Int below = new Vector3Int(pos.x, pos.y - 1, pos.z);
            Vector3Int left = new Vector3Int(pos.x - 1, pos.y, pos.z);
            Vector3Int right = new Vector3Int(pos.x + 1, pos.y, pos.z);
            Vector3Int aboveLeft = new Vector3Int(pos.x - 1, pos.y + 1, pos.z);
            Vector3Int aboveRight = new Vector3Int(pos.x + 1, pos.y + 1, pos.z);
            Vector3Int belowLeft = new Vector3Int(pos.x - 1, pos.y - 1, pos.z);
            Vector3Int belowRight = new Vector3Int(pos.x + 1, pos.y - 1, pos.z);

            TileBase replacement = null;
            // Corners take precedence if both neighboring sides are air.
            if (!tilemap.HasTile(above) && !tilemap.HasTile(left))
            {
                replacement = topLeftTexture ?? topTexture ?? leftTexture;
            }
            else if (!tilemap.HasTile(above) && !tilemap.HasTile(right))
            {
                replacement = topRightTexture ?? topTexture ?? rightTexture;
            }
            else if (!tilemap.HasTile(below) && !tilemap.HasTile(left))
            {
                replacement = bottomLeftTexture ?? bottomTexture ?? leftTexture;
            }
            else if (!tilemap.HasTile(below) && !tilemap.HasTile(right))
            {
                replacement = bottomRightTexture ?? bottomTexture ?? rightTexture;
            }
            // Single exposed sides
            else if (!tilemap.HasTile(above))
            {
                replacement = topTexture;
            }
            else if (!tilemap.HasTile(below))
            {
                replacement = bottomTexture;
            }
            else if (!tilemap.HasTile(left))
            {
                replacement = leftTexture;
            }
            else if (!tilemap.HasTile(right))
            {
                replacement = rightTexture;
            }

            if (replacement == null)
            {
                if (fallbackTexture != null)
                {
                    replacement = fallbackTexture;
                }
                else
                {
                    skippedMissingTexture++;
                }
            }
            if (replacement != null)
            {
                tilemap.SetTile(pos, replacement);
                replacedTiles++;
            }

            checkedTiles++;
        }

        tilemap.RefreshAllTiles();
        Debug.Log($"{nameof(ReColorTiles)}: Occupied {occupiedTiles}, checked {checkedTiles}, replaced {replacedTiles}. Skipped (no texture assigned): {skippedMissingTexture}.");
    }
}
