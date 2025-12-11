using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class GridNode
{
    public Vector2Int GridPosition;//position of hte node
    public Vector2 WorldPosition; //center coordinate of the of the node
    public float GCost = float.MaxValue;
    public float HCost;
    public bool isWalkable;
    public float FCost => GCost + HCost;
    public GridNode Parent;
    public GridNode(Vector2Int gridPos, Vector2 worldPos, bool isWalkable)
    {
        GridPosition = gridPos;
        WorldPosition = worldPos;
        this.isWalkable = isWalkable;
    }
}
public class AStar : MonoBehaviour
{
    public static AStar instance { get; set; }
    public Tilemap tilemap;
    private Vector3 cellSize => tilemap.cellSize;
    private BoundsInt bounds;
    //stores grid of nodes which contain grid position and world position and whether the node is walkable or not
    private GridNode[,] grid;

    private void Awake()
    {
        instance = this;
    }

    //taken from: https://medium.com/@aliyousefi-dev/unity-pathfinder-a-a-star-algorithm-guide-a2a0e9b92bb7
    //grid node stores
    //GCost: distance from start node
    //HCost: Estimated distance to the goal
    //FCost: Sum of G and H cost(lower cost is better)
    //Parent for path reconstruction
    

    //building grid which we will use to perform astar
    public void BuildGrid()
    {
        bounds = tilemap.cellBounds;
        grid = new GridNode[bounds.size.x, bounds.size.y];
        for(int i = 0; i < bounds.size.x; i++)
        {
            for(int j = 0; j < bounds.size.y; j++)
            {
                Vector3Int cellPos = new Vector3Int(bounds.x + i, bounds.y + j, 0);
                bool walkable = !tilemap.HasTile(cellPos);
                Vector3 worldPos3 = tilemap.GetCellCenterWorld(cellPos);
                Vector2 worldPos2 = new Vector2(worldPos3.x, worldPos3.y);
                grid[i, j] = new GridNode(new Vector2Int(i, j), worldPos2, walkable);
            }
        }
    }

    //used to get neighbors for a node
    private List<GridNode> GetNeighbors(GridNode node)
    {
        List<GridNode> Neighbors = new List<GridNode>();
        //get neighbors up, down, left, right(maybe diagonal in the future)
        //1,0 right
        if(node.GridPosition.x + 1  < bounds.size.x)
        {
            //add to list
            Neighbors.Add(grid[node.GridPosition.x+1, node.GridPosition.y]);
        }
        //-1,0 left
        if(node.GridPosition.x - 1 >= 0)
        {
            Neighbors.Add(grid[node.GridPosition.x - 1, node.GridPosition.y]);
        }
        //0,1 up
        if(node.GridPosition.y + 1 < bounds.size.y)
        {
            Neighbors.Add(grid[node.GridPosition.x, node.GridPosition.y+1]);
        }
        //0, -1 down
        if(node.GridPosition.y -1 >= 0)
        {
            Neighbors.Add(grid[node.GridPosition.x, node.GridPosition.y-1]);
        }
        return Neighbors;
    }

    private List<GridNode> GetPath(GridNode node, GridNode player)
    {
        List<GridNode> res = new List<GridNode>();
        GridNode current = node;
        while(current != player)
        {
            res.Add(current);
            current = current.Parent;
        }
        res.Add(player);
        res.Reverse();
        return res;
    }

    //astar method all enemies call
    public List<GridNode> FindPath(Vector2Int start, Vector2Int end)
    {
        GridNode startNode = grid[start.x, start.y];
        GridNode endNode = grid[end.x, end.y];

        //reset all node gcosts
        for (int x = 0; x < grid.GetLength(0); x++)
        {
            for (int y = 0; y < grid.GetLength(1); y++)
            {
                grid[x, y].GCost = float.MaxValue;
                grid[x, y].Parent = null; // also reset parent
            }
        }

        //initialize open and close list
        PriorityQueue<GridNode> openList = new PriorityQueue<GridNode>();
        HashSet<GridNode> closedList = new HashSet<GridNode>();
        //add start node but leave it's f cost to zero
        startNode.GCost = 0;
        startNode.HCost = Vector2Int.Distance(start, end);
        startNode.Parent = null;
        openList.Enqueue(startNode, (int)startNode.FCost);
        int iteration = 0;
        int maxIterations = 100000; // adjust as needed
        //loop until you find the end
        while (openList.Count > 0)
        {
            //Debug.Log("iteration: " + iteration);
            //if(iteration > maxIterations)
            //{
            //    break;
            //}
            //iteration++;
            //get the current node in list which has lowest f value
            GridNode current = openList.Dequeue();
            //remove the current node from the open list and add it to the closed list
            closedList.Add(current);

            //if the current node is the goal
            if(current == endNode)
            {
                Debug.Log("Found End");

                List<GridNode> nodes = GetPath(current, startNode);
                return nodes;
            }
            //otherwise get children
            foreach (GridNode neighbor in GetNeighbors(current))
            {
                //if child is in closed list or is not walkable, continue check if neighbor is in bounds
                if (!neighbor.isWalkable || closedList.Contains(neighbor))
                {
                    continue;
                }

                //create g and h values for child
                //g cost is defined as the distance between current node and start node
                float gCost = current.GCost + Vector2Int.Distance(current.GridPosition, neighbor.GridPosition);
                float hCost = Vector2Int.Distance(neighbor.GridPosition, endNode.GridPosition);

                if(gCost >= neighbor.GCost)
                {
                    continue;
                }

                //update neighbor costs and parent
                neighbor.GCost = gCost;
                neighbor.HCost = hCost;
                neighbor.Parent = current;

                //update position in open list
                openList.Enqueue(neighbor, (int)neighbor.FCost);
            }
        }
        //no path found
        return null;
    }

    //draw the grid with coordinate labels and obstacles and coordinate labels
    private void OnDrawGizmos()
    {
        //draw the grid
        if (grid == null) return;

        Gizmos.color = Color.white;

        for (int x = 0; x < grid.GetLength(0); x++)
        {
            for (int y = 0; y < grid.GetLength(1); y++)
            {
                GridNode node = grid[x, y];

                //color by walkability
                Gizmos.color = node.isWalkable ? Color.green : Color.red;
                if(Gizmos.color == Color.red)
                {
                    continue;
                }

                //draw the tile outline
                Gizmos.DrawWireCube(node.WorldPosition, tilemap.cellSize);

                //draw center point
                Gizmos.DrawSphere(node.WorldPosition, 0.05f);
            }
        }
    }

    public Vector2Int WorldToGrid(Vector3 worldPos)
    {
        // Convert world position to cell position in the tilemap
        Vector3Int cellPos = tilemap.WorldToCell(worldPos);

        // Convert to local grid coordinates (relative to bounds)
        int x = cellPos.x - bounds.x;
        int y = cellPos.y - bounds.y;

        // Clamp values to grid bounds
        x = Mathf.Clamp(x, 0, grid.GetLength(0) - 1);
        y = Mathf.Clamp(y, 0, grid.GetLength(1) - 1);

        return new Vector2Int(x, y);
    }


}
