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
        //Checking to see if there are any way points then return nothing
        if(wayPoints == null || wayPoints.Length == 0)
        {
            //Debug.LogWarning("NO WAYPOINTS ASSIGNED!");
            return;
        }

        //Setting the target way point to a way points ID  
        Transform targetWayPoint = wayPoints[currentWayPointId];
        //Setting the direction for the targets position
        Vector3 dir = (targetWayPoint.position - transform.position).normalized;
        //Setting the positions with a direction * the move speed for the AI 
        transform.position += dir * moveSpeed * Time.deltaTime;

        //Checking if the distance is less than .1
        if (Vector3.Distance(transform.position, targetWayPoint.position) < 0.1f)
        {
            //Increment by 1
            currentWayPointId++;

            //Checking to see if the enemy is at the last way point
            if(currentWayPointId >= wayPoints.Length)
            {
                //Destroy the enmy regardless (for now)
                Destroy(gameObject);
            }
        }
    }
    //Just some drawings
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
