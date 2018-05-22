// Assignment Introduction
// This is the controller for the Face Happy steering.
// Face Happies are weird creatures, if they touch anything they die.
// You'd think this would be enough to make anyone depressed, but the Face Happies are permanently happy.
// The Face Happies want to get into their safe cave (the goal line) before they touch anything, help them do so!

// ---------------- TASK 3 -----------------------

// In this task you must tweak the variables to allow as many Face Happies as possible to safely traverse the path to their goal; personal best is 32.
// Hint: Behaviour like Alignment gives nice troop-like formations, but limit personal movement of the Face Happies.
// Write a short explanation for you choice in weights.
// Now let the lag commence! Look forward to optimization classes :)

/*using UnityEngine;
using System.Collections;

public class Steering : MonoBehaviour
{

    public float moveSpeed = 5f;
    

    public float seperationWeight = 1000.0f;
    public float cohesionWeight = 0.001f;

    public float avoidanceWeight = 1000.0f;
    public float alignmentWeight = 0.0001f;
    public float pursuitWeight;

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
        for (int i = 0; i < nearbyObjects.Length; i++)
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
        }

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

        moveDir = moveDir.normalized * moveSpeed;

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
            Destroy(col.gameObject);
        }       
    }
}*/