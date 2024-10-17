using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shell : MonoBehaviour
{
    public GameObject explosion;
    public float speed = 0f;
    public float mass = 10;
    public float force = 2f;
    public float acceleration;
    public float yspeed = 0f;

    public float drag = 1;
    public float gravity = -9.8f;
    public float gAccel ;

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "tank")
        {
            GameObject exp = Instantiate(explosion, this.transform.position, Quaternion.identity);
            Destroy(exp, 0.5f);
            Destroy(this.gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        acceleration = force / mass;
        speed += acceleration * 1;
        gAccel = gravity / mass;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        speed *= (1- Time.deltaTime * drag);
        yspeed += gAccel * Time.deltaTime;
        this.transform.Translate(0, yspeed , speed );
    }
}
