using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class handgun : MonoBehaviour
{
    public GameObject bullet;
    // bullet stats
    public float velocity;

    // gun stats
    public float timeBetweenShots, timeBetweenShooting;

    bool shooting, readyToShoot;

    public Camera fpsCam;
    public Transform muzzle;

    public bool allowInvoke = true;

    private void Awake()
    {
        readyToShoot = true;
    }

    void Start()
    {
        
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
            targetPoint = ray.GetPoint(75);
        }

        // Calculate direction between the muzzle and the targetPoint
        Vector3 directionWithoutSpread = targetPoint - muzzle.position;

        // Instantiate bullet
        GameObject currentBullet = Instantiate(bullet, muzzle.position, Quaternion.identity);
        // Rotate to shoot direction
        currentBullet.transform.forward = directionWithoutSpread.normalized;
        // Add force to bullet
        currentBullet.GetComponent<Rigidbody>().AddForce(directionWithoutSpread.normalized * velocity, ForceMode.Impulse);

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
