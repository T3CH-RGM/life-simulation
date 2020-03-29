using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class FoodCreation : MonoBehaviour
{
    public int foodAmount;
    public GameObject foodObject;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < foodAmount; i++)
        {
            float xPos = Random.Range(-9.5f, 9.5f);
            float yPos = Random.Range(-9.5f, 9.5f);
            Instantiate(foodObject, new Vector3(xPos, 0.3F, yPos), Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}

