using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class FoodCreation : MonoBehaviour
{
    public int foodAmount;
    public GameObject foodObject;
    private int initialAgents;
    // Start is called before the first frame update
    void Start()
    {
        initialAgents = FindObjectsOfType<AgentsCreation>()[0].agentsAmount;
        addRandomFood(foodAmount);
        StartCoroutine(refillFood());
    }

    // Update is called once per frame
    void Update()
    {
        /* int actualAgents = FindObjectsOfType<AgentController>().Length;
        if (GameObject.FindGameObjectsWithTag("Food").Length < foodAmount * ((initialAgents * 1.0F) / actualAgents))
        {
            addRandomFood(foodAmount - GameObject.FindGameObjectsWithTag("Food").Length);
        } */
    }

    IEnumerator refillFood()
    {

        yield return new WaitForSeconds(2.0f);
        addRandomFood(foodAmount - GameObject.FindGameObjectsWithTag("Food").Length);
        StartCoroutine(refillFood());
    }

    void addRandomFood(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            float xPos = Random.Range(-14.5f, 14.5f);
            float yPos = Random.Range(-14.5f, 14.5f);
            Instantiate(foodObject, new Vector3(xPos, 0.3F, yPos), Quaternion.identity);
        }
    }

}

