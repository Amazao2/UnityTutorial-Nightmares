using UnityEngine;
using System.Collections;

public class GodMode : MonoBehaviour {

    public CapsuleCollider shield;
    public Transform gunBarrelEnd;

    public float godlikeShotgunReload = 0.25f;
    public float godlikeRifleReload = 0.20f;

    PlayerShooting weapons;

    float defaultShotgunReload;
    float defaultRifleReload;

    void Start()
    {
        weapons = gunBarrelEnd.GetComponent<PlayerShooting>();

        defaultShotgunReload = weapons.shotgunReload;
        defaultRifleReload = weapons.shotgunReload;
    }

    void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            if( shield.enabled )
            {
                shield.enabled = false;
                weapons.shotgunReload = godlikeShotgunReload;
                weapons.rifleReload = godlikeRifleReload;
            }
            else
            {
                shield.enabled = true;
                weapons.shotgunReload = defaultShotgunReload;
                weapons.rifleReload = defaultRifleReload;
            }
            
        }
    }
}
