using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.UI;
using TMPro;

public class AmmoCount : MonoBehaviour
{
    public TMP_Text ammoText;
    public TMP_Text reloadWarn;
    public GameObject weapon;
    handgun gun;
    int maxAmmo;
    int currentAmmo;

    private void Awake()
    {
        gun = weapon.GetComponent<handgun>();
        maxAmmo = gun.maxAmmo;
        currentAmmo = gun.currentAmmo;
        reloadWarn.enabled = false;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        currentAmmo = gun.currentAmmo;
        ammoText.text = currentAmmo.ToString() + "/" + maxAmmo.ToString();
        if (currentAmmo <= 5)
        {
            reloadWarn.enabled = true;
            if (currentAmmo == 0)
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
