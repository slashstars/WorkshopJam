using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Sounds : MonoBehaviour
{

    public AudioClip[] dead;
    public AudioClip[] gunFire;
    public AudioClip[] hitSound;
    public AudioClip[] hitVoice;
    public AudioClip[] jump;
    public AudioClip[] melee;

    private AudioSource audioSource;

    // Use this for initialization
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private AudioClip GetRandomClip(AudioClip[] arr)
    {
        int index = Random.Range(0, arr.Length - 1);

        return arr[index];
    }

    public void PlayDeadSound()
    {
        var clip = GetRandomClip(dead);
        audioSource.clip = clip;
        audioSource.Play();
    }
    public void PlayGunFire()
    {
        var clip = GetRandomClip(gunFire);
        audioSource.clip = clip;
        audioSource.Play();
    }
    public void PlayHitSound()
    {
        var clip = GetRandomClip(hitSound);
        audioSource.clip = clip;
        audioSource.Play();
    }
    public void PlayHitVoice()
    {
        var clip = GetRandomClip(hitVoice);
        audioSource.clip = clip;
        audioSource.Play();
    }
    public void PlayJumpSound()
    {
        var clip = GetRandomClip(jump);
        audioSource.PlayOneShot(clip, 0.5f);
    }
    public void PlayMeleeSound()
    {
        var clip = GetRandomClip(melee);
        audioSource.clip = clip;
        audioSource.Play();
    }




}
