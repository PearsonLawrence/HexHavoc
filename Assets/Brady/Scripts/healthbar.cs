//Braden Turner
//This script will show a visual health bar fot the player to see during combat and update as they take damage

using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;

    public Image healthBar;

    [SerializeField] private bool playerOneHealthBar;
    [SerializeField] private bool playerTwoHealthBar;

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthBar();
    }

    public void UpdateHealthBar()
    {
        if(playerOneHealthBar) 
        {
            float healthPercentage = MatchManager.Instance.playerOneHealth.Value / maxHealth;
            healthBar.fillAmount = healthPercentage;
            Color healthColor = Color.Lerp(Color.red, Color.green, healthPercentage);
           healthBar.color = healthColor;
        }

        if (playerTwoHealthBar)
        {
            float healthPercentage = MatchManager.Instance.playerTwoHealth.Value / maxHealth;
            healthBar.fillAmount = healthPercentage;
            Color healthColor = Color.Lerp(Color.red, Color.green, healthPercentage);
            healthBar.color = healthColor;
        }
        
    }

    /*public void TakeDamage(float damageAmount)
    {
        currentHealth -= damageAmount;

        // Ensure health doesn't go below 0
        currentHealth = Mathf.Max(currentHealth, 0f);

        UpdateHealthBar();

        if (currentHealth <= 0)
        {
            // Player is dead
            Debug.Log("Player is dead!");
        }
    }

    
    public void Heal(float healAmount)
    {
        currentHealth += healAmount;

        // Ensure health doesn't exceed the maximum
        currentHealth = Mathf.Min(currentHealth, maxHealth);

        UpdateHealthBar();
    }*/
}
