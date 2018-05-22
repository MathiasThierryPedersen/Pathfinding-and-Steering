using UnityEngine;
using System.Collections;

public class Unit : MonoBehaviour
{
    //public Transform target;
    private Vector3 position;

    float speed = 5;
    Vector3[] path;
    int targetIndex;

    
   void Update() {
        if (Input.GetMouseButton(0))
        {
            locatePosition();
        }

        
        }
    void locatePosition()
    {
        RaycastHit hitt;
        Ray rayy = Camera.main.ScreenPointToRay(Input.mousePosition);

        if(Physics.Raycast(rayy, out hitt, 1000))
        {
            position = new Vector3(hitt.point.x, hitt.point.y, hitt.point.z);
PathRequestManager.RequestPath(transform.position, position, OnPathFound);
        }
    }
   
    
 

    public void OnPathFound(Vector3[] newPath, bool pathSuccessful)
    {
        if (pathSuccessful)
        {
            path = newPath;
            targetIndex = 0;
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
                Gizmos.DrawCube(path[i], Vector3.one);

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
    public float seperationWeight = 1000.0f;
    public float cohesionWeight = 0.001f;

    public float avoidanceWeight = 1000.0f;
    public float alignmentWeight = 0.0001f;
    public float pursuitWeight;
    public float moveSpeed = 5f;

    // Fixed Update is used to remove random factor.
    void FixedUpdate()
    {
        //Setting up a start move vector, this is the one we will be adding to along the lines.
        Vector3 moveDir = Vector3.zero;

        // We add the seek vector to the combined steering vector.


        //We want to stick close to other Face Happies, because the Face Happy is a social creature.
        //Fancy name for this is "Cohesion" and the average position of the neighbours is used for this.
        Vector3 averageNeighbourPosition = Vector3.zero;
        Vector3 averageNeighbourDirection = Vector3.zero;
        int numberOfNearbyFaceHappies = 0;

        Collider[] nearbyObjects = Physics.OverlapSphere(transform.position, 15);
       /* for (int i = 0; i < nearbyObjects.Length; i++)
        {
            // We check for fellow Face Happies.
            if (nearbyObjects[i].gameObject.name.Contains("Unit") && nearbyObjects[i] != GetComponent<Collider>())
            {
                //We add up all the positions of nearby Face Happies to do proper cohesion.
                averageNeighbourPosition += nearbyObjects[i].transform.position;
                numberOfNearbyFaceHappies++;

                //We add up all the directions of nearby Face Happies to do proper alignment.
                averageNeighbourDirection += nearbyObjects[i].transform.forward;

                //We add a vector moving away from each nearby neighbour.
                Vector3 offSet = nearbyObjects[i].transform.position - transform.position;
                moveDir += (offSet / -offSet.sqrMagnitude) * seperationWeight;
            }
        }*/

        if (numberOfNearbyFaceHappies > 0)
        {
            //We divide by total number of Face Happies to get average position and direction.
            averageNeighbourPosition /= numberOfNearbyFaceHappies;
            averageNeighbourDirection /= numberOfNearbyFaceHappies;

            moveDir += (averageNeighbourPosition - transform.position).normalized * cohesionWeight;
            moveDir += averageNeighbourDirection.normalized * alignmentWeight;
       }

        RaycastHit hitInfo;
        //We check 5 length units in front of us for collisions, if one occur we take the necessary steps.
        if (Physics.SphereCast(new Ray(transform.position + moveDir.normalized * 1.6f, moveDir), 1f, out hitInfo, 3))
        {
            if (hitInfo.transform.gameObject.name.Contains("Unit"))
            {
                Vector3 vectorToCenterOfObstacle = hitInfo.transform.position - transform.position;
                moveDir -= Vector3.Project(vectorToCenterOfObstacle, transform.right).normalized * (1f / vectorToCenterOfObstacle.magnitude) * avoidanceWeight;
            }
        }

        //moveDir = moveDir.normalized * moveSpeed;

        //Time.fixedDeltaTime is multiplied with the movement to make the movement frame dependant, meaning that cases with higher frame rate
        //won't experience faster agents.
        transform.position += moveDir * Time.fixedDeltaTime;

        //Make sure he faces the right way.
        if (moveDir != Vector3.zero)
            transform.forward = moveDir;

        //Safety to ensure that they don't leave earth, this is needed due to small calculation-mistakes' tendency to add up.
       transform.position = new Vector3(transform.position.x, 0, transform.position.z);
 
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.name.Contains("Unit"))
        {
            GetComponent<Renderer>().material.color = Color.red;
        }
    } 

}