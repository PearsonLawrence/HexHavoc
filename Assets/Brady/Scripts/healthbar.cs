//written by braden turner
//a script to change the healthbar on the jumbotron to reflect the players health in match
using Unity.Netcode;
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
        //UpdateHealthBarClientRpc();

        currentHealth = maxHealth;
    }

    private void Update()
    {
        MatchManager matchManager = MatchManager.Instance;
        if (matchManager != null)
        {
            if (playerOneHealthBar)
            {
                float healthPercentage = matchManager.playerOneHealth.Value / maxHealth;
                //Debug.Log(matchManager.playerOneHealth.Value);
                if (healthBar != null)
                {
                    //Debug.Log("BAR PERCENT : " + healthPercentage);
                    healthBar.fillAmount = healthPercentage;
                    Color healthColor = Color.Lerp(Color.red, Color.green, healthPercentage);
                    healthBar.color = healthColor;
                }
                else
                {
                    //Debug.LogError("healthBar is null.");
                }
            }
            else
            {
                //Debug.LogError("Player one health bar or player one health is null.");
            }

            if (playerTwoHealthBar)
            {
                float healthPercentage = matchManager.playerTwoHealth.Value / maxHealth;
                if (healthBar != null)
                {
                    healthBar.fillAmount = healthPercentage;
                    Color healthColor = Color.Lerp(Color.red, Color.green, healthPercentage);
                    healthBar.color = healthColor;
                }
                else
                {
                    //Debug.LogError("healthBar is null.");
                }
            }
            else
            {
                //Debug.LogError("Player two health bar or player two health is null.");
            }
        }
        else
        {
            //Debug.LogError("MatchManager is null.");
        }
    }


    /*
    [ServerRpc]
    public void UpdateHealthBarServerRpc()
    {

        if (playerOneHealthBar)
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

    [ClientRpc]
    public void UpdateHealthBarClientRpc()
    {
        Debug.Log("in health bar update");
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
