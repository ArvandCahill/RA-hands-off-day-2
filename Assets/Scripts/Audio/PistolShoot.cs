using UnityEngine;

public class PistolShoot : MonoBehaviour
{
    [Header("Audio Settings")]
    public AudioSource audioSource;
    public AudioClip pistolShootSound;
    void Update()
    {
    
        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        if (audioSource != null && pistolShootSound != null)
        {
           
            audioSource.PlayOneShot(pistolShootSound);
        }
        else
        {
            Debug.LogWarning("AudioSource atau AudioClip belum diisi di Inspector!");
        }

        Debug.Log("Pistol menembak!");
    }
}
