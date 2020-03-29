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

        addRandomFood(foodAmount);

    }

    // Update is called once per frame
    void Update()
    {
        if (GameObject.FindGameObjectsWithTag("Food").Length < foodAmount * 0.95)
        {
            addRandomFood(foodAmount - GameObject.FindGameObjectsWithTag("Food").Length);
        }
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

