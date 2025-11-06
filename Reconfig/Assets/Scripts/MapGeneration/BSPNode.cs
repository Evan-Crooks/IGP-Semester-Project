public class BSPNode
{
    public int x, y, width, height;
    public BSPNode left, right;
    public Room room;
    public BSPNode(int x, int y, int width, int height)
    {
        this.x = x;
        this.y = y;
        this.width = width;
        this.height = height;
    }

    public bool IsLeaf()
    {
        return left == null && right == null;
    }
}
