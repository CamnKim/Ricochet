using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class PlayerHealth : MonoBehaviour
{
    public TMP_Text healthText;
    public int maxHealth = 100;
    int currentHealth;
    public bool isDead = false;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        healthText.text = currentHealth.ToString();
    }

    public void TakeDamage(int damage)
    {
        Debug.Log("Taking Damaged");
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            isDead = true;
        }
    }
}
