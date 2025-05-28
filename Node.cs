public class Node {
    public int X {
        get;
        set;
    }
    public int Y {
        get;
        set;
    }

    // Cost from start
    public int G{
        get;
        set;
    } 

    // Heuristic cost to end
    public int H {
        get;
        set;
    } 

    // Total cost
    public int F => G + H; 
    public Node Parent {
        get;
        set;
    }
    public bool Walkable {
        get;
        set;
    }

    public Node(int x, int y, bool walkable = true) {
        X = x;
        Y = y;
        Walkable = walkable;
    }
}