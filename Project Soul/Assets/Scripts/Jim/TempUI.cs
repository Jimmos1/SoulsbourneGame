using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TempUI : MonoBehaviour
{
    public static TempUI singleton;

    public GameObject pickupText;

    public IconImageHook quick_lh;
    public IconImageHook quick_rh;
    public IconImageHook quick_item;
    public IconImageHook quick_spell;

    public Slider healthSlider;
    public Slider healthDamageSlider;
    public Slider manaSlider;
    public Slider staminaSlider;

    float targetHealth;
    float targetMana;
    float targetStamina;

    float currentHealth;
    float currentMana;
    float currentStamina;

    private void Awake()
    {
        currentHealth = 100f;
        currentMana = 100f;
        currentStamina = 100f;

        singleton = this;
    }

    private void LateUpdate()
    {
        float delta = Time.deltaTime;

        currentHealth = Mathf.SmoothStep(currentHealth, targetHealth, delta / 0.18f);
        healthDamageSlider.value = currentHealth;

        currentMana = Mathf.SmoothStep(currentMana, targetMana, delta / 0.18f);
        manaSlider.value = currentMana;

        currentStamina = Mathf.SmoothStep(currentStamina, targetStamina, delta / 0.18f);
        staminaSlider.value = currentStamina;
    }

    public void ResetInteraction()
    {
        pickupText.SetActive(false);
    }

    public void LoadInteraction(InteractionType interactionType)
    {
        switch (interactionType)
        {
            case InteractionType.pickup:
                pickupText.SetActive(true);
                break;
            case InteractionType.talk:
                break;
            case InteractionType.open:
                break;
            default:
                break;
        }
    }

    public void UpdateSliderValues(float health, float stamina, float mana)
    {
        healthSlider.value = health;
        targetHealth = health;
        targetStamina = stamina;
        targetMana = mana;
    }

    public void UpdateQuickSlotForItem(Item targetItem, bool isLeft)
    {
        if (targetItem is WeaponItem)
        {
            if (isLeft)
            {
                quick_lh.UpdateIconHook(targetItem);
            }
            else
            {
                quick_rh.UpdateIconHook(targetItem);
            }
        }

        if (targetItem is ConsumableItem)
        {
            quick_item.UpdateIconHook(targetItem);
        }
    }
}
