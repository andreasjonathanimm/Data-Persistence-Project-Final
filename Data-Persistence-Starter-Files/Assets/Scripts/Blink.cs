using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blink : MonoBehaviour
{
    public GameObject Object;
    public float time;

    public void Start()
    {
        InvokeRepeating("Blinks", time, time);
    }

    void Blinks()
    {
        Object.SetActive(!Object.activeInHierarchy);
    }

}
