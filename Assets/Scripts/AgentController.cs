﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentController : MonoBehaviour
{
    private Rigidbody rb;
    public float energy;
    public float initialEnergy;
    public float speed;
    private float initialSpeed;
    public string id;
    private int children = 0;

    public int food = 0;

    // Start is called before the first frame update
    void Start()
    {
        initialEnergy = energy;
        initialSpeed = speed;
        var rand = new Random();
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        bool moveOrientation = Random.Range(0, 2) == 1; // True = X | False = Z
        float randomMove = Random.Range(-50f, 50f);
        Vector3 movement = moveOrientation ? new Vector3(randomMove, 0.0f, 0.0f) :
            new Vector3(0.0f, 0.0f, randomMove);

        rb.AddForce(movement * speed);

        energy -= Mathf.Pow(randomMove * speed, 2F);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Food")
        {
            food++;
            Destroy(collision.gameObject);
        }
    }

    public void moveToBorder()
    {
        bool moveOrientation = Random.Range(0, 2) == 1; // True = X | False = Z
        bool moveDirection = Random.Range(0, 2) == 1; // True = neg | False = pos

        if (moveOrientation)
            transform.position = new Vector3(moveDirection ? 14.0f : -14.0f, transform.position.y, transform.position.z);
        else
            transform.position = new Vector3(transform.position.x, transform.position.y, moveDirection ? 14.0f : -14.0f);
    }

    public void reproduce()
    {
        children++;

        float xPos = Random.Range(-14.5f, 14.5f);
        float yPos = Random.Range(-14.5f, 14.5f);

        GameObject newAgent = Instantiate(gameObject, new Vector3(xPos, 0.5F, yPos), Quaternion.identity);
        newAgent.GetComponent<AgentController>().id += "." + children.ToString();
        newAgent.GetComponent<AgentController>().energy = initialEnergy;
        newAgent.GetComponent<AgentController>().children = 0;
        newAgent.GetComponent<AgentController>().food = 0;
        newAgent.GetComponent<AgentController>().speed += Random.Range(speed - 1, speed / 2.0F);
        newAgent.GetComponent<AgentController>().moveToBorder();
    }

    public void die()
    {
        Destroy(gameObject);
    }
}
