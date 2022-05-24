using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZone : MonoBehaviour
{
    public MainManager Manager;
    public MenuManager MenuManager;
    public AudioClip defeatClip;
    private AudioSource audioSource;
    private void Start()
    {
        audioSource = GameObject.Find("GameManager").GetComponent<AudioSource>();
    }
    private void OnCollisionEnter(Collision other)
    {
        other.gameObject.SetActive(false);
        audioSource.PlayOneShot(defeatClip);
        if (Manager != null)
        {
            Manager.GameOver();
        }
        if (MenuManager != null)
        {
            MenuManager.GameOver();
        }
    }
}
