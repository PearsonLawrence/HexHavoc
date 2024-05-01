using UnityEngine;
using UnityEngine.UI;

public class healthVignetteController : MonoBehaviour
{
    public float changeSpeed = 2f;
    public Image healthBarImage;
    public float maxHealth = 100f;
    public float maxAlpha = 255f; // Maximum alpha value of the transparency slider

    public bool test = false;
    private float currentHealth;
    public GameObject audio;
    public float currentMatchHealth, lastHealth;
    private MatchManager instanceMatch;
    public bool player1;
    void Update(){
        if(player1)
        {
            currentMatchHealth = instanceMatch.playerOneHealth.Value;
        }
        else
        {
            currentMatchHealth = instanceMatch.playerTwoHealth.Value;
        }

        if(currentMatchHealth != lastHealth && currentMatchHealth < lastHealth)
        {
            float difference = lastHealth - currentMatchHealth;

            TakeDamage(difference);
            lastHealth = currentMatchHealth;
        }
        if (currentMatchHealth == 100)
            lastHealth = 100;

        if(test){
            TakeDamage(20);
    }
        if(healthBarImage.color.a > 0){
            Color tempColor = new Color(healthBarImage.color.r, healthBarImage.color.g, healthBarImage.color.b, 0);
            healthBarImage.color = Color.Lerp(healthBarImage.color, tempColor, Time.deltaTime * changeSpeed);
        }
    }
    
    void Start()
    {
        currentHealth = maxHealth;
        instanceMatch = MatchManager.Instance;
        lastHealth = maxHealth;
    }

    public void TakeDamage(float amount)
    {
        float adjust = amount * 2.5f;
        float percentAmount = adjust / maxHealth;
        // Calculate the percentage of remaining health
        float healthPercentage = currentHealth / maxHealth;

        // Map health percentage to alpha value range (0 to 1)
        float alpha = percentAmount * (maxAlpha / 255f);
        Color tempColor = new Color(0, 0, 0, alpha);
        healthBarImage.color += (healthBarImage.color.a < 240) ? tempColor : new Color(0,0,0);

        Color clampColor = new Color(healthBarImage.color.r, healthBarImage.color.g, healthBarImage.color.b, 240);
        healthBarImage.color = (healthBarImage.color.a >= 240) ? clampColor : healthBarImage.color;
        test = false;


        GameObject temp = Instantiate(audio, Camera.main.transform.position, Quaternion.identity);
        Destroy(temp, 2);
        //currentHealth -= amount;
        //UpdateVignette();

    }

    //void UpdateVignette()
    //{
        // Calculate the percentage of remaining health
        //float healthPercentage = currentHealth / maxHealth;
        
        // Map health percentage to alpha value range (0 to 1)
        //float alpha = healthPercentage * (maxAlpha / 255f);
        
        // Adjust the alpha value based on mapped value
        //Color color = healthBarImage.color;
        //color.a = alpha;
        //healthBarImage.color = color;
    //}

   
}