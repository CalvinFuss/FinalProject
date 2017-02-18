using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zebra : Boid {
    public GameObject check;
    public float speed;
    public GameObject startGameObject;
    public GameObject player;


    public GameObject blood;
    public GameObject bloodSplat;
    
    // Use this for initialization
    void Awake()
    {
       
    }

    void Start()
    {
        boidBehaviour(check, 10, startGameObject, "ZebraWaypoint"); // Zebra, number of Zebras, Object it moves to in order to initialise 'wander state', tag to wander to 


    }



    // Update is called once per frame
    void Update() {
        boidMove(speed, player, startGameObject,blood,bloodSplat); // Move speed, Player game object, Object it moves to when switching to 'wander state', blood particle, blood png
        
    }
    
      
    /*void OnCollisionEnter(Collision col)
    {
   
            if (col.gameObject.tag == "Player")
            {
                Instantiate(blood, transform.position, Quaternion.identity);

                Destroy(col.gameObject);
            }

        }*/

    }
    



