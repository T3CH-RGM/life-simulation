using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentController : MonoBehaviour
{

    private Rigidbody rb;
    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        var rand = new Random();
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        bool moveOrientation = Random.Range(0, 2) == 1; // True = X | False = Z
        float randomMove = Random.Range(-9.5f, 9.5f);
        Vector3 movment = moveOrientation ? new Vector3(randomMove, 0.0f, 0.0f):
            new Vector3(0.0f, 0.0f, randomMove);

        rb.AddForce(movment*speed);
        
    }
}
