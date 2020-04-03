using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class AgentsCreation : MonoBehaviour
{
    public GameObject prefab;

    public int agentsAmount;
    public int hospitalSize;
    public int hospitalUse;

    public int generation;
    private List<int> normalList;
    private List<int> incubatingList;
    private List<int> infectedList;
    private List<int> recoveredList;
    private List<int> deadList;

    private bool finished;

    // Start is called before the first frame update
    void Start()
    {
        normalList = new List<int>();
        incubatingList = new List<int>();
        infectedList = new List<int>();
        recoveredList = new List<int>();
        deadList = new List<int>();

        hospitalUse = 0;

        finished = false;

        var rand = new Random();
        int infected = Random.Range(0, agentsAmount);

        for (int i = 0; i < agentsAmount; i++)
        {
            float xPos = Random.Range(-14.5f, 14.5f);
            float yPos = Random.Range(-14.5f, 14.5f);
            GameObject newAgent = Instantiate(prefab, new Vector3(xPos, 0.29F, yPos), Quaternion.identity);
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
        int normal = 0;
        int incubating = 0;
        int infected = 0;
        int recovered = 0;
        int dead = 0;
        int incubationDays = 0;
        int diseaseDays = 0;
        foreach (AgentController agent in FindObjectsOfType<AgentController>())
        {
            AgentController agentController = agent.GetComponent<AgentController>();
            agentController.daysCounter++;
            if (agentController.status == "normal") normal++;
            else if (agentController.status == "incubating")
            {
                incubating++;
                incubationDays += agentController.incubationDays;
                if (agent.GetComponent<Renderer>().material.color == Color.green)
                {
                    agent.GetComponent<Renderer>().material.color = Color.yellow;
                }
                else if (agentController.daysCounter == agentController.incubationDays)
                {
                    agent.GetComponent<Renderer>().material.color = Color.red;
                    agentController.status = "infected";
                    if (hospitalSize > hospitalUse)
                    {
                        agent.moveToHospital();
                        hospitalUse++;
                    }
                    agentController.daysCounter = 0;
                }
            }
            else if (agentController.status == "infected")
            {
                infected++;
                diseaseDays += agentController.diseaseDays;
                incubationDays += agentController.incubationDays;
                if (agent.isHospitalized)
                {
                    if (agentController.daysCounter == agentController.diseaseDays)
                    {
                        agent.GetComponent<Renderer>().material.color = Color.blue;
                        agentController.status = "recovered";
                        agent.moveToPlayground();
                        agentController.daysCounter = 0;
                        hospitalUse--;
                    }
                }
                else
                {
                    if (hospitalSize > hospitalUse)
                    {
                        agentController.moveToHospital();
                        hospitalUse++;
                    }
                    else
                    {
                        if (agentController.daysCounter != 0)
                        {
                            float survivalRate = Mathf.Exp(-(agentController.daysCounter - 1) / 3.0f);
                            if (Random.Range(0.0f, 1.0f) > survivalRate)
                            {
                                agentController.die();
                                dead++;
                            }
                        }
                    }
                }
            }
            else if (agentController.status == "recovered") recovered++;
            else if (agentController.status == "dead") dead++;
        }
        showStats(generation, agentsInGeneration, normal, incubating, infected, recovered, dead, incubationDays, diseaseDays);
        generation++;
        StartCoroutine(stats());
    }

    public void showStats(int generation, int agentsInGeneration, int normal, int incubating, int infected, int recovered, int dead, int incubationDays, int diseaseDays)
    {
        normalList.Add(normal);
        incubatingList.Add(incubating);
        infectedList.Add(infected);
        recoveredList.Add(recovered);
        deadList.Add(dead);
        if (recovered + normal < agentsAmount)
        {

            Debug.Log("GENERATION: " + generation);
            // Debug.Log("Agents normal: " + normal);
            // Debug.Log("Agents incubating: " + incubating);
            // Debug.Log("Agents infected: " + infected);
            // Debug.Log("Agents recovered: " + recovered);
            // Debug.Log("Agents dead: " + dead);
            // Debug.Log("Incubation days avg: " + (incubationDays * 1.0f) / (infected + incubating + recovered + dead));
            // Debug.Log("Disease days avg: " + (diseaseDays * 1.0f) / (incubating + recovered + dead));
        }
        if (!finished && recovered + normal + dead == agentsAmount)
        {
            finished = true;

            string path = Application.dataPath + "/coronavirusExpansion.txt";
            if (!File.Exists(path))
                File.WriteAllText(path, "Coronavirus data:\n");

            string content = "Nor: ";
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
            content += "Generations: " + generation + "\n";
            content += "-----------------------------\n\n";

            File.AppendAllText(path, content);

            Debug.Log("File updated");
        }
    }
}