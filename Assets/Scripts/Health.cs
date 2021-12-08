using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    private int maxHealth;
    private int currentHealth;
    [SerializeField]
    private UnityEngine.UI.Slider healthbar;

    public void SetHealth(int amount)
    {
        currentHealth = maxHealth = amount;
        OnHealthChange();
    }

    public IEnumerator TakeDamageDelayed(int amount, float delay)
    {
        yield return new WaitForSeconds(delay); 
        currentHealth = Mathf.Max(currentHealth - amount, 0);
        if (currentHealth == 0)
        {
            SetHealth(20);
        }
        OnHealthChange();
    }

    void OnHealthChange()
    {
        healthbar.value = (float)currentHealth / (float)maxHealth;
    }
    
    void Heal(int amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        OnHealthChange();
    }

    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
