using UnityEngine;

public class Move : MonoBehaviour {

    public GameObject goal;
    Vector3 direction;
    public float speed = 2f;

    void Start() {

        // this.transform.Translate(goal.transform.position);
        // this.transform.Translate(6,0,2);
        // direction = goal.transform.position - transform.position;
        

    }

    private void LateUpdate() {
        direction = goal.transform.position - transform.position;
        this.transform.LookAt(goal.transform.position);
        if (direction.magnitude > 2){        
            Vector3 velocity = direction.normalized * speed * Time.deltaTime;
            this.transform.position = this.transform.position + velocity;
        }
    }
}
