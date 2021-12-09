using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class handgun : MonoBehaviour
{
    public GameObject bullet;
    // bullet stats
    public int ricochets;
    public float velocity;

    // gun stats
    public float timeBetweenShots, timeBetweenShooting, range;

    bool shooting, readyToShoot;

    public Camera fpsCam;
    // tip of gun
    public Transform muzzle;

    public bool allowInvoke = true;
    
    public Animator arms;
    public AudioClip gunshot;
    private AudioSource gunAudio;
    private void Awake()
    {
        readyToShoot = true;
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

        if (readyToShoot && shooting)
        {
            Shoot();
        }
    }

    void Shoot()
    {
        readyToShoot = false;

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
}
