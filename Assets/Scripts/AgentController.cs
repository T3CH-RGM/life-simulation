using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentController : MonoBehaviour
{
    private Rigidbody rb;
    public float energy;
    private float initialEnergy;
    public float speed;
    private float initialSpeed;
    public string generation;
    private int children = 0;

    // Start is called before the first frame update
    void Start()
    {
        initialEnergy = energy;
        initialSpeed = speed;
        var rand = new Random();
        rb = GetComponent<Rigidbody>();

        // Debug.Log("Agent " + generation + " born with " + initialSpeed + " speed.");
    }

    void FixedUpdate()
    {
        bool moveOrientation = Random.Range(0, 2) == 1; // True = X | False = Z
        float randomMove = Random.Range(-100f, 100f);
        Vector3 movment = moveOrientation ? new Vector3(randomMove, 0.0f, 0.0f) :
            new Vector3(0.0f, 0.0f, randomMove);

        rb.AddForce(movment * speed);

        energy -= Mathf.Pow(speed, 2F);

        if (energy <= 0)
        {
            // Debug.Log("Agent " + generation + " died but left " + children + " children.");
            FindObjectsOfType<AgentsCreation>()[0].agents.Remove(gameObject);
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Food(Clone)")
        {
            energy += initialEnergy * 0.25F;

            if (energy >= initialEnergy * 1.5F)
            {
                reproduce();
            }

            Destroy(collision.gameObject);
        }
    }

    void reproduce()
    {
        children++;
        energy /= children + 1;

        float xPos = Random.Range(-9.5f, 9.5f);
        float yPos = Random.Range(-9.5f, 9.5f);

        GameObject newAgent = Instantiate(gameObject, new Vector3(xPos, 0.5F, yPos), Quaternion.identity);
        newAgent.GetComponent<AgentController>().generation += "." + children.ToString();
        newAgent.GetComponent<AgentController>().energy = initialEnergy;
        newAgent.GetComponent<AgentController>().children = 0;
        newAgent.GetComponent<AgentController>().speed += Random.Range(speed - 1, speed / 2.0F);
        FindObjectsOfType<AgentsCreation>()[0].agents.Add(newAgent);
    }
}
