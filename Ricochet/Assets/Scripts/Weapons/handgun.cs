using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class handgun : MonoBehaviour
{
    public GameObject bullet;
    // bullet stats
    public int ricochets;
    public int maxAmmo = 12;
    public int damage = 35;
    public int currentAmmo;
    public float velocity;

    // gun stats
    public float timeBetweenShots, timeBetweenShooting, range;

    public bool shooting, reload, readyToShoot;

    public Camera fpsCam;
    // tip of gun
    public Transform muzzle;

    public bool allowInvoke = true;

    public Animation reloadAnim;
    public Animator arms;
    public AudioClip gunshot;
    private AudioSource gunAudio;

    private void Awake()
    {
        readyToShoot = true;
        currentAmmo = maxAmmo;
    }

    void Start()
    {
        gunAudio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        MyInput();
    }

    void MyInput()
    {
        shooting = Input.GetButtonDown("Fire1");
        reload = Input.GetButtonDown("Reload");

        if (readyToShoot && shooting)
        {
            Shoot();
        }
        if (reload)
        {
            Reload();
        }
    }

    void Shoot()
    {
        if (currentAmmo == 0)
        {
            return;
        }
        readyToShoot = false;
        currentAmmo--;

        // Find where the bullet will hit (ray through middle of screen)
        Ray ray = fpsCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

        RaycastHit hit;
        Vector3 targetPoint;

        // check for hit
        if (Physics.Raycast(ray, out hit))
        {
            targetPoint = hit.point;
        } else
        {
            targetPoint = ray.GetPoint(range);
        }

        // Calculate direction between the muzzle and the targetPoint
        Vector3 directionWithoutSpread = targetPoint - muzzle.position;

        // Instantiate bullet
        GameObject currentBullet = Instantiate(bullet, muzzle.position, Quaternion.identity);
        currentBullet.GetComponent<Ricochet>().numRicochets = ricochets;
        // Rotate to shoot direction
        currentBullet.transform.forward = directionWithoutSpread.normalized;
        // Add force to bullet
        currentBullet.GetComponent<Rigidbody>().AddForce(directionWithoutSpread.normalized * velocity, ForceMode.Impulse);
        // Shoot animation
        arms.SetTrigger("Shoot");
        // Shoot sound
        gunAudio.clip = gunshot;
        gunAudio.Play();

        
        if (currentAmmo == 0)
        {
            arms.SetBool("Out Of Ammo Slider", true);
        }
        // Invoke resetShot
        if (allowInvoke)
        {
            Invoke("ResetShot", timeBetweenShots);
            allowInvoke = false;
        }
    }

    private void ResetShot()
    {
        readyToShoot = true;
        allowInvoke = true;
    }

    private void Reload()
    {
        if (currentAmmo == maxAmmo) return;
        readyToShoot = false;
        if (currentAmmo > 0)
        {
            arms.SetTrigger("Reload");
        }
        else
        {
            arms.SetTrigger("Reload Empty");
        }
    }

}
