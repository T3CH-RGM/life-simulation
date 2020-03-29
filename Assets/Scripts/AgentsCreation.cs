using System.Collections;
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
            newAgent.GetComponent<AgentController>().generation = i.ToString();
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
}
