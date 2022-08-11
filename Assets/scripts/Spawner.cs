using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject prefab;

    public Transform parent;

    List<GameObject> objects = new List<GameObject>();

    public float speed = 10f;

    public float size;
    public float offset;

    public float LevelDistance;

    public float distanceBetweenCells;

    int CurrentLevel = 1;

    public int MaxLevel = 10;

    // Update is called once per frame
    void Update()
    {


    }

    private void Move()
    {
        transform.position += new Vector3(LevelDistance, 0, 0);
    }

    
    public void Generation()
    {
        for (int i = 0; i < MaxLevel; i++)
        {
            for (int j = 0; j < LevelDistance / distanceBetweenCells; j++)
            {
                for (int k = 0; k < CurrentLevel; k++)
                {
                    GameObject Obstacle = Instantiate(prefab, new Vector3 (transform.position.x + offset + (j * distanceBetweenCells) + (i * LevelDistance),
                                                       Random.Range(-size, size), Random.Range(-size, size)), Quaternion.identity);    
                    Obstacle.transform.parent = parent;
                    objects.Add(Obstacle);
                }
            }
            CurrentLevel++;
        }
    }

    

    public void Clear()
    {
        foreach (GameObject obstacle in objects)
        {
            Destroy(obstacle);
        }
        objects.Clear();
        CurrentLevel = 1;

    }
}