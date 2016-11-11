using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Grid : MonoBehaviour{

    public float nodeSize;
    public LayerMask unwalkableMask;
    public Vector2 gridWorldSize;
    public float nodeRadius;
    public Node[,] grid;
    public TerrainLayer[] terrains;
    Dictionary<int, int> terrainDic = new Dictionary<int,int>();

    int gridXSize, gridYSize;
    float nodeDiameter;

    [System.Serializable]
    public class TerrainLayer{
        public LayerMask layer;
        public int penalty;
    }

	// Use this for initialization
	void Awake () {
        nodeDiameter = nodeRadius * 2;
        gridXSize = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridYSize = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);

        foreach (TerrainLayer terrain in terrains)
        {
            terrainDic.Add(terrain.layer.value, terrain.penalty);
        }

        CreateGrid();
	}

    public int maxSize { get { return gridXSize * gridYSize; } }

    void CreateGrid()
    {
        grid = new Node[gridXSize, gridYSize];
        Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.up * gridWorldSize.y / 2;

        for (int x = 0; x < gridXSize; x++)
        {
            for (int y = 0; y < gridYSize; y++)
            {
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.up * (y * nodeDiameter + nodeRadius);
                bool walkable = !(Physics2D.OverlapArea(new Vector2(worldPoint.x - nodeRadius * nodeSize, worldPoint.y - nodeRadius * nodeSize), new Vector2(worldPoint.x + nodeRadius * nodeSize, worldPoint.y + nodeRadius * nodeSize), unwalkableMask));

                int penalty = 0;

                if (walkable)
                {
                    RaycastHit2D hit = Physics2D.Raycast(new Vector2(worldPoint.x, worldPoint.y), new Vector2(0, 0), 0.01F);
                    if (hit.collider != null)
                    {
                        LayerMask layer = hit.collider.gameObject.layer;
                        int layerValue = (int) Mathf.Pow(2,layer.value);
                        terrainDic.TryGetValue(layerValue, out penalty);
                    }
                }

                grid[x,y] = new Node(walkable, worldPoint, x, y, penalty);
            }
        }
    }

    public List<Node> getNeighbors(Node node)
    {
        List<Node> neighbors = new List<Node>();

        for(int x = -1; x <= 1; x++){
            for(int y = -1; y <= 1; y++){
                if((x != 0 && y != 0) || (x == 0 && y == 0))
                {
                    continue;
                }
                int checkX = node.gridX + x;
                int checkY = node.gridY + y;

                if(checkX > 0 && checkX < gridXSize -1 && checkY > 0 && checkY < gridYSize -1  )
                {
                    neighbors.Add(grid[checkX, checkY]);
                }
            }
        }

        return neighbors;
    }

    public Vector2 getWalkablePositionNear(Vector2 postion)
    {
        Node nodeForPosition = NodeFromWorldPoint(postion);
        if (nodeForPosition.walkable)
        {
            return postion;
        }

        List<Node> neighbors = getNeighbors(nodeForPosition);
        for (int i = 0; i < neighbors.Count; i++)
        {
            if (neighbors[i].walkable)
            {
                return neighbors[i].position;
            }
        }

        return Vector2.zero;
    }

    public Node NodeFromWorldPoint(Vector3 position)
    {
        //The percentage of the grid away from 0,0 we are.
        float percentX = (position.x + gridWorldSize.x / (float)2) / gridWorldSize.x;
        float percentY = (position.y + gridWorldSize.y / (float)2) / gridWorldSize.y;

        //Clamps the percents so that we dont get erros if our point is off the grid
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        //Get our indexes
        int x = Mathf.RoundToInt((gridXSize - 1) * percentX);
        int y = Mathf.RoundToInt((gridYSize - 1) * percentY);

        if (grid[x, y] == null)
        {
            Debug.Log("X: " + x + " Y:" + y);
        }
        return grid[x, y];
    }

	// Update is called once per frame
	void OnDrawGizmos(){
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, gridWorldSize.y));
        
        if(grid != null)
        {
            foreach(Node n in grid){
                if (!n.walkable)
                {
                    Gizmos.color = Color.red;
                }
                else if (n.penalty != 0)
                {
                    Gizmos.color = Color.blue;
                }
                else
                {
                    Gizmos.color = Color.white;
                }
                Gizmos.DrawWireCube(n.position, Vector3.one * (nodeDiameter * nodeSize));
            }
        }
    }
}
