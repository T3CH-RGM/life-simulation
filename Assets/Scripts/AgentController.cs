using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentController : MonoBehaviour
{
    private Rigidbody rb;
    public string id;
    public int age;

    public int daysCounter;
    public int incubationDays;
    public int diseaseDays;
    public bool isHospitalized;

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

        isHospitalized = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        int ageRangeDecision = 70;
        int ageRange = Random.Range(0, 100); // < ageRangeDecision = age < 40; >= ageRangeDecision = age >= 40 
        age = ageRange < ageRangeDecision ? Random.Range(1, 40) : Random.Range(40, 100);

        var rand = new Random();
        rb = GetComponent<Rigidbody>();

        moveX = Random.Range(0, 2) == 1; // True = X | False = Z
        moveZ = Random.Range(0, 2) == 1; // True = X | False = Z

        nextPlace = transform.position;
    }

    void FixedUpdate()
    {

        if (GetComponent<Renderer>().material.color != Color.red &&
            GetComponent<Renderer>().material.color != Color.black &&
            GetComponent<Renderer>().material.color != Color.blue)
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

            float speed = 4;
            if (FindObjectsOfType<AgentsCreation>()[0].inQuarantine) speed /= 2.0f;
            else speed = 4;

            transform.position = Vector3.MoveTowards(transform.position, nextPlace, speed * Time.deltaTime);

            moves--;
        } else {
            transform.position = Vector3.MoveTowards(transform.position, transform.position, Time.deltaTime);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Agent")
        {
            if (status == "normal" && collision.gameObject.GetComponent<Renderer>().material.color == Color.yellow)
            {
                status = "incubating";
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

    public void incubate()
    {
        GetComponent<Renderer>().material.color = Color.yellow;
    }

    public void infect()
    {
        GetComponent<Renderer>().material.color = Color.red;
        status = "infected";
        daysCounter = 0;
    }

    public void recover()
    {
        GetComponent<Renderer>().material.color = Color.blue;
        status = "recovered";
        moveToPlayground();
        daysCounter = 0;
    }

    public void moveToHospital()
    {
        isHospitalized = true;
        Vector3 agentPosition = transform.position;
        transform.position = new Vector3(agentPosition.x, agentPosition.y, agentPosition.z + 120.0f);
    }
    public void moveToPlayground()
    {
        isHospitalized = false;
        Vector3 agentPosition = transform.position;
        transform.position = new Vector3(agentPosition.x, agentPosition.y, agentPosition.z - 120.0f);
    }

    public void die()
    {
        if (!isHospitalized) moveToHospital();
        GetComponent<Renderer>().material.color = Color.black;
        status = "dead";
    }
}
