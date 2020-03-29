using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentController : MonoBehaviour
{
    private Rigidbody rb;
    public float energy;
    public float initialEnergy;
    public float speed;
    private float initialSpeed;
    public string id;
    private int children = 0;

    public int food = 0;
    private bool found_food = false;
    private Vector3 food_pos;

    // Start is called before the first frame update
    void Start()
    {
        initialEnergy = energy;
        initialSpeed = speed;
        var rand = new Random();
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (energy > 0)
        {
            if (!found_food)
            {
                int i = 0;
                int closest = -1;
                float closest_distance = Mathf.Infinity;
                Collider[] hitColliders = Physics.OverlapSphere(transform.position, 2);

                while (i < hitColliders.Length)
                {
                    if (hitColliders[i].gameObject.tag == "Food")
                    {
                        if (Vector3.Distance(transform.position, hitColliders[i].transform.position) < closest_distance)
                        {
                            closest = i;
                        }
                    }

                    i++;
                }

                if (closest != -1)
                {
                    found_food = true;
                    food_pos = hitColliders[closest].transform.position;
                }
            }


            if (found_food)
            {
                transform.position = Vector3.MoveTowards(transform.position, food_pos, speed * Time.deltaTime);
            }
            else
            {
                bool moveOrientation = Random.Range(0, 2) == 1; // True = X | False = Z
                float randomMove = Random.Range(-14.5f, 14.5f);
                Vector3 movement = moveOrientation ? new Vector3(randomMove, 0.0f, 0.0f) :
                    new Vector3(0.0f, 0.0f, randomMove);

                rb.AddForce(movement * speed);

                energy -= randomMove * Mathf.Pow(speed, 2F);
            }
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
        // newAgent.GetComponent<AgentController>().speed += Random.Range(speed - 1, speed / 2.0F);
    }

    public void die()
    {
        Destroy(gameObject);
    }
}
