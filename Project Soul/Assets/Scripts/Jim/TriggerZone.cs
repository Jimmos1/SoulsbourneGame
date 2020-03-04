using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerZone : MonoBehaviour
{
    public SoundManager.Sound sound;
    public Transform locationSource;
    bool isCreated = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isCreated)
        {
            isCreated = true;
            SoundManager.PlaySound(sound, locationSource.position, false);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //stop it please
            //SoundManager.PlaySound(sound, locationSource.position);
        }
    }
}
