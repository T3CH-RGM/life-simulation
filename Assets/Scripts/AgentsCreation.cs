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
        for(int i = 0; i < agentsAmount; i++)
        {
            Instantiate(prefab, new Vector3(0, i*1F, 0), Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
