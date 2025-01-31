using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        // Check if the player collides with the wall (or any object with a tag "Wall")
        if (collision.gameObject.CompareTag("Wall"))
        {
            // Handle what happens when the player hits the wall
            Debug.Log("Player collided with the wall!");
            // You can stop movement or apply a force to bounce the player back, etc.
        }
    }
}

