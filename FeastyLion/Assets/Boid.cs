using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;



public class Boid : MonoBehaviour {

    // Use this for initialization
    
    List<GameObject> listOfBoids = new List<GameObject>();  
     
    float distanceWanted;    
    Vector3 randomDestination;
    Vector3 averageCenter;
    float numberOfAnimals = 0;

    int direction;

    NavMeshAgent agent;
    Vector3 result;

    GameObject[] ZebraWaypoints;
    Connectors connector;

    Vector3 destination;
    Vector3 fleeDestination;

    bool fleeing;
    
    
    void Start()
    {
       
       // float angle = Random.Range(0,Mathf.PI *2);
        //velocity = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle));
       // boidBehaviour();
        randomDestination = new Vector3(Random.Range(-250, 250), 0, Random.Range(-250,250));
      

     
    }




	// Update is called once per frame
	void Update () {
        
       // boidMove();
    }


    public virtual void boidBehaviour(GameObject animal, int numberOfBoids, GameObject startGameObject, string tag)//  Virtual void used for inheritance. Enemy type, number of enemys to be instantiated, 
                                                                                                                       //...initial game object to move to, tag used for waypoints
    {
        for(int i = 0; i < numberOfBoids; i++) // used to create boids
        {
            
            GameObject clone;

           clone =  Instantiate(animal, new Vector3(Random.Range(50, 80), 1, Random.Range(50, 80)), Quaternion.identity); // Instantiates enemy game object in random position 
          //  GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
           // cube.transform.position = new Vector3(Random.Range(100,130),1,Random.Range(100,130));
           // cube.GetComponent<Renderer>().material.color = Color.red;
            
            listOfBoids.Add(clone); // Adds instantiated enemy to list

        }
        foreach(GameObject boid in listOfBoids)
        {
            
            // Rigidbody gameObjectsRigidBody = boid.AddComponent<Rigidbody>() as Rigidbody;
            // NavMeshAgent agent = boid.AddComponent<NavMeshAgent>();
        }


        numberOfAnimals = numberOfBoids;// used for avergae position 

       destination = startGameObject.transform.position; // sets inital destination 
        ZebraWaypoints = GameObject.FindGameObjectsWithTag(tag); // Gets game objects 
        Debug.Log(ZebraWaypoints.Length);
    }






    public virtual void boidMove(float speed, GameObject player, GameObject startGameObject, GameObject blood, GameObject bloodSplat)// boid speed, player, Gameobject to move to when reverting to wander, blood particle, blood png
    {
        
        distanceWanted = 10;// Distance wanted between gameobjects
        
   
        foreach (GameObject boids in listOfBoids.ToArray()) // To array - allows me to dynamically remove objects while the loop is running
        {
            agent = boids.GetComponent<NavMeshAgent>(); // Gets navmesh agent component 
             
            if (Vector3.Distance(boids.transform.position,destination) < 2 && fleeing == false)    // If distance betweeen waypoint and boid is below 2 units                                       
            {
                wander(boids); // move to next waypoint
                
            }

            if(Vector3.Distance(boids.transform.position, player.transform.position) < 80 && fleeing == false) // If the player come within a distance of 80 units
            {
                fleeing = true; 
              flee(player, boids); // flle from player
            }
             if (Vector3.Distance(boids.transform.position, player.transform.position) > 80&& fleeing == true) // if distance between boids and enemy is  greater than 80 units, move back to starting waypoint
            {
                fleeing = false;
                destination = startGameObject.transform.position;

            }


                //Debug.Log(Vector3.Distance(boids.transform.position, destination));
            // boids.transform.position = Vector3.Slerp(boids.transform.position, randomDestination, speed * Time.deltaTime);
            //boids.transform.eulerAngles = Vector3.Slerp(transform.eulerAngles, randomDestination, Time.deltaTime * speed);
            // agent.SetDestination(randomDestination);
          
                 
            

            
            agent.SetDestination(destination);// Move to destination - varies depending on current state
                                                                
          // result = Vector3.Slerp(boids.transform.position, averageCenter, speed);  
            boids.transform.LookAt(destination);// Rotate gaameobject to look at the player
            boids.transform.Rotate(0, 180, 0); // Correct the rotation 
        boids.transform.position = Vector3.MoveTowards(boids.transform.position, averageCenter, Time.deltaTime * speed/50f); // Move game objects centerpoint of flock
                                               //boids.GetComponent<NavMeshAgent>().SetDestination(averageCenter);


            averageCenter = new Vector3(0, 0, 0); // reset value
            
           
            foreach (GameObject relativeBoid in listOfBoids.ToArray())// compares boids to each other
            {
                float distance = Vector3.Distance(boids.transform.position, relativeBoid.transform.position); // Gets distances between a boids and all its neighbours

               
                

                if (distance > 0 && distance < distanceWanted ) // If the distance between that boid is less than the distance wanted
                {
                    
                    averageCenter = averageCenter + boids.transform.position;
                    
                    Vector3 difference = relativeBoid.transform.position - boids.transform.position; // Gets the difference in position 
                    difference.y = 0; // sets y axis to 0 as we do not want to adress that axis

                    difference = difference.normalized; // Normalise difference

                    boids.GetComponent<Rigidbody>().AddForce(-difference * 10, ForceMode.Force);// add force in the opposite direction to where the boids are moving towards. 
                                       
                }                               
            }
            averageCenter = averageCenter / numberOfAnimals;// sets average center value
            DestroyPrey(boids, player, blood, bloodSplat);// destroy boid function
                 
        }               
    }

    void chooseRandomPoint(GameObject boids)// This funciton choose a random point on the map - Used for initial boid movement
    {
        Vector3 randomDestinations = new Vector3(Random.insideUnitSphere.x * 50, transform.position.y, Random.insideUnitSphere.z * 50); // Created a unti sphere with origin at 0 (centerpoint)

        randomDestinations += boids.transform.position; // added all boid positions to value
        NavMeshHit hit;
        NavMesh.SamplePosition(randomDestinations, out hit, 1000, 1); // Found a walkable navmesh position within that unit sphere
        randomDestination = hit.position;// Sets desitnation value 
    }
   

    void wander(GameObject boids)
    {              
            foreach (GameObject waypoint in ZebraWaypoints) // Wander script
            {
          
            if (waypoint.transform.position == destination) { // if the destination of waypoint is equal 

                connector = waypoint.GetComponent<Connectors>();  // Gets waypoint script component each of which has a list of waypoints that the current waypoint is connected to                             
                destination = connector.connectors[Random.Range(0, connector.connectors.Count-1)].transform.position; // Chooses random connected waypoint from current wapoint script

            }
        }      
    }


    void flee(GameObject player, GameObject boids) // Flee function
    {
        int multiplyBy = 100;
        boids.transform.rotation = Quaternion.LookRotation(boids.transform.position - player.transform.position);// Turns away from player position

        Vector3 runTo = boids.transform.position + player.transform.forward * multiplyBy; // sets a position 100 units in the direction to where the player is facing

        NavMeshHit hit;
        NavMesh.SamplePosition(runTo, out hit, 1000, 1 << NavMesh.GetAreaFromName("Walkable")); // choose a walkable position within a certain distance of the boids

       destination = hit.position;

        

    }

    void DestroyPrey(GameObject boid,  GameObject player, GameObject blood, GameObject bloodSplat)
    {
        if(Vector3.Distance(boid.transform.position, player.transform.position) < 5)// Is distance between boid and player is less that 5 units. 
        {
            
          
            Instantiate(blood, new Vector3(boid.transform.position.x, boid.transform.position.y + 10, boid.transform.position.z), Quaternion.identity);// Instantiate blood particle effect
            Instantiate(bloodSplat, new Vector3(boid.transform.position.x, 0.01f, boid.transform.position.z), Quaternion.Euler(0, Random.Range(0f, 360f), 0));// Instantiate bloodsplat particle effect
            listOfBoids.Remove(boid); // Remove boid from list
            Destroy(boid); // destroy boid
        }
    }




    void OnDrawGizmos()
    {
        foreach(GameObject boid in listOfBoids)
        {
            Gizmos.DrawLine(boid.transform.position, randomDestination);
        }
    }
}
