using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpectrumBar : MonoBehaviour
{

    public Slider RedBar;
    public Slider BlueBar;

    private float maxValue = 100;
    public float currentRedValue;
    public float currentBlueValue;

    private Coroutine RegenRed;
    private Coroutine RegenBlue;

    private WaitForSeconds regenTick = new WaitForSeconds(0.1f);

    // Start is called before the first frame update
    void Start()
    {
        currentBlueValue = maxValue;
        currentRedValue = maxValue;

        RedBar.maxValue = maxValue;
        BlueBar.maxValue = maxValue;

        RedBar.value = maxValue;
        BlueBar.value = maxValue;
    }

    public void startRedBar()
    {
        StopAllCoroutines();
        StartCoroutine(RedSpectrumBar());
        StartCoroutine(RecoverBlueBar());


        if (!GameManager.instance.RedActive)
        {
           if (RegenRed != null)
                StopCoroutine(RegenRed);

            RegenRed = StartCoroutine(RecoverRedBar());
        }

    }

    public void startBlueBar()
    {
        StopAllCoroutines();
        StartCoroutine(BlueSpectrumBar());
        StartCoroutine(RecoverRedBar());

        if (!GameManager.instance.BlueActive)
        {
            if (RegenBlue != null)
                StopCoroutine(RegenBlue);

            RegenBlue = StartCoroutine(RecoverBlueBar());
        }
    }

    //decrease bar values

    public IEnumerator RedSpectrumBar()
    {
        while(GameManager.instance.RedActive && currentRedValue >= 0)
        {
            currentRedValue -= maxValue / 50;
            RedBar.value = currentRedValue;
            yield return new WaitForSeconds(0.1f);
        }
        if(currentRedValue <= 0)
        {
            GameManager.instance.RedActive = false;
            GameManager.instance.RedPanel.SetActive(false);
            StartCoroutine(RecoverRedBar());
        }
       
    }

    public IEnumerator BlueSpectrumBar()
    {
        while (GameManager.instance.BlueActive && currentBlueValue >= 0)
        {
            currentBlueValue -= maxValue / 50;
            BlueBar.value = currentBlueValue;
            yield return new WaitForSeconds(0.1f);
        }
        if(currentBlueValue <= 0)
        {
            GameManager.instance.BlueActive = false;
            GameManager.instance.BluePanel.SetActive(false);
            StartCoroutine(RecoverBlueBar());
        }
    }


    //Recover Bar Values

    private IEnumerator RecoverRedBar()
    {
        yield return new WaitForSeconds(1f);

        while(currentRedValue < maxValue)
        {
            currentRedValue += maxValue / 100;
            RedBar.value = currentRedValue;
            yield return regenTick;
        }

        RegenRed = null;
    }

    private IEnumerator RecoverBlueBar()
    {
        yield return new WaitForSeconds(1f);

        while (currentBlueValue < maxValue)
        {
            currentBlueValue += maxValue / 100;
            BlueBar.value = currentBlueValue;
            yield return regenTick;
        }

        RegenBlue = null;
    }
}
