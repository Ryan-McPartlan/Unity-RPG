using UnityEngine;
using System.Collections;

//What this does: Move the NPC to its target location. Can be thrown onto any NPC that paths. Must be passed a target. Put a 
//"Get target" function into each NPC that has this that sets some transform to thiscomponent.target.

public class PathingUnit : MonoBehaviour
{
    public Vector2 target; //TODO: Have fighter pass the vector3 instead of the transform
    public float speed;
    Vector3[] path;
    int targetIndex;
    bool requestOut = false;

    void Update()
    {
        if (path == null && !requestOut)
        {
            PathRequestManager.RequestPath(transform.position, target, OnPathFound);
            requestOut = true;
        }
    }

    public void resetPath()
    {
        StopCoroutine("FollowPath");
        path = null;
        targetIndex = 0;
        requestOut = false;
    }

    public void OnPathFound(Vector3[] newPath, bool pathSuccessful)
    {
        requestOut = false;

        if (pathSuccessful && this != null)
        {
            path = newPath;
            StopCoroutine("FollowPath");
            StartCoroutine("FollowPath");
        }
    }

    IEnumerator FollowPath()
    {
        Vector3 currentWaypoint = path[0];

        while (true)
        {
            if (transform.position == currentWaypoint)
            {
                targetIndex++;
                if (targetIndex >= path.Length)
                {
                    resetPath();
                    yield break;
                }
                currentWaypoint = path[targetIndex];
            }

            transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, speed * Time.deltaTime);
            yield return null;
        }
    }

    public void OnDrawGizmos()
    {
        if (path != null)
        {
            for (int i = targetIndex; i < path.Length; i++)
            {
                Gizmos.color = Color.black;
                Gizmos.DrawCube(path[i], Vector3.one * 0.1F);

                if (i == targetIndex)
                {
                    Gizmos.DrawLine(transform.position, path[i]);
                }
                else
                {
                    Gizmos.DrawLine(path[i - 1], path[i]);
                }
            }
        }
    }
}
