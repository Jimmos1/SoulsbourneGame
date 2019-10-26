using UnityEngine;

public class FootstepScript : MonoBehaviour
{
    public AudioClip footStep;
    public AudioSource audioSource;

    void Footstep() //Plays audioclip at audiosource's position
    {
        audioSource.PlayOneShot(footStep);
    }
}