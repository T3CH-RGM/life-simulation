using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentController : MonoBehaviour
{
    private Rigidbody rb;
    public float energy;
    public float initialEnergy;
    public float speed;
    public float initialSpeed;
    public float size;
    public float initialSize;
    public float sense;
    public float initialSense;
    public string id;
    private int children = 0;

    public int food = 0;
    private bool found_food = false;
    private Vector3 food_pos;
    private bool found_threat = false;
    private Vector3 threat_pos;

    public int initalMoves = 10;
    private int moves;
    private Vector3 nextPlace;
    private bool moveOrientation;
    private bool moveX;
    private bool moveZ;

    AgentController()
    {
        initialEnergy = energy;
        initialSpeed = speed;
        initialSize = size;
        initialSense = sense;
        moves = 0;
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
        if (!found_food)
        {
            int i = 0;
            int closest = -1;
            float closest_distance = Mathf.Infinity;
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, sense);

            while (i < hitColliders.Length)
            {
                if (hitColliders[i].gameObject.tag == "Food")
                {
                    if (Vector3.Distance(transform.position, hitColliders[i].transform.position) < closest_distance)
                    {
                        closest = i;
                    }
                }
                else if (hitColliders[i].gameObject.tag == "Agent")
                {
                    if (Vector3.Distance(transform.position, hitColliders[i].transform.position) < closest_distance && hitColliders[i].gameObject.GetComponent<AgentController>().size > size)
                    {
                        closest = i;
                    }
                }

                i++;
            }

            if (closest != -1)
            {
                found_threat = false;
                if (hitColliders[closest].gameObject.tag == "Food")
                {
                    found_food = true;
                    food_pos = hitColliders[closest].transform.position;
                }
                else if (hitColliders[closest].gameObject.tag == "Agent")
                {
                    found_food = false;
                    found_threat = true;
                    threat_pos = hitColliders[closest].transform.position;
                }
            }
        }


        if (found_food)
        {
            transform.position = Vector3.MoveTowards(transform.position, food_pos, speed * 4 * Time.deltaTime);
        }
        else if (found_threat)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(-threat_pos.x, threat_pos.y, -threat_pos.z), speed * 4 * Time.deltaTime);
            found_threat = false;
        }
        else
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
                moves = initalMoves;
            }

            transform.position = Vector3.MoveTowards(transform.position, nextPlace, speed * 4 * Time.deltaTime);

            moves--;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Food")
        {
            food++;
            Destroy(collision.gameObject);
            found_food = false;

        }
        else if (collision.gameObject.tag == "Wall")
        {
            moves = 0;
        }
    }

    public void reproduce()
    {
        children++;

        float xPos = Random.Range(-14.5f, 14.5f);
        float yPos = Random.Range(-14.5f, 14.5f);
        GameObject newAgent = Instantiate(gameObject, new Vector3(xPos, 0.5F, yPos), Quaternion.identity);
        newAgent.GetComponent<AgentController>().id += "." + children.ToString();
        newAgent.GetComponent<AgentController>().energy = initialEnergy;
        newAgent.GetComponent<AgentController>().children = 0;
        newAgent.GetComponent<AgentController>().food = 0;
        newAgent.GetComponent<AgentController>().speed += Random.Range(-0.1F, 0.1F);
        if (newAgent.GetComponent<AgentController>().speed < 1)
        {
            newAgent.GetComponent<AgentController>().speed = 1;
        }
        newAgent.GetComponent<AgentController>().sense += Random.Range(-0.1F, 0.1F);
        if (newAgent.GetComponent<AgentController>().sense < 1)
        {
            newAgent.GetComponent<AgentController>().sense = 1;
        }
        newAgent.GetComponent<AgentController>().size += Random.Range(-0.1F, 0.1F);
        if (newAgent.GetComponent<AgentController>().size < 1)
        {
            newAgent.GetComponent<AgentController>().size = 1;
        }

        surviveGeneration();
    }

    public void surviveGeneration()
    {
        food = 0;
        energy = initialEnergy;
    }

    public void die()
    {
        Destroy(gameObject);
    }

    public float getMutationsValue()
    {
        float senseValue = sense - 1;
        float mutationsValue = 0.5F * Mathf.Pow(speed, 2.0F) * Mathf.Pow(size, 3.0F) + senseValue;
        return mutationsValue;
    }
}
