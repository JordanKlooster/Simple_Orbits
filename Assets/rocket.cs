using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class rocket : MonoBehaviour {

    public string HIn = "H1";
    public string VIn = "V1";


    //public bool initiateTheMass = false; // I don't know what this is



    // Just variables for prefab spawning bullet

    public Rigidbody2D bullet;
    public Transform sPlace; //rocket pos += angle and short distance.

    public float sepera = 2; //distance from space ship to bullet
    public float sForce = 30;
    public float smass = 0.01f;
    




    // array = array + new item


    public float rocket_strength;
	public float turn_strength;
	private Rigidbody2D rb;
	private float rot;

    public float FireRate = 0.2f;
    private float ShotTime;

    

    public GameObject Controller; // controller is physics controller
    public gravity gravityScript;

    public GameObject trail;

    public GameObject CamObj;
    public Camera cam;
    public Transform camTs;

    public bool timeBased = false;









    void Start () {
        gravityScript = Controller.GetComponent<gravity>(); // Controller is the Physics Controller GameObject
		rb = GetComponent<Rigidbody2D> ();
        trail.SetActive(false);
        sPlace = GetComponent<Transform>();

        
        ShotTime = FireRate;

        cam = CamObj.GetComponent<Camera>();
        camTs = CamObj.GetComponent<Transform>();
    }// End of Start
	








	void Update () {

        

            rot = -1 * rb.rotation / 180;       // now using rb.rotation instead of transform

            
            DragMouse();
            SpaceWarControl();



            if (Input.GetButtonDown("Reload") && Input.GetAxisRaw("Reload") > 0)
            {
                //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                SceneManager.LoadScene(0);
            }
            else if (Input.GetKeyUp(KeyCode.Escape) /*&& !Input.anyKey*/ )
            {
                Application.Quit();
            }


            cam.orthographicSize += Input.GetAxis("Mouse ScrollWheel") * -cam.orthographicSize / 4;


    }// End of Update










    
    void DragMouse(){ // spawns prefab based on click, drag, and other variables.

        // Use var and if statement to spawn dif things (if x==1 spawn ship) if I have to
        // but i want to give a function a prefab, position, and velocity to spawn it in

        // REMINDER make UI data driven, not hardcoded




        if (Input.GetMouseButtonDown(0) && Input.GetButton("Shift"))
        {
            //spawn massObject and add to list

            // instantiate prefab I think
            
            //gravityScript.rbList.Add();

        }
        else if (Input.GetMouseButtonDown(0))
        {
            //Debug.Log(Input.GetAxisRaw("Mouse X") );

            Vector3 pd = Camera.main.ScreenToWorldPoint(Input.mousePosition);   // point down
            pd.z = 0;
            gameObject.transform.position = pd; // have position when first clicked

            Debug.Log(pd.x);
        }




        if (Input.GetMouseButton(1) && Input.GetButton("Shift") )
        {
            // remove massobject from list and delete it (not sure wich one should be done first, delete or remove)
            //gravityScript.smMass.Add(SpawnBody(bullet, sPlace.position + new Vector3(sepera * Mathf.Sin(rot * Mathf.PI), sepera * Mathf.Cos(rot * Mathf.PI), 0), sPlace.rotation, rb.velocity, sPlace.up * sForce, smass, new Vector3(0.2f, 0.2f, 1)));
            try{
                Destroy(gravityScript.smMass[0]);
                gravityScript.smMass.Remove(gravityScript.smMass[0]);
            }
            catch {
                Debug.Log("deleting smMass[0] didn't work (rocket.cs>DragMouse)");
            }

        }
        else if (Input.GetMouseButton(1)) {

            camTs.position -= new Vector3( Input.GetAxisRaw("Mouse X") , Input.GetAxisRaw("Mouse Y") , 0) * cam.orthographicSize/7;// difference in movement
        }

    }   // End of DragMouse()













    // The controls for the spaceWar game
    void SpaceWarControl (){

        
        rb.AddTorque (-1 * Input.GetAxisRaw(HIn) * rb.mass * turn_strength );       // if player 1 use H1 and V1, or just use public variables // or just make spacewar for now


        ShotTime += Time.deltaTime;
        //Debug.Log(Time.deltaTime);
        //Debug.Log("ShotTime: " + ShotTime);

        
        if (Input.GetAxisRaw(VIn) > 0 && ShotTime >= FireRate)          // Up to Shoot
        {
                    // I'm using smMass so all the tiny masses don't calculate force for eachother and bog down the system
            gravityScript.smMass.Add( SpawnBody(bullet,   sPlace.position + new Vector3(sepera * Mathf.Sin(rot * Mathf.PI), sepera * Mathf.Cos(rot * Mathf.PI), 0),   sPlace.rotation, rb.velocity,   sPlace.up * sForce,     smass,    new Vector3(0.2f, 0.2f, 1)));
            //  smMass is a variable (a list actually) in gravity.cs
            //  here I'm accessing it and adding a variable to it.

            Destroy(gravityScript.smMass[0], 1.5f);


            ShotTime = 0;

            //shotInstance.tag = "smallMass";
            //doesn't need the tag, I can just add it to a list
            /*          // old system, I wanted it as a function so I could spawn other things using same code
            Rigidbody2D shotInstance;
            shotInstance = Instantiate(bullet, sPlace.position + new Vector3(sepera * Mathf.Sin(rot * Mathf.PI), sepera * Mathf.Cos(rot * Mathf.PI), 0), sPlace.rotation) as Rigidbody2D;
            shotInstance.velocity = rb.velocity;
            shotInstance.AddForce(sPlace.up * sForce);
            shotInstance.mass = smass; // sets mass of shot to 
            shotInstance.tag = "smallMass";
            shotInstance.transform.localScale = new Vector3(0.2f, 0.2f, 1); // just the bullet size
            */

        }   // End of shooting if statement



        if (Input.GetAxisRaw(VIn) < 0)              // Down to Ship Booster
        {

            if (!timeBased) {
                rb.AddForce(new Vector2( rocket_strength * rb.mass * Mathf.Sin(rot * Mathf.PI), rocket_strength * rb.mass * Mathf.Cos(rot * Mathf.PI)));
            }else {
                rb.AddForce(new Vector2(Time.deltaTime * rocket_strength * rb.mass * Mathf.Sin(rot * Mathf.PI), Time.deltaTime * rocket_strength * rb.mass * Mathf.Cos(rot * Mathf.PI)));
            }

            trail.SetActive(true);

        }
        else {
            trail.SetActive(false);
        }// End of If Statement



    }// End of SpaceWarControl











    Rigidbody2D SpawnBody(Rigidbody2D RB, Vector3 v3, Quaternion rot, Vector2 vel, Vector2 forc, float mas, Vector3 scal) {          // last int is for wich list to add body to 

        Rigidbody2D shotInstance;
        shotInstance = Instantiate(RB,v3,rot) as Rigidbody2D; // RidgidBody2D (from prefab), it's position, it's rotation
        shotInstance.velocity = vel;    // desired velocity for object
        shotInstance.AddForce(forc);    // desired

        shotInstance.mass = mas; // sets mass of shot to 
        shotInstance.transform.localScale = scal; // just the bullet size

        return (shotInstance);

    }   



}
