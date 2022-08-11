using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    //Object Health
    public int currentHealth;
    public int maxHealth;
    
    //Health UI
    public Slider HealthBarSlider;

    // Start is called before the first frame update
    void Start()
    {
        SetupHealthBar(LevelManager.instance.HealthBarCanvas, LevelManager.instance.Camera);
        currentHealth = maxHealth;
        SetMaxHealth(maxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        SetHealth(currentHealth);

        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    void Die()
    {
        //Sound Play Death
        //Anim Play Death

        //Object Destroy
        Destroy(HealthBarSlider.gameObject,1f);
        Destroy(gameObject,1f);
    }

    public void SetupHealthBar(Canvas Canvas, Camera Camera)
    {
        HealthBarSlider.transform.SetParent(Canvas.transform);

        if(HealthBarSlider.TryGetComponent<FaceCamera>(out FaceCamera faceCamera))
        {
            faceCamera.Camera = Camera;
        }
    }

    public void SetHealth(int health)
    {
        HealthBarSlider.value = health;
    }

    public void SetMaxHealth(int health)
    {
        HealthBarSlider.maxValue = health;
        HealthBarSlider.value = health;
    }
}
