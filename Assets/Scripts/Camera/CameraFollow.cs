using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {

    public Transform target;
    public float smoothing = 5f; // adds a bit of lag when the camera follows the player

    Vector3 offset;

    void Start()
    {
        offset = transform.position - target.position;

    }

    // FixedUpdate is used to follow a rigid body (same as the player's update)
    void FixedUpdate()
    {
        Vector3 targetCamPos = target.position + offset;
        transform.position = Vector3.Lerp(transform.position, targetCamPos, smoothing * Time.deltaTime); // Lerp slowly moves between 2 positions
    }

}
