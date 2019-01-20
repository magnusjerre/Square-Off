﻿using Jerre.Events;
using UnityEngine;

namespace Jerre
{
    [RequireComponent(typeof (BulletSettings)), RequireComponent(typeof (Rigidbody))]
    public class BulletCollider : MonoBehaviour
    {
        BulletSettings settings;

        // Start is called before the first frame update
        void Start()
        {
            settings = GetComponent<BulletSettings>();
        }

        private void OnTriggerEnter(Collider other)
        {
            var playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                var playerSettings = other.GetComponent<PlayerSettings>();
                Debug.Log("Player " + settings.PlayerOwnerNumber + " hit player " + playerSettings.playerNumber + "! Doing damage: " + settings.Damage);
                if (playerHealth.DoDamage(settings.Damage))
                {
                    AFEventManager.INSTANCE.PostEvent(AFEvents.Kill(settings.PlayerOwnerNumber, playerSettings.playerNumber));
                }
            } else
            {
                Debug.Log("Bullet hit something else..");
            }
            Destroy(gameObject);
        }
    }
}