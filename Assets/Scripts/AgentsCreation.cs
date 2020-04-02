using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentsCreation : MonoBehaviour
{
    public GameObject prefab;
    public int agentsAmount;
    public int generation;

    // Start is called before the first frame update
    void Start()
    {
        var rand = new Random();

        for (int i = 0; i < agentsAmount; i++)
        {
            float xPos = Random.Range(-14.5f, 14.5f);
            float yPos = Random.Range(-14.5f, 14.5f);
            GameObject newAgent = Instantiate(prefab, new Vector3(xPos, 0.5F, yPos), Quaternion.identity);
            newAgent.GetComponent<AgentController>().id = i.ToString();
        }

        StartCoroutine(stats());
    }

    // Update is called once per frame
    void Update()
    {
    }

    IEnumerator stats()
    {

        yield return new WaitForSeconds(2.0f);
        float speed = 0;
        float sense = 0;
        float size = 0;
        int agentsDied = 0;
        int agentsBorn = 0;
        int agentsInGeneration = FindObjectsOfType<AgentController>().Length;
        foreach (AgentController agent in FindObjectsOfType<AgentController>())
        {
            AgentController agentController = agent.GetComponent<AgentController>();
            speed += agentController.speed;
            sense += agentController.sense;
            size += agentController.size;
            float mutationsValue = agentController.getMutationsValue();
            if (agentController.food > mutationsValue * 2)
            {
                agentController.reproduce();
                agentController.surviveGeneration();
                agentsBorn++;
            }
            else if (agentController.food >= mutationsValue)
            {
                agentController.surviveGeneration();
            }
            else
            {
                agentController.die();
                agentsDied++;
            }
        }
        showStats(generation, agentsInGeneration, agentsBorn, agentsDied, speed, sense, size);
        generation++;
        StartCoroutine(stats());
    }

    public void showStats(int generation, int agentsInGeneration, int agentsBorn, int agentsDied, float speed, float sense, float size)
    {
        Debug.Log("GENERATION: " + generation);
        Debug.Log("Agents alive: " + agentsInGeneration);
        Debug.Log("Agents born: " + agentsBorn);
        Debug.Log("Agents dead: " + agentsDied);
        Debug.Log("Speed avg: " + (speed / (agentsInGeneration * 1.0f)));
        Debug.Log("Sense avg: " + (sense / (agentsInGeneration * 1.0f)));
        Debug.Log("Size avg: " + (size / (agentsInGeneration * 1.0f)));
    }
}