using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxDisappear : MonoBehaviour
{
    bool trigger = false;
    [SerializeField] float delayTime = 1f;
    [SerializeField] float frequencyTime = 1f;

    void Start()
    {
        InvokeRepeating("Disappear", delayTime, frequencyTime);
    }
    void Disappear()
    {
        transform.gameObject.SetActive(trigger);
        trigger = !trigger;
    }
}
