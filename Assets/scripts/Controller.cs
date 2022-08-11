using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Controller : MonoBehaviour
{

    public Rigidbody body;

    int layerMask = 1 << 8;

    List<float> distance;

    public float Vision = 3;

    public int MultOfForce = 400;

    private void Update()
    {

        distance = UpdateRaycasts();

        // the axis to apply force should be the opposite ray
        // so for the ray that points up (distance[0]) the 
        // force should be applied to the "down" axis
        // if the string axis is a negative type set inverted
        // to true

        MoveInDirection(distance[0], "down", true);
        MoveInDirection(distance[1], "left");
        MoveInDirection(distance[2], "up");
        MoveInDirection(distance[3], "right", true);
        MoveInDirection(distance[4], "backward", true);
        MoveInDirection(distance[5], "forward");


    }

    float difference(float amount, bool IsInverted)
    {
        if (amount == 0) return 0;
        if (IsInverted) return -(amount - Vision);
        return Vision - amount;
    }


    Vector3 MoveInDirection(float amount, string Stringdirection, bool IsInverted=false)
    {
        Vector3 direction = ConvertStringToVector3(Stringdirection);
        direction.Normalize();
        Vector3 forceToAdd = difference(amount, IsInverted) * Time.deltaTime * MultOfForce * direction;
        body.AddForce(forceToAdd);
        return forceToAdd;
    }

    List<float> UpdateRaycasts()
    {
        List<float> distance = new List<float>();

        

        distance.Add(GenerateRaycast("up", Vision));       // generates Raycast that points up        [0]
        distance.Add(GenerateRaycast("right", Vision));    // generates Raycast that points right     [1]
        distance.Add(GenerateRaycast("down", Vision));     // generates Raycast that points down      [2]
        distance.Add(GenerateRaycast("left", Vision));     // generates Raycast that points left      [3]

        distance.Add(GenerateRaycast("forward", Vision));  // generates Raycast that points forward   [4]
        distance.Add(GenerateRaycast("backward", Vision)); // generates Raycast that points backward  [5]

        return distance;
    }

    float GenerateRaycast(string StringRotation, float size)
    {

        Vector3 Rotation = ConvertStringToVector3(StringRotation);

        RaycastHit hit;
        if (Physics.Raycast(transform.position, Rotation, out hit, size, layerMask))
        {
            Debug.DrawRay(transform.position, Vector3.Normalize(Rotation) * hit.distance, Color.yellow);
            return hit.distance;
        }
        else
        {
            Debug.DrawRay(transform.position, Vector3.Normalize(Rotation) * size, Color.white);
            return 2f;
        }
    }

    Vector3 ConvertStringToVector3(string String)
    {
        if (String == "up")
        {
            return new Vector3(0, 90, 0);
        }
        if (String == "right")
        {
            return new Vector3(0, 0, -90);
        }
        if (String == "down")
        {
            return new Vector3(0, -90, 0);
        }
        if (String == "left")
        {
            return new Vector3(0, 0, 90);
        }
        if (String == "forward")
        {
            return new Vector3(90, 0, 0);
        }
        if (String == "backward")
        {
            return new Vector3(-90, 0, 0);
        }
        Debug.LogError(String + " is Unknown (typo?)");
        return new Vector3 (0,0,0);
    }
}
