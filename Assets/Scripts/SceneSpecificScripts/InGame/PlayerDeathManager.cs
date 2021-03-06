﻿using UnityEngine;
using System.Collections;
using Jerre.Events;
using Jerre.JPhysics;
using Jerre.JColliders;

namespace Jerre
{
    public class PlayerDeathManager : MonoBehaviour
    {

        public static PlayerDeathManager instance;

        public float RespawnTime = 2f;

        public ParticleSystem playerDeathExplosionPrefab;

        private PlayerSpawnManager spawnPointManager;

        private void Awake()
        {
            spawnPointManager = GameObject.FindObjectOfType<PlayerSpawnManager>();
            instance = this;
        }

        // Use this for initialization
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void RegisterPlayerDeath(PlayerSettings playerSettings)
        {
            CreateExplosion(playerSettings);
            EnableOrDisablePlayer(playerSettings, false);
            StartCoroutine(RespawnPlayer(playerSettings.playerNumber, RespawnTime));
        }

        private void EnableOrDisablePlayer(PlayerSettings playerSettings, bool enabled)
        {
            playerSettings.GetComponent<PlayerHealth>().enabled = enabled;
            playerSettings.GetComponent<JCollider>().enabled = enabled;
            playerSettings.GetComponent<JPhysicsBody>().enabled = enabled;
            var playerInputComponent = playerSettings.GetComponent<PlayerInputComponent>();
            playerInputComponent.InputIsFresh = enabled ? playerInputComponent.InputIsFresh : false;
            playerInputComponent.enabled = enabled;
            SetActiveForAllChildren(playerSettings.transform, enabled);
        }

        private void SetActiveForAllChildren(Transform someTransform, bool active)
        {
            for (var i = 0; i < someTransform.childCount; i++)
            {
                someTransform.GetChild(i).gameObject.SetActive(active);
            }
        }

        private IEnumerator RespawnPlayer(int playerNumber, float delayInSeconds)
        {
            Debug.Log("Waiting " + delayInSeconds + " seconds to respawn player " + playerNumber);
            yield return new WaitForSeconds(delayInSeconds);

            Debug.Log("Time to respawn player " + playerNumber);
            var playerSettings = GameObject.FindObjectsOfType<PlayerSettings>();
            for (var i = 0; i < playerSettings.Length; i++)
            {
                var player = playerSettings[i];
                if (player.playerNumber == playerNumber)
                {
                    Debug.Log("Found player to respawn");
                    ReEnablePlayer(player);
                    break;
                }
            }
            Debug.Log("Couldn't find player " + playerNumber + " to respawn. Did the player quit?");
        }

        private void ReEnablePlayer(PlayerSettings playerSettings)
        {
            playerSettings.GetComponent<PlayerHealth>().ResetHealth();
            var playerHealth = playerSettings.GetComponent<PlayerHealth>();
            playerHealth.ResetHealth();
            AFEventManager.INSTANCE.PostEvent(AFEvents.Respawn(playerSettings.playerNumber, playerHealth.HealthLeft));
            
            EnableOrDisablePlayer(playerSettings, true);

            var newSpawnPoint = spawnPointManager.GetNextSpawnPoint();
            var pTransform = playerSettings.transform;
            pTransform.position = newSpawnPoint.position;
            pTransform.rotation = newSpawnPoint.rotation;
        }

        private void CreateExplosion(PlayerSettings playerSettings)
        {
            var explosion = Instantiate(playerDeathExplosionPrefab, playerSettings.transform.position, playerSettings.transform.rotation);
            var explosionMainModule = explosion.main;
            explosionMainModule.startColor = playerSettings.color;
        }
    }
}
