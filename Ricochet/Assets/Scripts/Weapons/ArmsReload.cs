using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmsReload : MonoBehaviour
{
    public GameObject gun;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // triggers when reload animation ends
    public void ReloadEnd()
    {
        gun.GetComponent<handgun>().readyToShoot = true;
        gun.GetComponent<handgun>().currentAmmo = gun.GetComponent<handgun>().maxAmmo;
    }
    public void ReloadEmpty()
    {
        gun.GetComponent<handgun>().arms.SetBool("Out Of Ammo Slider", false);
    }

}
