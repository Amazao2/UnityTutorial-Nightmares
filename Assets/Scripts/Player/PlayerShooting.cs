using UnityEngine;
using System.Collections.Generic;

public class PlayerShooting : MonoBehaviour
{
    public float lineWidthOverride = 0.05f;
    public bool isShotgun = false;

    // Rifle properties
    public int rifleDamage = 20;
    public float rifleReload = 0.15f;
    public float rifleRange = 100f;

    // Shotgun Properties
    public int shotgunDamage = 1;
    public float shotgunReload = 0.5f;
    public float shotgunRange = 30f;    


    float timer;
    RaycastHit shootHit;
    int shootableMask;
    ParticleSystem gunParticles;
    List<LineRenderer> gunLines;
    LineRenderer baseLineRenderer;
    AudioSource gunAudio;
    Light gunLight;
    float effectsDisplayTime = 0.2f;


    void Awake ()
    {
        shootableMask = LayerMask.GetMask ("Shootable");
        gunParticles = GetComponent<ParticleSystem> ();
        gunLines = new List<LineRenderer>();
        baseLineRenderer = GetComponent<LineRenderer>();
        gunAudio = GetComponent<AudioSource> ();
        gunLight = GetComponent<Light> ();
    }


    void Update ()
    {
        timer += Time.deltaTime;

        // switch guns
        if ( Input.GetKeyDown(KeyCode.Alpha1) ) // rifle
            isShotgun = false;
        else if ( Input.GetKeyDown(KeyCode.Alpha2) ) // shotgun)
            isShotgun = true;

        float timeBetweenBullets;
        
        // what gun is selected?
        if (isShotgun)
            timeBetweenBullets = shotgunReload; 
        else
            timeBetweenBullets = rifleReload;

        if(Input.GetButton ("Fire1") && timer >= timeBetweenBullets)
        {
            Shoot ();
        }

        if(timer >= timeBetweenBullets * effectsDisplayTime)
        {
            DisableEffects ();
        }
    }


    public void DisableEffects ()
    {
        gunLines.ForEach(delegate(LineRenderer l)
        {
            Destroy(l.gameObject);
        });
        gunLines.Clear();
        gunLight.enabled = false;
    }


    void Shoot ()
    {
        timer = 0f;

        gunAudio.Play ();

        gunLight.enabled = true;

        gunParticles.Stop();
        gunParticles.Play();

        if (isShotgun)
        {
            ShotgunShot().ForEach(delegate(Ray r)
            {
                SingleShot(r);
            });
        }
        else
        {
            SingleShot(CreateRay(transform.forward));
        }

        
    }

    

    List<Ray> ShotgunShot()
    {
        List<Ray> bullets = new List<Ray>();

        bullets.Add(CreateRay(transform.forward));

        for (int i = -60; i <= 60; i++)
        {
            bullets.Add(CreateRay(Quaternion.AngleAxis(i/2.0f, Vector3.up) * transform.forward));
        }            

        return bullets;
    }

    void SingleShot(Ray ray)
    {
        LineRenderer gunLine = new GameObject().AddComponent<LineRenderer>();
        gunLine.enabled = true;
        gunLine.materials = baseLineRenderer.materials;
        gunLine.SetWidth(lineWidthOverride, lineWidthOverride);
        gunLine.SetPosition(0, transform.position);

        gunLines.Add(gunLine);

        float range;
        int damagePerShot;

        if (isShotgun)
        {
            range = shotgunRange;
            damagePerShot = shotgunDamage;
        }
        else
        {
            range = rifleRange;
            damagePerShot = rifleDamage;
        }

        if (Physics.Raycast(ray, out shootHit, range, shootableMask))
        {
            EnemyHealth enemyHealth = shootHit.collider.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damagePerShot, shootHit.point);
            }
            gunLine.SetPosition(1, shootHit.point);
        }
        else
        {
            gunLine.SetPosition(1, ray.origin + ray.direction * range);
        }
    }

    Ray CreateRay(Vector3 direction)
    {
        var ray = new Ray();

        ray.origin = transform.position;
        ray.direction = direction;

        return ray;
    }
}
