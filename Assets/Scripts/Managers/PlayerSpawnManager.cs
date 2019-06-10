﻿using Jerre.Events;
using Jerre.GameSettings;
using Jerre.UI;
using System.Collections.Generic;
using UnityEngine;

namespace Jerre
{
    public class PlayerSpawnManager : MonoBehaviour, IAFEventListener
    {
        public PlayerSettings playerPrefab;//yolo

        private Dictionary<int, PlayerSettings> playerNumberMap;
        private SpawnPointManager spawnPointManager;

        private Color[] playerColors;
        private int indexOfNextColor = 0;

        private bool CanJoinInGame = false;
        public Transform PlayersContainer;

        private void Awake()
        {
            playerNumberMap = new Dictionary<int, PlayerSettings>();
            playerColors = new Color[] { Color.red, Color.green, Color.blue, Color.yellow };

            CanJoinInGame = PlayersState.INSTANCE.ReadyPlayersCount == 0;

            spawnPointManager = GameObject.FindObjectOfType<SpawnPointManager>();

            AFEventManager.INSTANCE.AddListener(this);
        }

        void Start()
        {
            if (!CanJoinInGame)
            {
                var allPlayerSettings = new List<PlayerSettings>(PlayersState.INSTANCE.ReadyPlayersCount);
                for (var i = 0; i < PlayersState.INSTANCE.ReadyPlayersCount; i++)
                {
                    var playerMenuSettings = PlayersState.INSTANCE.GetSettings(i);
                    allPlayerSettings.Add(AddPlayer(playerMenuSettings.Number, playerMenuSettings.Color));
                }
                AFEventManager.INSTANCE.PostEvent(AFEvents.PlayersAllCreated(allPlayerSettings));
            }
        }

        void Update()
        {
            if (!CanJoinInGame) return;
            for (int i = 1; i <= 4; i++)
            {
                var joinLeaveKeyName = PlayerInputTags.JOIN_LEAVE + i;
                if (Input.GetButtonDown(joinLeaveKeyName)) {
                    if (!playerNumberMap.ContainsKey(i))
                    {
                        AddPlayer(i);
                    } else
                    {
                        RemovePlayer(i);
                    }
                }
            }
        }

        private void AddPlayer(int playerNumber)
        {
            AddPlayer(playerNumber, NextColor());
        }

        private PlayerSettings AddPlayer(int playerNumber, Color color)
        {
            var spawnPoint = spawnPointManager.GetNextSpawnPoint();
            var newPlayer = Instantiate(playerPrefab, spawnPoint.transform.position, spawnPoint.transform.rotation);
            newPlayer.playerNumber = playerNumber;
            playerNumberMap.Add(playerNumber, newPlayer);
            newPlayer.color = color;
            var basicGameSettings = GameSettingsState.INSTANCE.BasicGameSettings;
            var basicWeaponsSettings = GameSettingsState.INSTANCE.BasicWeaponsSettings;
            newPlayer.MaxSpeed = basicGameSettings.Speed;
            newPlayer.MaxAcceleration = basicGameSettings.MaxAcceleration;
            newPlayer.BoostSpeed = basicGameSettings.BoostSpeed;
            newPlayer.BoostDuration = basicGameSettings.BoostDuration;
            newPlayer.BoostPauseDuration = basicGameSettings.BoostPause;
            newPlayer.MaxHealth = basicGameSettings.Health;
            newPlayer.FireRate = basicWeaponsSettings.FireRate;
            newPlayer.BombPauseTime = basicWeaponsSettings.BombPauseTime;
            newPlayer.transform.parent = PlayersContainer;

            var mainEngineParticles = newPlayer.GetComponent<PlayerMainEngineParticles>();
            mainEngineParticles.ParticleColor = color;
            AFEventManager.INSTANCE.PostEvent(AFEvents.PlayerJoin(playerNumber, color));
            return newPlayer;
        }

        private void RemovePlayer(int playerNumber)
        {
            if (playerNumberMap.ContainsKey(playerNumber))
            {
                var playerToRemove = playerNumberMap[playerNumber];
                if (playerNumberMap.Remove(playerNumber))
                {
                    Destroy(playerToRemove.gameObject);
                }
            }
            AFEventManager.INSTANCE.PostEvent(AFEvents.PlayerLeave(playerNumber));
        }

        private Color NextColor()
        {
            var color = playerColors[indexOfNextColor];
            indexOfNextColor = (indexOfNextColor + 1) % playerColors.Length;
            return color;
        }

        public bool HandleEvent(AFEvent afEvent)
        {
            if (afEvent.type == AFEventType.GAME_OVER)
            {
                //for (var i = 1; i <= 4; i++)
                //{
                //    RemovePlayer(i);
                //    indexOfNextColor = 0;
                //    //scoreUIManager.ResetNumberInLine();
                //}
            }
            return false;
        }
    }
}
