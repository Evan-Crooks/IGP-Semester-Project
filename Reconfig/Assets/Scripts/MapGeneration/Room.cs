using UnityEngine;
using UnityEngine.Tilemaps;

public class Room
{
    protected int left, right, top, bottom;

    protected int GetWidth()
    {
        return right - left + 1;
    }

    protected int GetHeight()
    {
        return top - bottom + 1;
    }

    public Room(int left, int right, int top, int bottom)
    {
        this.left = left;
        this.right = right;
        this.top = top;
        this.bottom = bottom;
    }

    public void Draw(Tilemap tilemap)
    {
        for(int x = left; x <= right; x++)
        {
            for(int  y = bottom; y <= top; y++)
            {
                tilemap.SetTile(new Vector3Int(x, y, 0), null);
            }
        }
    }

    public int GetLeft()
    {
        return left;
    }
    public int GetRight()
    {
        return right;
    }
    public int GetTop()
    {
        return top;
    }
    public int GetBottom()
    {
        return bottom;
    }

    public int GetCenterX()
    {
        return (right + left) / 2;
    }

    public int GetCenterY()
    {
        return (top + bottom) / 2;
    }
}