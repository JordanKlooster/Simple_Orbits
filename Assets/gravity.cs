

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class gravity : MonoBehaviour {

    public GameObject[] massObjects; //= new List<GameObject>();
    public Rigidbody2D[] rb;

    public List<Rigidbody2D> rbList ;
    public List<Rigidbody2D> smMass;            // smaller masses that don't calc force between eachother

    //public List<GameObject> massObjects = new List<GameObject>();
    //public List<Rigidbody2D> rb = new List<Rigidbody2D>();


    private float xdist;        //distance of 2 objects
	private float ydist;
	private float dist;

	private float Gforce;       //the gravitational constant that I change all the time

	public float angle;         // the angle in radians the force is applied at

	public float GravConstant;  // not really because it's a variable. I could call it G (big G)

    public bool timeBased = false;




    //General workflow

    // Start
        //get reference to all object with arrays
    // End Start

    // FixedUpdate
        // cycle through all combinations of massObjects
            // Find distance between objects based on transform.position, I should probably change to rb.position
            // calc GravForce using universal gravitation equation
            // find the angle based off distance components
            // Add force and opposite force

        // End cycling through mass Objects
    // End FixedUpdate

    
    





    void Start () {

        InitiateMass("mass");       // initiates all objects with tag mass

    }   // End of Start()
	






	void Update (){


        
        AllMass();                  // This goes through all the mass object pairs and applies gravity each time.   // I think a better way would be for AllMass to 

        if (smMass.Count >1) {  SmallMass();    }
        //REMINDER to put in a variable to change properties of gravity when doing UI

    }   // End of FixedUpdate()








    public void InitiateMass(string tag) {                      // finds current objects with tag mass in level and adds to list

        // Finding all the objects with "mass" tag and keeping them in an array
        massObjects = GameObject.FindGameObjectsWithTag(tag);
        
        // Declaring and setting these arrays to the same length as massObjects array
        rb = new Rigidbody2D[massObjects.Length];



        // Getting the rigidbody components for all the mass objects
        for (int i = 0; i < massObjects.Length; i++)
        {
            rb[i] = massObjects[i].GetComponent<Rigidbody2D>();
            
        }

        rbList = new List<Rigidbody2D>(rb);             // I just kept arrays then converted to list because it's easier for me
}








    
    public void AllMass()                           // loops through all the objects with the tag "mass" as of april 2016
    {

        for (int i = 0; i < rbList.Count - 1; i++)            // This goes through all the pairs of objects to attract eachother
        {

            for (int j = i + 1; j < rbList.Count; j++)
            {
                //Debug.Log("i: " + i + "  j: " + j + "   I: " + massObjects[i].name + "   J: " + massObjects[j].name);

                
                applyGravity(rbList[i], rbList[j]);         // goes to this function to increase score (that was a joke(because the function name is descriptive))

            }
        } // End of loops for mass objects


    }// End of AllMassObjects



    public void SmallMass()                           // loops through all the objects with the tag "mass" as of april 2016
    {

        

        for (int i = 0; i < rbList.Count; i++)              // This goes through all the pairs of objects to attract eachother
        {
            
            for (int j = 0; j < smMass.Count; j++)              // what's different for this is j just starts at zero
            {
                //Debug.Log("i: " + i + "  j: " + j + "   I: " + massObjects[i].name + "   J: " + massObjects[j].name);
                
                applyGravity(smMass[j], rbList[i]);         // goes to this function to increase score (that was a joke(because the function name is descriptive))
                
            }
        } // End of loops for mass objects


    }// End of AllMassObjects








    // Apply the gravity force using positions and masses of 2 objects
    public void applyGravity(Rigidbody2D r1, Rigidbody2D r2) {



        // The distance components between the 2 objects
        xdist = r1.position.x - r2.position.x;
        ydist = r1.position.y - r2.position.y;

        // Ppythagorean theorem to get the distance, but I keep it sqaured.
        dist = (Mathf.Pow(xdist, 2) + Mathf.Pow(ydist, 2)); // actually the distance squared

        // The magnitude of the force applied, I tried getting a thing where you can control the rate of time.
        if (!timeBased)
        {
            Gforce = Mathf.PI * (GravConstant * r1.mass * r2.mass) / dist;//dist is actually dist^2 
        }else {
            Gforce = Time.deltaTime * Mathf.PI * (GravConstant * r1.mass * r2.mass) / dist;       // this is the time based gravity
        }

        // my magic bit of code that turns vector components into radian angles where up is 0
        if (ydist >= 0)
        {
            angle = 1 + Mathf.Atan(xdist / ydist) / Mathf.PI;
        }
        else {
            angle = Mathf.Atan(xdist / ydist) / Mathf.PI;
        }


        // addforce and addforce in opposite direction for the other object
        r1.AddForce(new Vector2(Gforce * Mathf.Sin(angle * Mathf.PI), Gforce * Mathf.Cos(angle * Mathf.PI)));
        r2.AddForce(new Vector2(-1 * Gforce * Mathf.Sin(angle * Mathf.PI), -1 * Gforce * Mathf.Cos(angle * Mathf.PI)));
    }   // End of applyGravity()






}   // End of Public Class
