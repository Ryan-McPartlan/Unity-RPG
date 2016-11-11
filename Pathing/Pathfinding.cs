using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//

public class Pathfinding : MonoBehaviour {
    PathRequestManager requestManager;
    Grid grid;

	// Use this for initialization
    void Awake()
    {
        requestManager = GetComponent<PathRequestManager>();
        grid = GetComponent<Grid>();
    }

    public void StartFindPath(Vector3 startPosition, Vector3 endPosition){
        StartCoroutine(FindPath(startPosition, endPosition));
    }

    IEnumerator FindPath(Vector3 startPos, Vector3 endPos)
    {

        Vector3[] wayPoints = new Vector3[0];
        bool success = false;
        Node startNode = grid.NodeFromWorldPoint(startPos);
        Node endNode = grid.NodeFromWorldPoint(endPos);

        if (startNode == endNode)
        {
            requestManager.FinishedProcessingPath(wayPoints, success);
            yield return null;
        }
        else
        {
            Heap<Node> openSet = new Heap<Node>(grid.maxSize);
            HashSet<Node> closedSet = new HashSet<Node>();
            openSet.Add(startNode);

            while (openSet.Count > 0)
            {
                Node currentNode = openSet.RemoveFirst();
                closedSet.Add(currentNode);

                if (currentNode.gridX == endNode.gridX && currentNode.gridY == endNode.gridY)
                {
                    success = true;
                    break;
                }

                foreach (Node n in grid.getNeighbors(currentNode))
                {
                    if (!n.walkable || closedSet.Contains(n))
                    {
                        continue;
                    }

                    int newMovementCost = currentNode.gCost + 5 + n.penalty;
                    if (newMovementCost < n.gCost || !openSet.Contains(n))
                    {
                        n.gCost = newMovementCost;
                        n.hCost = getDistance(n, endNode);
                        n.parent = currentNode;
                        if (!openSet.Contains(n))
                        {
                            openSet.Add(n);
                        }
                        else
                        {
                            openSet.UpdateItem(n);
                        }
                    }
                }
            }
            yield return null;
            if (success)
            {
                wayPoints = RetracePath(startNode, endNode);
            }
            requestManager.FinishedProcessingPath(wayPoints, success);
        }
    }

    Vector3[] RetracePath(Node start, Node end)
    {
        List<Node> path = new List<Node>();
        Node currentNode = end;

        while(currentNode != start)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        Vector3[] waypoints = SimplifyPath(path);
        System.Array.Reverse(waypoints);
        return waypoints;
    }

    Vector3[] SimplifyPath(List<Node> path){
        List<Vector3> wayPoints = new List<Vector3>();
        Vector2 directionOld = Vector2.zero;
        for (int i = 1; i < path.Count; i++)
        {
            Vector2 directionNew = new Vector2(path[i-1].gridX - path[i].gridX, path[i-1].gridY - path[i].gridY);
            if (directionNew != directionOld)
            {
                wayPoints.Add(new Vector3(path[i-1].position.x, path[i-1].position.y, 0));
                wayPoints.Add(new Vector3(path[i].position.x, path[i].position.y, 0));
                directionOld = directionNew;
            }
        }
        wayPoints.Add(new Vector3(path[path.Count - 1].position.x, path[path.Count - 1].position.y, 0));

        if (wayPoints.Count == 0)
        {
            Debug.Log("Squish!");
            return new Vector3[] { new Vector3(0, 0, 0) };
        }
        return wayPoints.ToArray();
    }

    int getDistance(Node nodeA, Node nodeB)
    {
        return Mathf.Abs(nodeA.gridX - nodeB.gridX) + Mathf.Abs(nodeA.gridY - nodeB.gridY);
    }
}
