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

    void Update(){
        if(test){
            TakeDamage(5);
    }
        if(healthBarImage. < 255){
            color.a = Mathf.Lerp(color.a, 255, Time.deltaTime * changeSpeed);
        }
    }
    
    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float amount)
    {
        float percentAmount = amount/maxHealth;
        color.a -= percentAmount;
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