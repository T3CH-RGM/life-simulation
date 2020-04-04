﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class AgentsCreation : MonoBehaviour
{
    public GameObject prefab;

    public int agentsAmount;
    public float hospitalSize;
    public float hospitalUse;

    public int generation;
    public int cases;
    public int activeCases;
    private List<int> casesList;
    private List<int> activeCasesList;
    private List<int> normalList;
    private List<int> incubatingList;
    private List<int> infectedList;
    private List<int> recoveredList;
    private List<int> deadList;

    private List<int> deadAges;
    private int deadHospital;
    private int deadPlayground;

    public int quarantinePercent;
    public bool inQuarantine;
    public bool hasBeenQuarantine;

    private bool finished;

    // Start is called before the first frame update
    void Start()
    {
        hasBeenQuarantine = false;
        inQuarantine = false;

        casesList = new List<int>();
        activeCasesList = new List<int>();
        normalList = new List<int>();
        incubatingList = new List<int>();
        infectedList = new List<int>();
        recoveredList = new List<int>();
        deadList = new List<int>();

        deadAges = new List<int>();

        cases = 0;
        hospitalUse = 0;

        finished = false;

        var rand = new Random();
        int infected = Random.Range(0, agentsAmount);

        for (int i = 0; i < agentsAmount; i++)
        {
            float xPos = Random.Range(-59.5f, 59.5f);
            float yPos = Random.Range(-59.5f, 59.5f);
            GameObject newAgent = Instantiate(prefab, new Vector3(xPos, 0.5F, yPos), Quaternion.identity);
            newAgent.GetComponent<AgentController>().id = i.ToString();
            // Status --> normal=green, incubating=yellow, infected, recovered=blue, dead=black
            if (i == infected)
            {
                newAgent.GetComponent<AgentController>().status = "incubating";
                newAgent.GetComponent<AgentController>().daysCounter = 0;
                newAgent.GetComponent<AgentController>().incubationDays = Random.Range(10, 15);
                newAgent.GetComponent<AgentController>().diseaseDays = Random.Range(14, 21);
                newAgent.GetComponent<Renderer>().material.color = Color.yellow;
            }
            else
            {
                newAgent.GetComponent<AgentController>().status = "normal";
                newAgent.GetComponent<Renderer>().material.color = Color.green;
            }
        }

        StartCoroutine(stats());
    }

    // Update is called once per frame
    void Update()
    {
    }

    IEnumerator stats()
    {
        yield return new WaitForSeconds(1.0f);
        int agentsInGeneration = FindObjectsOfType<AgentController>().Length;

        activeCases = 0;

        int normal = 0;
        int incubating = 0;
        int infected = 0;
        int recovered = 0;
        int dead = 0;
        foreach (AgentController agent in FindObjectsOfType<AgentController>())
        {
            AgentController agentController = agent.GetComponent<AgentController>();
            agentController.daysCounter++;
            if (agentController.status == "normal") normal++;
            else if (agentController.status == "incubating")
            {
                if (agent.GetComponent<Renderer>().material.color == Color.green)
                {
                    incubating++;
                    agent.incubate();
                }
                else if (agentController.daysCounter == agentController.incubationDays)
                {
                    agent.infect();
                    infected++;
                    activeCases++;
                    cases++;
                    if (hospitalUse < 99.9)
                    {
                        agent.moveToHospital();
                        hospitalUse += 1.0f / (agentsAmount * hospitalSize / 100) * 100;
                    }
                }
                else
                {
                    incubating++;
                }
            }
            else if (agentController.status == "infected")
            {
                if (agent.isHospitalized)
                {
                    if (agentController.daysCounter == agentController.diseaseDays)
                    {
                        double survivalRate = 0.8359247f + (0.9948879f - 0.8359247f) / (1.0f + Mathf.Pow((agentController.age / 78.44983f), 12.59674f)); // adding age variation to surviving chances
                        if (Random.Range(0.0f, 1.0f) > survivalRate)
                        {
                            agentController.die();
                            dead++;
                            deadAges.Add(agentController.age);
                        }
                        else
                        {
                            agent.recover();
                            recovered++;
                        }
                        hospitalUse -= 1.0f / (agentsAmount * hospitalSize / 100) * 100;
                        if (hospitalUse < 0) hospitalUse = 0;
                        activeCases--;
                    }
                    else
                    {
                        infected++;
                    }
                }
                else
                {
                    if (hospitalUse < 100)
                    {
                        infected++;
                        agentController.moveToHospital();
                        hospitalUse += 1.0f / (agentsAmount * hospitalSize / 100) * 100;
                    }
                    else
                    {
                        if (agentController.daysCounter != 0)
                        {
                            double survivalRate = Mathf.Exp(-(agentController.daysCounter - 1) / 3.0f);
                            survivalRate /= 0.8359247f + (0.9948879f - 0.8359247f) / (1.0f + Mathf.Pow((agentController.age / 78.44983f), 12.59674f)); // adding age variation to surviving chances
                            if (Random.Range(0.0f, 1.0f) > survivalRate)
                            {
                                agentController.die();
                                dead++;
                                deadAges.Add(agentController.age);
                                activeCases--;
                            }
                            else
                            {
                                infected++;
                            }
                        }
                        else
                        {
                            infected++;
                        }
                    }
                }
            }
            else if (agentController.status == "recovered")
            {
                recovered++;
            }
            else if (agentController.status == "dead")
            {
                dead++;
            }
        }
        if (infected >= agentsAmount * quarantinePercent / 100.0f && !inQuarantine && !hasBeenQuarantine)
        {
            inQuarantine = true;
            Debug.Log("Quarantine declared!!! Stay at home as much as possible.");
        }
        else if (inQuarantine && infected <= agentsAmount * (quarantinePercent * 0.3f) / 100.0f)
        {
            hasBeenQuarantine = true;
            inQuarantine = false;
            Debug.Log("No more quarantine. Live normally.");
        }
        activeCases = infected;
        showStats(generation, agentsInGeneration, normal, incubating, infected, recovered, dead, cases, activeCases);
        generation++;
        StartCoroutine(stats());
    }

    public void showStats(int generation, int agentsInGeneration, int normal, int incubating, int infected, int recovered, int dead, int cases, int activeCases)
    {
        activeCasesList.Add(activeCases);
        casesList.Add(cases);

        normalList.Add(normal);
        incubatingList.Add(incubating);
        infectedList.Add(infected);
        recoveredList.Add(recovered);
        deadList.Add(dead);
        if (recovered + normal + dead < agentsAmount)
        {
            Debug.Log("GENERATION: " + generation);
        }
        if (!finished && recovered + normal + dead == agentsAmount)
        {
            finished = true;

            string path = Application.dataPath + "/coronavirusExpansion.txt";

            File.WriteAllText(path, "Coronavirus data:\n");

            string content = "Cas: ";
            foreach (int data in casesList) content += data + ", ";
            content += "\n";
            content += "Act: ";
            foreach (int data in activeCasesList) content += data + ", ";
            content += "\n";
            content += "Nor: ";
            foreach (int data in normalList) content += data + ", ";
            content += "\n";
            content += "Inc: ";
            foreach (int data in incubatingList) content += data + ", ";
            content += "\n";
            content += "Inf: ";
            foreach (int data in infectedList) content += data + ", ";
            content += "\n";
            content += "Rec: ";
            foreach (int data in recoveredList) content += data + ", ";
            content += "\n";
            content += "Die: ";
            foreach (int data in deadList) content += data + ", ";
            content += "\n";
            content += "Dead ages: ";
            foreach (int data in deadAges) content += data + ", ";
            content += "\n";
            content += "Generations: " + generation + "\n";
            content += "-----------------------------\n\n";

            File.AppendAllText(path, content);

            Debug.Log("File updated");
        }
    }
}