using UnityEngine;

public class Movement : MonoBehaviour
{
    public float speed = 5;

    bool Sprint;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float y = 0;
        float z = Input.GetAxis("Vertical");

        if (Input.GetKey(KeyCode.Space))
        {
            y = 1;
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            y = -1;
        }

        if (Input.GetKeyDown(KeyCode.LeftControl)){
            speed *= 2;
        }

        if (Input.GetKeyUp(KeyCode.LeftControl)){
            speed /= 2;
        }

        Vector3 move = transform.right * x + transform.forward * z + transform.up * y;

        transform.position += move * speed * Time.deltaTime;
    }

    
}
