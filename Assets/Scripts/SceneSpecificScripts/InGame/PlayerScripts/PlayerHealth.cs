﻿using Jerre.Events;
using UnityEngine;

namespace Jerre
{
    [RequireComponent(typeof (PlayerSettings))]
    public class PlayerHealth : MonoBehaviour
    {
        [HideInInspector]
        public int MaxHealth;

        [SerializeField]
        private int healthLeft;
        public int HealthLeft
        {
            get { return healthLeft;  }
        }

        private PlayerSettings settings;

        // Use this for initialization
        void Start()
        {
            settings = GetComponent<PlayerSettings>();
            MaxHealth = settings.MaxHealth;
            ResetHealth();
        }

        // returns true if player is killed, false otherwise
        public bool DoDamage(int damage)
        {
            healthLeft -= damage;
            if (healthLeft <= 0)
            {
                healthLeft = 0;
                Debug.Log("Player " + settings.playerNumber + " died!");
                AFEventManager.INSTANCE.PostEvent(AFEvents.HealthDamage(settings.playerNumber, damage, healthLeft));
                PlayerDeathManager.instance.RegisterPlayerDeath(settings);
                return true;
            }
            AFEventManager.INSTANCE.PostEvent(AFEvents.HealthDamage(settings.playerNumber, damage, healthLeft));
            return false;
        }

        public void ResetHealth()
        {
            healthLeft = MaxHealth;
        }
    }
}
