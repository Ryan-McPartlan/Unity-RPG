using UnityEngine;
using System.Collections;

public class Node : IHeapItem<Node>{

    public bool walkable;
    public Vector2 position;
    public int gridX;
    public int gridY;
    public Node parent;
    public int penalty;
    int heapIndex;

    public int gCost;
    public int hCost;

    public Node(bool walkableIn, Vector3 worldPos, int xIn, int yIn, int penaltyIn)
    {
        walkable = walkableIn;
        position = worldPos;
        gridX = xIn;
        gridY = yIn;
        penalty = penaltyIn;
    }

    public int fCost
    {
        get {
            return (gCost + hCost);
        }
    }

    public int HeapIndex
    {
        get { return heapIndex; }
        set {heapIndex = value;}
    }

    public int CompareTo(Node nodeToCompare)
    {
        int compare = fCost.CompareTo(nodeToCompare.fCost);
        if (compare == 0)
        {
            hCost.CompareTo(nodeToCompare.hCost);
        }
        return -compare;
    }
}
