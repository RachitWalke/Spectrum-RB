using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    /* public Slider slider;

     public void setMaxHealth(int health)
     {
         slider.maxValue = health;
         slider.value = health;
     }

     public void setHealth(int health)
     {
         slider.value = health;
     }*/

    public Image image;


    private float _health = 1f;

    private void Update()
    {
        if (image.fillAmount > _health) image.fillAmount -= 0.01f;
    }

    public void setHealth(int health)
    {
        _health = health / 100f;
    }
}
