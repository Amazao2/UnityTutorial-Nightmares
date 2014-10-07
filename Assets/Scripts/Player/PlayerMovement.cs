using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 6f;

    Vector3 movement;
    Animator anim;
    Rigidbody playerRigidBody;
    int floorMask;
    float camRayLength = 100f;

    void Awake()
    {
        floorMask = LayerMask.GetMask("Floor");
        anim = GetComponent<Animator>();
        playerRigidBody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Move(h, v);
        Turning();
        Animating(h, v);
    }

    void Move(float h, float v)
    {
        movement.Set(h, 0f, v); // no vertical motion

        movement = movement.normalized * // normalized means regardless of direction the player moves the same distance
            speed * 
            Time.deltaTime; // time between each update call
 
        playerRigidBody.MovePosition(transform.position + movement); // current position + movement vector
    }

    void Turning()
    {
        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit floorHit;

        // cast a ray from a point on the screen (mouse position) to a point on the game plane
        if (Physics.Raycast(camRay, out floorHit, camRayLength, floorMask)) // dod the ray hit something?
        {
            Vector3 playerToMouse = floorHit.point - transform.position;
            playerToMouse.y = 0f; // prevent the player from rotating back if vector has y component (it shouldn't)

            Quaternion newRotation = Quaternion.LookRotation(playerToMouse);
            playerRigidBody.MoveRotation(newRotation);
        }
    }

    void Animating(float h, float v)
    {
        bool isWalking = h != 0f || v != 0f;
        anim.SetBool("IsWalking", isWalking); // string reference to a variable? Can I refer to this variable directly?
    }

}
