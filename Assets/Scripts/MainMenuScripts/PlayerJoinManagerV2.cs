﻿using UnityEngine;
using UnityEngine.SceneManagement;
using static Jerre.PlayerMenuItem;

namespace Jerre
{
    public class PlayerJoinManagerV2 : MonoBehaviour
    {         
        private PlayerMenuItem[] playerMenuItems;
        private bool GameAlreadyStarted = false;

        void Start()
        {
            playerMenuItems = GameObject.FindObjectsOfType<PlayerMenuItem>();
        }

        void Update()
        {

            var nReadyPlayers = 0;
            var nJoinNotReadyPlayers = 0;

            for (var i = 0; i < playerMenuItems.Length; i++)
            {
                var player = playerMenuItems[i];
                if (player.viewState == ViewState.JOINED_READY)
                {
                    nReadyPlayers++;
                }
                if (player.viewState == ViewState.JOINED_NOT_READY)
                {
                    nJoinNotReadyPlayers++;
                }
            }

            var shouldStartGame = (nReadyPlayers > 0 && nJoinNotReadyPlayers == 0);
            if (shouldStartGame && !GameAlreadyStarted)
            {
                GameAlreadyStarted = true;
                for (var i = 0; i < playerMenuItems.Length; i++)
                {
                    var player = playerMenuItems[i];
                    player.SetUsePlayerInput(false);

                    if (player.viewState == ViewState.JOINED_READY)
                    {
                        PlayersState.INSTANCE.PlayerInfos.Add(new PlayerInfo(player.PlayerNumber, player.PlayerColor));
                    }
                }
                Debug.Log("Starting game!");
                Invoke("TriggerNextSceneLoad", 1f);
            }
        }

        void TriggerNextSceneLoad()
        {
            Debug.Log("Bo! Next scene should load");
            SceneManager.LoadScene(SceneNames.GAME_SCENE, LoadSceneMode.Single);
        }
    }
}
