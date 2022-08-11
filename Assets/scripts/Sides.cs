using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sides : MonoBehaviour
{
    public Transform Top;
    public Transform back;
    public Transform front;
    public Transform Bottom;

    // Start is called before the first frame update
    void Start()
    {
        Top.localScale    = new Vector3(1000, 1, 4);
        back.localScale   = new Vector3(1000, 4, 1);
        Bottom.localScale = new Vector3(1000, 1, 4);
        front.localScale  = new Vector3(1000, 4, 1);

        Bottom.localPosition = new Vector3(0,-2.5f, 0);
        back.localPosition   = new Vector3(0, 0,    2.5f);
        Top.localPosition    = new Vector3(0, 2.5f, 0);
        front.localPosition   = new Vector3(0, 0,   -2.5f);
    }
}
