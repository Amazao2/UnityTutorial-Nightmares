using UnityEngine;
using System.Collections;

public class GodMode : MonoBehaviour {

    public CapsuleCollider shield;

    void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            if( shield.enabled )
            {
                shield.enabled = false;
            }
            else
            {
                shield.enabled = true;
            }
            
        }
    }
}
