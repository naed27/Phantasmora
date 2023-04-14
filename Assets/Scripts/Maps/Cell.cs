public class Cell
{
    public string id;
    public string tile;

    public Coordinate coordinates;

    public string pathId = null;
    public Cell previousCell = null;

    public bool isDeadEnd = false;

    public Cell(int x, int y)
    {
        this.id = "(" + x.ToString()+ ", " + y.ToString() +")";
        this.coordinates = new Coordinate(x, y);
    }

    public bool IsDeadEnd() { return this.isDeadEnd; }

    public bool IsEdge() { return this.tile == "edge"; }

    public bool IsWall() { return this.tile == "wall"; }

    public bool IsFloor() { return this.tile == "floor"; }

    public bool IsNull() { return this.tile == "null"; }

    public bool IsEndPoint() { return this.tile == "end";  }

    public bool IsSameCell(Cell cell) {
        if (cell == null) return false;
        return this.id == cell.id;
    }

    public bool IsWithinPath(string pathId) { return this.pathId == pathId; }

}
