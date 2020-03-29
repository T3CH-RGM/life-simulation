using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentsCreation : MonoBehaviour
{
    public GameObject prefab;
    public int agentsAmount;

    public List<GameObject> agents;
    int lastAgents;

    // Start is called before the first frame update
    void Start()
    {
        var rand = new Random();

        agents = new List<GameObject>();

        for (int i = 0; i < agentsAmount; i++)
        {
            float xPos = Random.Range(-9.5f, 9.5f);
            float yPos = Random.Range(-9.5f, 9.5f);
            GameObject newAgent = Instantiate(prefab, new Vector3(xPos, 0.5F, yPos), Quaternion.identity);
            newAgent.GetComponent<AgentController>().generation = i.ToString();

            agents.Add(newAgent);
        }

        StartCoroutine(stats());
    }

    // Update is called once per frame
    void Update()
    {
    }

    IEnumerator stats()
    {

        yield return new WaitForSeconds(5.0f);
        float speed = 0;
        Debug.Log("Agents alive: " + agents.Count);
        foreach (GameObject agent in agents)
        {
            speed += agent.GetComponent<AgentController>().speed;
        }
        Debug.Log("Speed avg: " + (speed / (agents.Count * 1.0f)));
        StartCoroutine(stats());
    }
}