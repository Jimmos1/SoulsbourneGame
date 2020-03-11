using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IconImageHook : MonoBehaviour
{
    public Image targetImage;
    public Item targetItem;

    private void Start()
    {
        targetImage = GetComponent<Image>();

        if (targetImage.sprite == null)
        {
            targetImage.enabled = false;
        }
    }

    public void UpdateIconHook(Item ti)
    {
        if (ti == null)
        {
            targetImage.enabled = false;
            return;
        }

        targetItem = ti;

        if (targetItem.icon != null)
        {
            targetImage.sprite = targetItem.icon;
        }
    }

    void UpdateIcon()
    {
        targetImage.sprite = targetItem.icon;
        targetImage.enabled = true;
    }
}
