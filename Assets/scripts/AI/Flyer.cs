using System.Collections.Generic;
using UnityEngine;

public class Flyer : MonoBehaviour
{
    private bool initilized = false;
    
    private NeuralNetwork net;
    private Rigidbody rBody;

    public float maxSpeed = 5;

    public Material[] mats;

    public bool Moveable = true;
    bool firstRan = true;

    int layerMask = 1 << 8;
    float[] inputs;

    public float Vision = 3;

    public int MultOfForce = 400;

    private void Start()
    {
        rBody = GetComponent<Rigidbody>();
        this.GetComponent<MeshRenderer>().material = mats[0];
        //print(Moveable);
    }

    private void FixedUpdate()
    {
        if (initilized)
        {
            inputs = UpdateRaycasts();

            

            float[] output = net.FeedForward(inputs);
            
            if (Moveable)
            {
                MoveInDirection(output[0], new Vector3(0, -90, 0), true); // down
                MoveInDirection(output[1], new Vector3(0, 0, 90));        // left
                MoveInDirection(output[2], new Vector3(0, 90, 0));        // up
                MoveInDirection(output[3], new Vector3(0, 0, -90), true); // right
                MoveInDirection(output[4], new Vector3(-90, 0, 0), true); // backward
                MoveInDirection(output[5], new Vector3(90, 0, 0));        // forward

                net.SetFitness(transform.position.x);

                if (rBody.velocity.magnitude > maxSpeed)
                {
                    rBody.velocity = rBody.velocity.normalized * maxSpeed;
                }
            }
            
            if (!Moveable && firstRan)
            {
                net.SetFitness(transform.position.x);
                firstRan = false;
            }

            if (!Moveable)
            {
                this.GetComponent<MeshRenderer>().material = mats[1];
            }
        }
    }

    List<float> ReturnMax(float[] inputs)
    {
        int index = 0;
        float value = -100;

        List<float> indexAndValue = new List<float>();

        foreach (float input in inputs)
        {
            if (input > value)
            {
                value = input;
            }
        }

        indexAndValue.Add(index);
        indexAndValue.Add(value);
        return indexAndValue;
    }
        

    public void Init(NeuralNetwork net)
    {
        this.net = net;
        initilized = true;
    }


    // VVVVVVVVV Ray Casting VVVVVVVVV
    
    float GenerateRaycast(Vector3 Rotation, float size)
    {

        RaycastHit hit;
        if (Physics.Raycast(transform.position, Rotation, out hit, size, layerMask))
        {
            Debug.DrawRay(transform.position, Vector3.Normalize(Rotation) * hit.distance, Color.yellow);
            return hit.distance;
        }
        else
        {
            Debug.DrawRay(transform.position, Vector3.Normalize(Rotation) * size, Color.white);
            return 0f;
        }
    }
    float[] UpdateRaycasts()
    {
        float[] distance = new float[10];


        distance[0] = (GenerateRaycast(new Vector3( 0, 90,  0), Vision));       // [0] generates Raycast that points up       
        distance[1] = (GenerateRaycast(new Vector3( 0,  0,-90), Vision));       // [1] generates Raycast that points right     
        distance[2] = (GenerateRaycast(new Vector3( 0,-90,  0), Vision));       // [2] generates Raycast that points down      
        distance[3] = (GenerateRaycast(new Vector3( 0,  0, 90), Vision));       // [3] generates Raycast that points left      

        distance[4] = (GenerateRaycast(new Vector3( 0, 90,  0), Vision));       // [4] generates Raycast that points forward   
        distance[5] = (GenerateRaycast(new Vector3( 0,-90,  0), Vision));       // [5] generates Raycast that points backward  

        distance[6] = (GenerateRaycast(new Vector3(90, 90, 90),  Vision));      // [6] generates Raycast that points towards top left corner 
        distance[7] = (GenerateRaycast(new Vector3(90, 90,-90), Vision));       // [7] generates Raycast that points towards top right corner 
        distance[8] = (GenerateRaycast(new Vector3(90,-90, 90), Vision));       // [8] generates Raycast that points towards bottom left corner 
        distance[9] = (GenerateRaycast(new Vector3(90, 90,-90), Vision));       // [9] generates Raycast that points towards bottom right corner 

        return distance;
    }
    Vector3 MoveInDirection(float amount, Vector3 direction, bool IsInverted = false)
    {
        
        direction.Normalize();
        Vector3 forceToAdd = difference(amount, IsInverted) * Time.deltaTime * MultOfForce * direction;
        rBody.AddForce(forceToAdd);
        return forceToAdd;
    }
    float difference(float amount, bool IsInverted)
    {
        if (amount == 0) return 0;
        if (IsInverted) return -(amount - Vision);
        return Vision - amount;
    }
}