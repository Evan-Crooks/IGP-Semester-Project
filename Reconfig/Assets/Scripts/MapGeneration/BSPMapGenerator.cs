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
    public int m_width = 50;
    public int m_height = 50;
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
        root = new BSPNode(0, 0, m_width, m_height);
        //generate partitions
        BSPSplit(root, 0);
        GenerateRooms(root);
        FillWithWalls();
        ConnectRooms(root);
        DrawRooms(root);

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


        //width height ratio
        float ratio = ((float)(root.width)) / ((float)(root.height));

        //choose how we split based on current width height ratio
        float splitDirection = 0.0f;
        if(ratio > 1.25)
        {
            //room is too wide so we split along the x axis
            splitDirection = 1.0f;
        } else if(ratio < 0.75)
        {
            //room is too high so we split along y axis
            splitDirection = 0.1f;
        } else
        {
            //neither too wide nor too tall, so we split randomly
            splitDirection = UnityEngine.Random.value;
        }

        //choose max height to width ratio if splitting along x axis
        //choose min height to width ratio if splitting along x axis
        //calculate min sub room width
        int minRoomContainerWidth = (int)(root.width*0.45f);
        //calculate max sub room width
        int maxRoomContainerWidth = (int)(root.width * 0.55f);
        //choose max width to height ratio if splitting along y axis
        //calculate min sub room height
        int minRoomContainerHeight = (int)(root.height * 0.45f);
        //calculate max sub room height
        int maxRoomContainerHeight = (int)(root.height * 0.55f);

        //split room based on ranges set
        if(splitDirection > 0.5f)
        {
            //pick a random width for sub room, left room width will be this, right room will be width - this width
            int randomWidth = UnityEngine.Random.Range(minRoomContainerWidth, maxRoomContainerWidth);
            //calculate left room
            int leftRoomWidth = randomWidth;
            root.left = new BSPNode(root.x, root.y, leftRoomWidth, root.height);
            //calculate right room width using splitX
            int rightRoomWidth = root.width - randomWidth;
            root.right = new BSPNode(root.x+randomWidth, root.y, rightRoomWidth, root.height);
        } else {
            //get random location within room splitting range along the y axis to split
            int randomHeight = UnityEngine.Random.Range(minRoomContainerHeight, maxRoomContainerHeight);
            int bottomRoomHeight = randomHeight;
            root.left = new BSPNode(root.x, root.y, root.width,bottomRoomHeight);
            int topRoomHeight = root.height - randomHeight;
            root.right = new BSPNode(root.x, root.y+bottomRoomHeight, root.width, topRoomHeight);
        }

        BSPSplit(root.left, currentDepth + 1);
        BSPSplit(root.right, currentDepth + 1);
        
    }

    void FillWithWalls()
    {
        for (int x = 0; x < m_width; x++)
        {
            for (int y = 0; y < m_height; y++)
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
            //generate rooms for left child and right child
            GenerateRooms(node.left);
            GenerateRooms(node.right);
            //now nodes left and right should have a room, time to pick their middle point as our center
            int xLeft = node.left.room.GetCenterX();
            int yLeft = node.left.room.GetCenterY();
            int xRight = node.right.room.GetCenterX();
            int yRight = node.right.room.GetCenterY();
            int xMid = (xRight + xLeft)/2;
            int yMid = (yRight + yLeft)/2;

            //center will be xMid and yMid
            node.room = new Room(xMid, xMid, yMid, yMid);
        }
    }

    /*Corridor drawing section of code*/
    void ConnectRooms(BSPNode node)
    {
        if(node == null)
        {
            return;
        }
        //have children connect to its children first
        ConnectRooms(node.left);
        ConnectRooms(node.right);

        //if no rooms to connect
        if (node.left == null || node.right == null)
        {
            return;
        }

        if (node.left.room == null || node.right.room == null)
        {
            return;
        }


        //get center of room for left child and right child
        int x1 = node.left.room.GetCenterX();
        int y1 = node.left.room.GetCenterY();

        int x2 = node.right.room.GetCenterX();
        int y2 = node.right.room.GetCenterY();

        //get center of room for parent
        int xParent = node.room.GetCenterX();
        int yParent = node.room.GetCenterY();


        //connect room left to parent center point
        float rand = UnityEngine.Random.value;
        int corridorWidth = UnityEngine.Random.Range(3, 5);
        if (rand > 0.5)
        {
            bresenham(x1, y1, xParent, yParent, corridorWidth);
        } else
        {
            manhattanLine(x1, y1, xParent, yParent, 3);
        }

        rand = UnityEngine.Random.value;
        corridorWidth = UnityEngine.Random.Range(3, 5);
        if (rand > 0.5)
        {
            bresenham(x2, y2, xParent, yParent, corridorWidth);
        }
        else
        {
            manhattanLine(x2, y2, xParent, yParent, 3);
        }

    }


    //https://www.roguebasin.com/index.php/Bresenham%27s_Line_Algorithm#C.23
    void swap<T>(ref T lhs, ref T rhs)
    {
        T temp;
        temp = lhs;
        lhs = rhs;
        rhs = temp;
    }
    //https://www.roguebasin.com/index.php/Bresenham%27s_Line_Algorithm#C.23
    void bresenham(int x0, int y0, int x1, int y1, int corridorWidth)
    {
        bool steep = Math.Abs(y1 - y0) > Math.Abs(x1 - x0);
        if (steep) { swap<int>(ref x0, ref y0); swap<int>(ref x1, ref y1); }
        if (x0 > x1) { swap<int>(ref x0, ref x1); swap<int>(ref y0, ref y1); }
        int dX = (x1 - x0), dY = Math.Abs(y1 - y0), err = (dX / 2), ystep = (y0 < y1 ? 1 : -1), y = y0;

        for (int x = x0; x <= x1; ++x)
        {
            if (!(steep ? drawCorridor(y, x, corridorWidth) : drawCorridor(x, y, corridorWidth))) return;
            err = err - dY;
            if (err < 0) { y += ystep; err += dX; }
        }

    }

    void manhattanLine(int x0, int y0, int x1, int y1, int corridorWidth)
    {
        int halfWidth = corridorWidth / 2;
        for (int x = Math.Min(x0, x1); x <= Math.Max(x0, x1); x++)
        {
            for (int dy = -halfWidth; dy <= halfWidth; dy++)
            {
                tilemap.SetTile(new Vector3Int(x, y0 + dy, 0), null);
            }
        }

        for (int y = Math.Min(y0, y1); y <= Math.Max(y0, y1); y++)
        {
            for (int dx = -halfWidth; dx <= halfWidth; dx++)
            {
                tilemap.SetTile(new Vector3Int(x1 + dx, y, 0), null);
            }
        }
    }


    bool drawCorridor(int x, int y, int corridorWidth)
    {
        int halfWidth = corridorWidth / 2;
        for (int i = -halfWidth; i <= halfWidth; i++)
        {
            for (int j = -halfWidth; j <= halfWidth; j++)
            {
                tilemap.SetTile(new Vector3Int(x + i, y + j, 0), null);
            }
        }
        return true;
    }


    /*Drawing and customizing rooms part of code*/

    void DrawRooms(BSPNode node)
    {
        if (node == null)
        {
            return;
        }

        if (node.IsLeaf())
        {
            drawRoom(node.room, tilemap);
        }
        else
        {
            DrawRooms(node.left);
            DrawRooms(node.right);
        }
    }
    public void drawRoom(Room room, Tilemap tilemap)
    {
        //choose either cave or not cave room
        float rand = UnityEngine.Random.value;
        if (rand > 0.5)
        {
            //draw rectangle
            int left = room.GetLeft();
            int right = room.GetRight();
            int top = room.GetTop();
            int bottom = room.GetBottom();
            for (int x = left; x <= right; x++)
            {
                for (int y = bottom; y <= top; y++)
                {
                    tilemap.SetTile(new Vector3Int(x, y, 0), null);
                }
            }
            int platforms = UnityEngine.Random.Range(1, 10);
            //fill with platforms
            AddPlatforms(room, platforms);
        }
        else
        {
            //determining cave density
            rand = UnityEngine.Random.Range(0.4f, 0.6f);
            generateCave(room, rand);
        }

    }

    //https://www.roguebasin.com/index.php/Random_Walk_Cave_Generation
    void generateCave(Room room, float proportionOfEmptySpace)
    {
        //start from center of room
        int x = room.GetCenterX();
        int y = room.GetCenterY();
        //get dimensions of room
        int left = room.GetLeft();
        int right = room.GetRight();
        int top = room.GetTop();
        int bottom = room.GetBottom();
        //number of tiles we want to be walkable
        int width = right - left + 1;
        int height = bottom - top + 1;
        int totalTiles = Math.Abs(width * height);
        int emptyTiles = (int)(totalTiles * proportionOfEmptySpace);
        int tilesEmptied = 1;
        //start random walk which is constrained along dimension
        while (tilesEmptied < emptyTiles)
        {
            //take step in random direction
            bool moved = false;
            while (!moved)
            {
                int direction = UnityEngine.Random.Range(0, 4);
                switch (direction)
                {
                    case 0:
                        //move up x, y+1
                        if(y+1 <= 0 + m_height-2)
                        {
                            y++;
                            moved = true;
                        }
                        break;
                    case 1:
                        //move down x, y-1
                        if (y-1 >= 1)
                        {
                            y--;
                            moved = true;
                        }
                        break;
                    case 2:
                        //move left x-1, y
                        if(x-1 > 0)
                        {
                            x--;
                            moved = true;
                        }
                        break;
                    default:
                        //move right x+1, y
                        if(x+1 <= 0+m_width-2)
                        {
                            x++;
                            moved = true;
                        }
                        break;
                }
            }
            //if cell is wall
            if (tilemap.GetTile(new Vector3Int(x,y,0)) != null){
                //turn new cell into floor and increment tiles emptied
                tilemap.SetTile(new Vector3Int(x, y, 0), null);
                tilesEmptied++;
            }
        }
    }

    void AddPlatforms(Room room, int numberOfPlatforms)
    {
        if (room == null)
        {
            return;
        }

        //get room dimensions
        int left = room.GetLeft() + 2;
        int right = room.GetRight() - 2;
        int top = room.GetTop() - 3;
        int bottom = room.GetBottom() + 3;

        //calculate room width
        int width = right - left + 1;
        //calculate room height
        int height = top - bottom + 1;

        //generate platforms
        for (int platform = 0; platform < numberOfPlatforms; platform++)
        {
            int platformWidth = UnityEngine.Random.Range(3, width / 2);
            int platformHeight = UnityEngine.Random.Range(1, 4);

            int startX = UnityEngine.Random.Range(left, right - platformWidth + 1);
            int startY = UnityEngine.Random.Range(bottom, top - platformHeight + 1);

            //place platform
            for (int x = startX; x < startX+platformWidth; x++)
            {
                for (int y = startY; y < startY+platformHeight; y++)
                {
                    Vector3Int pos = new Vector3Int(x, y, 0);
                    tilemap.SetTile(pos, wallTile);
                }
            }
        }
    }

    void addColumns(Room room, int numberOfColumns)
    {

    }
}
