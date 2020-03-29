﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentsCreation : MonoBehaviour
{
    public GameObject prefab;
    public int agentsAmount;

    // Start is called before the first frame update
    void Start()
    {
        var rand = new Random();

        for (int i = 0; i < agentsAmount; i++)
        {
            float xPos = Random.Range(-9.5f, 9.5f);
            float yPos = Random.Range(-9.5f, 9.5f);
            GameObject newAgent = Instantiate(prefab, new Vector3(xPos, 0.5F, yPos), Quaternion.identity);
            newAgent.GetComponent<AgentController>().id = i.ToString();
            newAgent.GetComponent<AgentController>().moveToBorder();
        }

        StartCoroutine(stats());
    }

    // Update is called once per frame
    void Update()
    {
    }

    IEnumerator stats()
    {

        yield return new WaitForSeconds(10.0f);
        float speed = 0;
        int agentsDied = 0;
        int agentsBorn = 0;
        int agentsInGeneration = FindObjectsOfType<AgentController>().Length;
        Debug.Log("Agents alive: " + agentsInGeneration);
        foreach (AgentController agent in FindObjectsOfType<AgentController>())
        {
            AgentController agentController = agent.GetComponent<AgentController>();
            speed += agentController.speed;
            if (agentController.food == 0)
            {
                agentController.die();
                agentsDied++;
            }
            else if (agentController.food >= 2)
            {
                agentController.reproduce();
                agentController.food = 0;
                agentController.energy = agentController.initialEnergy;
                agentsBorn++;
                agentController.moveToBorder();
            }
            else
            {
                agentController.food = 0;
                agentController.energy = agentController.initialEnergy;
                agentController.moveToBorder();
            }
        }
        Debug.Log("Speed avg: " + (speed / (agentsInGeneration * 1.0f)));
        Debug.Log("Agents born: " + agentsBorn);
        Debug.Log("Agents dead: " + agentsDied);
        StartCoroutine(stats());
    }
}