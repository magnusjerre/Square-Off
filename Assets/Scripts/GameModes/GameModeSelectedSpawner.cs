﻿using Jerre.GameMode.Undead;
using UnityEngine;

namespace Jerre.GameMode
{
    public class GameModeSelectedSpawner : MonoBehaviour
    {
        public CountDownTimer countDownTimerPrefab;
        public RectTransform TopBar;

        private void Awake()
        {
            var selectedGameMode = PlayersState.INSTANCE.selectedGameMode;
            Debug.Log("selectedGameMode" + selectedGameMode);
            switch (selectedGameMode)
            {
                case GameModes.UNDEAD:
                    {
                        var undeadMode = gameObject.AddComponent<UndeadGameMode>();
                        undeadMode.countDownTimerPrefab = countDownTimerPrefab;
                        undeadMode.TopBar = TopBar;
                        break;
                    }
                case GameModes.FREE_FOR_ALL:
                    {
                        var scoreMode = gameObject.AddComponent<FreeForAllGameModeManager>();
                        scoreMode.countDownTimerPrefab = countDownTimerPrefab;
                        scoreMode.TopBar = TopBar;
                        break;
                    }
            }
        }

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
