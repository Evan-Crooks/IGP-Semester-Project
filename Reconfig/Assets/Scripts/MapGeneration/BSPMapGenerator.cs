using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Tilemaps;
using UnityEngine.U2D;
using System;
using Unity.Mathematics;
public class BSPMapGenerator : MonoBehaviour
{
    public int width = 50;
    public int height = 50;
    public int maxDepth = 6;

    //min height and min width
    public int minRoomHeight = 10;
    public int minRoomWidth = 20;

    public Tilemap tilemap;
    public TileBase wallTile;

    private BSPNode root;
    // Start is called before the first frame update
    void Start()
    {
        //initialize the root of the tree
        root = new BSPNode(0, 0, width, height);
        //generate partitions
        BSPSplit(root, 0);
        GenerateRooms(root);
        FillWithWalls();
        DrawRooms(root);
        ConnectRooms(root);

    }


    void BSPSplit(BSPNode root, int currentDepth)
    {
        if (root==null)
        {
            return;
        }
        if(currentDepth >= maxDepth)
        {
            return;
        }
        //check if can split vertically
        bool canSplitVertically = root.width >= 2 * minRoomWidth;
        //check if can split horizontally
        bool canSplitHorizontally = root.height >= 2 * minRoomHeight;
        //pick random dimension
        float rand = UnityEngine.Random.value;

        if(rand > 0.5f && canSplitVertically)
        {
            int splitX = UnityEngine.Random.Range(root.x + 1, root.x + root.width);
            root.left = new BSPNode(root.x, root.y, splitX - root.x, root.height);
            root.right = new BSPNode(splitX, root.y, root.x + root.width - splitX, root.height);
        } else if (canSplitHorizontally)
        {
            int splitY = UnityEngine.Random.Range(root.y + 1, root.y + root.height);
            root.left = new BSPNode(root.x, root.y, root.width, splitY - root.y);
            root.right = new BSPNode(root.x, splitY, root.width, root.y + root.height - splitY);
        }

        BSPSplit(root.left, currentDepth + 1);
        BSPSplit(root.right, currentDepth + 1);
        
    }

    void DrawRooms(BSPNode node)
    {
        if(node == null)
        {
            return;
        }

        if (node.IsLeaf())
        {
            node.room.Draw(tilemap);
        }
        else
        {
            DrawRooms(node.left);
            DrawRooms(node.right);
        }
    }

    void FillWithWalls()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                tilemap.SetTile(new Vector3Int(x, y, 0), wallTile);
            }
        }
    }

    void GenerateRooms(BSPNode node)
    {
        if (node.IsLeaf())
        {
            int padding = 1;

            //maximum size
            int maxWidth = node.width - 2 * padding;
            int maxHeight = node.height - 2 * padding;

            //minimum size
            int minWidth = Mathf.Min(minRoomWidth, maxWidth);
            int minHeight = Mathf.Min(minRoomHeight, maxHeight);

            if (maxWidth < minWidth || maxHeight < minHeight)
            {
                node.room = null;
                return;
            }

            //random room size 
            int roomWidth = UnityEngine.Random.Range(minWidth, maxWidth + 1);
            int roomHeight = UnityEngine.Random.Range(minHeight, maxHeight + 1);

            //random position inside partition
            int left = UnityEngine.Random.Range(node.x + padding, node.x + node.width - roomWidth);
            int bottom = UnityEngine.Random.Range(node.y + padding, node.y + node.height - roomHeight);
            int right = left + roomWidth - 1;
            int top = bottom + roomHeight - 1;

            //create partition
            node.room = new Room(left, right, top, bottom);
        }
        else if(node != null)
        {
            GenerateRooms(node.left);
            GenerateRooms(node.right);
        }
    }

    void ConnectRooms(BSPNode node)
    {
        if(node == null)
        {
            return;
        }
        //have children connect to its children first
        ConnectRooms(node.left);
        ConnectRooms(node.right);

        if (node.left == null && node.right == null)
        {
            return;
        }

        //now connect children together
        int x1 = node.left.x + node.left.width / 2;
        int y1 = node.left.y + node.left.height / 2;

        int x2 = node.right.x + node.right.width / 2;
        int y2 = node.right.y + node.right.height / 2;

        int corridorWidth = 3;
        int halfWidth = corridorWidth / 2;

        //horizontal segment
        for (int x = Math.Min(x1, x2); x <= Math.Max(x1, x2); x++)
        {
            for (int dy = -halfWidth; dy <= halfWidth; dy++)
            {
                tilemap.SetTile(new Vector3Int(x, y1 + dy, 0), null);
            }
        }

        //vertical segment
        for (int y = Math.Min(y1, y2); y <= Math.Max(y1, y2); y++)
        {
            for (int dx = -halfWidth; dx <= halfWidth; dx++)
            {
                tilemap.SetTile(new Vector3Int(x2 + dx, y, 0), null);
            }
        }
    }
}
