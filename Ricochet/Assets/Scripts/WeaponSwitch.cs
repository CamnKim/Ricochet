using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSwitch : MonoBehaviour
{
    public GameObject primary;
    public GameObject secondary;
    public AudioClip a_takeOut;

    // Start is called before the first frame update

    private void Awake()
    {
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Primary"))
        {
            SwitchGun(secondary, primary);
        }
        if (Input.GetButton("Secondary"))
        {
            SwitchGun(primary, secondary);
        }
    }

    void SwitchGun(GameObject currentGun, GameObject nextGun)
    {
        currentGun.SetActive(false);
        nextGun.SetActive(true);
    }
}
