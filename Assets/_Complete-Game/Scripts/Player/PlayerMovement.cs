using UnityEngine;
using UnitySampleAssets.CrossPlatformInput;

namespace CompleteProject
{
    public class PlayerMovement : MonoBehaviour
    {
        public float speed = 6f;            // The speed that the player will move at.


        Vector3 movement;                   // The vector to store the direction of the player's movement.
        Animator anim;                      // Reference to the animator component.
        Rigidbody playerRigidbody;          // Reference to the player's rigidbody.
        void Awake()
        {
            // Set up references.
            anim = GetComponent<Animator>();
            playerRigidbody = GetComponent<Rigidbody>();
        }


        void FixedUpdate()
        {
            // Store the input axes.           
            float h = CrossPlatformInputManager.VirtualAxisReference("LeftHorizontal").GetValue;
            float v = CrossPlatformInputManager.VirtualAxisReference("LeftVertical").GetValue;
            //Move the player around the scene.
            Move(h, v);

            // Turn the player to face the mouse cursor.
            Turning();

            // Animate the player.
            Animating(h, v);
        }


        void Move(float h, float v)
        {
            // Set the movement vector based on the axis input.
            movement.Set(h, 0f, v);

            // Normalise the movement vector and make it proportional to the speed per second.
            movement = movement.normalized * speed * Time.deltaTime;

            // Move the player to it's current position plus the movement.
            playerRigidbody.MovePosition(transform.position + movement);
        }


        void Turning()
        {
            float h = CrossPlatformInputManager.VirtualAxisReference("RightHorizontal").GetValue;
            float v = CrossPlatformInputManager.VirtualAxisReference("RightVertical").GetValue;
            Vector3 lookRotation = new Vector3(h, 0, v);
            if (h == 0 && v == 0)
            {
                lookRotation = movement;
            }
            if (lookRotation.magnitude != 0)
            {
                Quaternion newRotatation = Quaternion.LookRotation(lookRotation);

                // Set the player's rotation to this new rotation.
                playerRigidbody.MoveRotation(newRotatation);
            }
        }


        void Animating(float h, float v)
        {
            // Create a boolean that is true if either of the input axes is non-zero.
            bool walking = h != 0f || v != 0f;

            // Tell the animator whether or not the player is walking.
            anim.SetBool("IsWalking", walking);
        }
    }
}