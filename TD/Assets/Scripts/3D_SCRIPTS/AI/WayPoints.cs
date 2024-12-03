using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPoints : MonoBehaviour
{
    public Transform[] wayPoints;
    public float moveSpeed = 2f;
    private int currentWayPointId = 0;

    // Update is called once per frame
    void Update()
    {
        if(wayPoints == null || wayPoints.Length == 0)
        {
            //Debug.LogWarning("NO WAYPOINTS ASSIGNED!");
            return;
        }

        Transform targetWayPoint = wayPoints[currentWayPointId];
        Vector3 dir = (targetWayPoint.position - transform.position).normalized;
        transform.position += dir * moveSpeed * Time.deltaTime;

        if (Vector3.Distance(transform.position, targetWayPoint.position) < 0.1f)
        {
            currentWayPointId++;

            if(currentWayPointId >= wayPoints.Length)
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnDrawGizmo()
    {
        if(wayPoints == null || wayPoints.Length == 0)
        {
            return;
        }

        Gizmos.color = Color.green;

        for(int i = 0; i < wayPoints.Length -1; i++)
        {
            Gizmos.DrawLine(wayPoints[i].position, wayPoints[i + 1].position);
        }

    }  

}
