using UnityEngine;
using System.Collections;
using UnityEngine.UI;

// A very simplistic car driving on the x-z plane.

public class Drive : MonoBehaviour
{
    public float speed = 10.0f;
    public float rotationSpeed = 100.0f;
    public GameObject Fuel;
    public float autoSpeed = 4f;
    public float autoRotate = .2f;

    bool autoPilot = false;

    void Start()
    {
        
    }

    float CalculateDistance(){
        float distance = Mathf.Sqrt(Mathf.Pow(Fuel.transform.position.x - transform.position.x,2) + Mathf.Pow(Fuel.transform.position.z - transform.position.z,2));

        Vector3 fuelPos = new Vector3(Fuel.transform.position.x , 0 , Fuel.transform.position.z);
        Vector3 tankPos = new Vector3(transform.position.x , 0 , transform.position.z);
        Vector3 tankToFuel = fuelPos - tankPos;

        float uDistance = Vector3.Distance(fuelPos,tankPos);
        Debug.Log("Distance : "+distance);
        Debug.Log("UDistance: "+uDistance);
        Debug.Log("VMagnitude "+tankToFuel.magnitude);
        Debug.Log("VsqMagnitude "+tankToFuel.sqrMagnitude);

        return distance;
    }

    void CalculateAngle() {
        Vector3 tankForward = transform.up;
        Vector3 fuelDirection = Fuel.transform.position - transform.position;

        Debug.DrawRay(this.transform.position , tankForward , Color.green, 2);
        Debug.DrawRay(this.transform.position , fuelDirection , Color.red, 2);
   
        float dot = (tankForward.x * fuelDirection.x + tankForward.y * fuelDirection.y);
        float angle = angle = Mathf.Acos(dot / (tankForward.magnitude * fuelDirection.magnitude));
    
        Debug.Log("Angle: "+ angle * Mathf.Rad2Deg);
        Debug.Log("Unity Angle: " + Vector3.Angle(tankForward,fuelDirection));

        int clockwise = 1;
        if (Cross(tankForward,fuelDirection).z < 0)
            clockwise = -1;

        if ((angle* Mathf.Rad2Deg) > 15){
            this.transform.Rotate(0,0,angle * Mathf.Rad2Deg * clockwise * autoRotate);
        }
        
    }

    Vector3 Cross(Vector3 v , Vector3 w){
        float xMulti = v.y * w.z - v.z * w.y;
        float yMulti = v.x * w.z - v.z * w.x;
        float zMulti = v.x * w.y - v.y * w.x;
        
        return ( new Vector3(xMulti,yMulti,zMulti));
    }

    void AutoPilot(){

        CalculateAngle();

        this.transform.position += this.transform.up * autoSpeed * Time.deltaTime;
        }

    void Update()
    {
        // Get the horizontal and vertical axis.
        // By default they are mapped to the arrow keys.
        // The value is in the range -1 to 1
        float translation = Input.GetAxis("Vertical") * speed;
        float rotation = Input.GetAxis("Horizontal") * rotationSpeed;

        // Make it move 10 meters per second instead of 10 meters per frame...
        translation *= Time.deltaTime;
        rotation *= Time.deltaTime;

        // Move translation along the object's z-axis
        transform.Translate(0, translation, 0);

        // Rotate around our y-axis
        transform.Rotate(0, 0, -rotation);

        if(Input.GetKeyDown(KeyCode.Space)){
            CalculateDistance();
            CalculateAngle();
        }

        if(Input.GetKeyDown(KeyCode.T)){
            autoPilot = ! autoPilot;
        }

        if (autoPilot){
            AutoPilot();
        }

        if (CalculateDistance() < 3){
            autoPilot = false;
        }
    }
}