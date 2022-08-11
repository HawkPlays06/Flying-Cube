using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Manager : MonoBehaviour
{
    public GameObject flyerPrefab;

    public Transform parent;

    public float time;
    private bool isTraining = false;
    public int populationSize = 50;
    private int generationNumber = 0;
    private int[] layers = new int[] { 10, 20, 20, 6 };
    private List<NeuralNetwork> nets;
    [HideInInspector]
    public  List<Flyer> flyerList = null;
    public Spawner spawner;

    bool allDead = false;

    public TextMeshProUGUI generationText;

    void Timer()
    {
        isTraining = false;
    }

    private void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        generationText.text = "Current Generation: "+ generationNumber;
        if (!isTraining)
        {
            if (generationNumber == 0)
            {
                InitFlyerNeuralNetworks();
            }
            else
            {
                
                nets.Sort();
                for (int i = 0; i < populationSize / 2; i++)
                {
                    nets[i] = new NeuralNetwork(nets[i+(populationSize / 2)]);
                    nets[i].Mutate();

                    nets[i + (populationSize / 2)] = new NeuralNetwork(nets[i + (populationSize/2)]);
                }

                for (int i = 0; i < populationSize; i++)
                {
                    nets[i].SetFitness(0f);
                }
            }

            generationNumber++;

            isTraining = true;
            Invoke("Timer", time);
            CreateFlyerBodies();

            if (allDead)
            {
                isTraining = true;
                CancelInvoke("Timer");
                Invoke("Timer", 2f);
                print("allDead");
                CreateFlyerBodies(); ;

            }
        }
        allDead = true;
        foreach (Flyer flyer in flyerList)
        {
            if (flyer.Moveable)
            {
                allDead = false;
                break;
            }
        }
    }

    void CreateFlyerBodies()
    {
        allDead = false;
        if (flyerList != null)
        {
            for (int i = 0; i < flyerList.Count; i++)
            {
                GameObject.Destroy(flyerList[i].gameObject);
            }
        }

        flyerList = new List<Flyer>();

        for (int i = 0; i < populationSize; i++)
        {
            float Randomness = .5f;
            Flyer flyer = ((GameObject)Instantiate(flyerPrefab, new Vector3(UnityEngine.Random.Range(-Randomness, Randomness), UnityEngine.Random.Range(-Randomness, Randomness), 0), 
                                                                flyerPrefab.transform.rotation)).GetComponent<Flyer>();
            flyer.transform.parent = parent;
            flyer.Init(nets[i]);
            flyerList.Add(flyer);
        }
        
        spawner.Clear();
        spawner.Generation();
    }

    void InitFlyerNeuralNetworks()
    {
        if (populationSize % 2 != 0)
        {
            populationSize--;
        }

        nets = new List<NeuralNetwork>();

        for (int i = 0; i < populationSize; i++)
        {
            NeuralNetwork net = new NeuralNetwork(layers);
            net.Mutate();
            nets.Add(net);
        }
    }
}
