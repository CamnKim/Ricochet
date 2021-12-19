using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Gun : MonoBehaviour
{
    // bullet prefab
    public GameObject bullet;

    // bullet force
    public float shootForce, upwardForce;

    // weapon stats
    public float timeBetweenShooting, spread, reloadTime, reloadTimeEmpty, timeBetweenShots;
    public int maxAmmo, ricochets, damage;
    public bool fullAuto;

    int bulletsLeft, bulletsShot;

    bool shooting, readyToShoot, reloading;

    // references to camera and muzzle
    public Camera fpsCam;
    public Transform attackPoint;

    public bool allowInvoke = true;

    // graphics
    public GameObject muzzleFlash;
    public TMP_Text ammoText;
    public TMP_Text reloadWarn;
    public Animator armsAnim;

    // audio
    public AudioClip a_shoot;
    public AudioClip a_reload;
    public AudioClip a_reloadEmpty;
    public AudioSource gunAudio;

    private void Awake()
    {
        bulletsLeft = maxAmmo;
        readyToShoot = true;
        gunAudio = GetComponent<AudioSource>();
        reloadWarn.enabled = false;
        reloading = false;
    }

    // Update is called once per frame
    void Update()
    {
        MyInput();

        // update ammo display
        UpdateDisplay();

    }

    private void MyInput()
    {
        // check if full auto
        if (fullAuto) shooting = Input.GetButton("Fire1");
        else shooting = Input.GetButtonDown("Fire1");

        if (Input.GetButtonDown("Reload") && bulletsLeft < maxAmmo && !reloading) Reload();

        // shooting
        if (readyToShoot && shooting && !reloading && bulletsLeft > 0)
        {
            bulletsShot = 0;

            Shoot();
        }

        if (Input.GetButtonDown("Inspect")) armsAnim.SetTrigger("Inspect");
    }

    private void Shoot()
    {
        readyToShoot = false;

        // set audio clip
        gunAudio.clip = a_shoot;

        // find hit point
        Ray ray = fpsCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        // check for hit
        Vector3 targetPoint;
        if (Physics.Raycast(ray, out hit))
        {
            targetPoint = hit.point;
        }
        else
        {
            targetPoint = ray.GetPoint(100); // point in the distance
        }

        // calculate direction
        Vector3 direction = targetPoint - attackPoint.position;

        // instantiate bullet prefab
        GameObject currentBullet = Instantiate(bullet, attackPoint.position, Quaternion.identity);
        currentBullet.GetComponent<Ricochet>().numRicochets = ricochets;
        currentBullet.GetComponent<Ricochet>().damage = damage;
        currentBullet.GetComponent<Ricochet>().maxRicochet = ricochets;
        // rotate bullet to shoot direction
        currentBullet.transform.forward = direction.normalized;

        // add forces to bullet
        currentBullet.GetComponent<Rigidbody>().AddForce(direction.normalized * shootForce, ForceMode.Impulse);

        // Shoot animation
        armsAnim.SetTrigger("Shoot");

        // play shooting sound
        gunAudio.Play();
        

        // instantiate muzzle flash
        

        bulletsLeft--;
        bulletsShot++;

        if (bulletsLeft == 0)
        {
            armsAnim.SetBool("Out Of Ammo Slider", true);
        }

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
        reloading = true;
        if (bulletsLeft > 0)
        {
            Invoke("ReloadFinished", reloadTime);
            armsAnim.SetTrigger("Reload");
            gunAudio.clip = a_reload;
            gunAudio.Play();
        }
        else
        {
            Invoke("ReloadFinished", reloadTimeEmpty);
            armsAnim.SetTrigger("Reload Empty");
            gunAudio.clip = a_reloadEmpty;
            gunAudio.Play();
        }
    }
    private void ReloadFinished()
    {
        if (bulletsLeft == 0)
        {
            armsAnim.SetBool("Out Of Ammo Slider", false);
        }
        bulletsLeft = maxAmmo;
        reloading = false;
    }

    private void UpdateDisplay()
    {
        ammoText.text = bulletsLeft.ToString() + "/" + maxAmmo.ToString();
        if (bulletsLeft <= 5)
        {
            reloadWarn.enabled = true;
            if (bulletsLeft == 0)
            {
                ammoText.color = new Color(255f, 0f, 0f);
            }
            else
            {
                ammoText.color = new Color(255f, 165f, 0f);
            }
        }
        else
        {
            reloadWarn.enabled = false;
            ammoText.color = new Color(255f, 255f, 255f);
        }
    }
}
