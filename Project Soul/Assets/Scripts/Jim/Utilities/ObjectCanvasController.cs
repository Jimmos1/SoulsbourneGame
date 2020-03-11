using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectCanvasController : MonoBehaviour
{
    // It is all dirty please ignore
    FastStats stats;
    public Canvas canvas;
    public Slider healthSlider;

    private GoapCore ai;
    public bool toEnableCanvas = false;


    private void Awake()
    {
        stats = GetComponentInParent<IDamageable>().GetStats();

        if (transform.TryGetComponent(out ai))
        {
            healthSlider.maxValue = ai.GetCurrentHealth();
        }

    }

    private void LateUpdate()
    {
        canvas.enabled = toEnableCanvas;
        healthSlider.value = stats.health;
    }

    public void isAware()
    {
        toEnableCanvas = true;
    }

    public void isNotAware()
    {
        toEnableCanvas = false;
    }
}