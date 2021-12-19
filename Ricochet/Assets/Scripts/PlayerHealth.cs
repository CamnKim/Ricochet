using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class PlayerHealth : MonoBehaviour
{
    public TMP_Text healthText;
    public TMP_Text centerText;
    public int maxHealth = 100;
    public int currentHealth;
    public bool isDead = false;
    public Image damageImage;
    public float flashSpeed = 5f;
    public Color flashColor = new Color(1f, 0f, 0f, 0.1f);
    bool damaged;
    public GameObject r_arms;
    public GameObject p_arms;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (damaged) damageImage.color = flashColor;
        else damageImage.color = Color.Lerp(damageImage.color, Color.clear, flashSpeed * Time.deltaTime);
        healthText.text = currentHealth.ToString();
        damaged = false;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }

    public void TakeDamage(int damage)
    {
        damaged = true;
        Debug.Log("Taking Damaged");
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            isDead = true;
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().isDead = true;
            centerText.text = "You Died";
            r_arms.SetActive(false);
            p_arms.SetActive(false);
        }
    }
}
