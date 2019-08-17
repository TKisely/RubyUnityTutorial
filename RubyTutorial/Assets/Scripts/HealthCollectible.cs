﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthCollectible : MonoBehaviour
{

    [SerializeField] private AudioClip collectedClip;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Collected clip is a "+collectedClip.GetType());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        RubyController controller = other.GetComponent<RubyController>();

        if (controller != null)
        {
            if (controller.health < controller.maxHealth)
            {
                controller.ChangeHealth(1);
                Destroy(gameObject);

                RubyController.PlaySound(collectedClip);
                Debug.Log("PlaySound called from Collectible object");

            }
        }
    }
}
