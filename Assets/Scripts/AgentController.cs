using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentController : MonoBehaviour
{
    private Rigidbody rb;
    public string id;
    public int daysCounter;
    public int incubationDays;
    public int diseaseDays;

    public int changeMoves;
    private int moves;
    private Vector3 nextPlace;
    private bool moveOrientation;
    private bool moveX;
    private bool moveZ;
    public string status;

    AgentController()
    {
        changeMoves = 10;
        moves = 0;
        daysCounter = 0;
    }

    // Start is called before the first frame update
    void Start()
    {
        var rand = new Random();
        rb = GetComponent<Rigidbody>();

        moveX = Random.Range(0, 2) == 1; // True = X | False = Z
        moveZ = Random.Range(0, 2) == 1; // True = X | False = Z

        nextPlace = transform.position;
    }

    void FixedUpdate()
    {
        if (moves == 0)
        {
            moveOrientation = Random.Range(0, 2) == 1; // True = X | False = Z
            if (moveOrientation) moveX = Random.Range(0, 2) == 1; // True = X | False = -X
            else moveZ = Random.Range(0, 2) == 1; // True = Z | False = -Z

            float randomMoveX = moveX ? Random.Range(0.0f, 5.0f) : -Random.Range(0.0f, 5.0f);
            float randomMoveZ = moveZ ? Random.Range(0.0f, 5.0f) : -Random.Range(0.0f, 5.0f);
            Vector3 movement = new Vector3(randomMoveX, 0.0f, randomMoveZ);

            nextPlace = transform.position + movement;
            moves = changeMoves;
        }

        transform.position = Vector3.MoveTowards(transform.position, nextPlace, 4 * Time.deltaTime);

        moves--;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Agent")
        {
            if (status == "normal" && collision.gameObject.GetComponent<Renderer>().material.color == Color.yellow)
            {
                gameObject.GetComponent<AgentController>().status = "incubating";
                daysCounter = -1;
                incubationDays = Random.Range(10, 15);
                diseaseDays = Random.Range(14, 21);
            }
        }
        else if (collision.gameObject.tag == "Wall")
        {
            moves = 0;
        }
    }

    public void moveToHospital() {
        Vector3 agentPosition = transform.position;
        transform.position = new Vector3(agentPosition.x, agentPosition.y, agentPosition.z + 30.0f);
    }
    public void moveToPlayground() {
        Vector3 agentPosition = transform.position;
        transform.position = new Vector3(agentPosition.x, agentPosition.y, agentPosition.z - 30.0f);
    }

    public void die()
    {
        Destroy(gameObject);
    }
}
