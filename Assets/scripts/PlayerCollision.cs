using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    public bool Enabled = true;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Wall" && Enabled)
        {
            gameObject.GetComponent<Flyer>().Moveable = false;
        }
    }
}
