﻿using Jerre.Events;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Jerre
{
    public class PauseMenu : MonoBehaviour, IAFEventListener
    {

        public int PlayerNumber;
        public Color PlayerColor;
        public Image PlayerIndicatorIcon;

        private bool IsPausing
        {
            get
            {
                return PlayerNumber > 0;
            }
        }

        void Awake()
        {
            AFEventManager.INSTANCE.AddListener(this);
        }

        void Start()
        {
            HidePauseMenu();
        }

        void Update()
        {
            if (!IsPausing) return;
            
            if (Input.GetButton(PlayerInputTags.DODGE_RIGHT + PlayerNumber) 
                && Input.GetButton(PlayerInputTags.DODGE_LEFT + PlayerNumber) 
                && Input.GetButton(PlayerInputTags.ACCEPT + PlayerNumber))
            {
                QuitGame();
            }
            else if (Input.GetButtonDown(PlayerInputTags.FIRE2  + PlayerNumber))
            {
                ResumeGame();
            }
        }

        private void ShowPauseMenu()
        {
            PlayerIndicatorIcon.color = PlayerColor;
            gameObject.SetActive(true);
            PlayerComponentsEnabler.EnableOrDisableAllPlayersInputResponses(false);
            Time.timeScale = 0f;
        }

        private void HidePauseMenu()
        {
            PlayerNumber = -1;
            gameObject.SetActive(false);
            PlayerComponentsEnabler.EnableOrDisableAllPlayersInputResponses(true);
            Time.timeScale = 1f;
        }

        private void ResumeGame()
        {
            HidePauseMenu();
        }

        private void QuitGame()
        {
            PlayerComponentsEnabler.EnableOrDisableAllPlayersInputResponses(true);
            Time.timeScale = 1f;
            AFEventManager.INSTANCE.RemoveAllListeners();
            PlayersState.INSTANCE.Reset();
            SceneManager.LoadScene(SceneNames.GAME_MODE_SELECTION);
        }

        public bool HandleEvent(AFEvent afEvent)
        {
            switch (afEvent.type)
            {
                case AFEventType.PAUSE_MENU_ENABLE:
                    {
                        if (IsPausing) break;
                        var payload = (PauseMenuEnablePayload)afEvent.payload;
                        PlayerNumber = payload.PlayerNumber;
                        PlayerColor = payload.PlayerColor;
                        ShowPauseMenu();
                        break;
                    }
                case AFEventType.PAUSE_MENU_DISABLE:
                    {
                        HidePauseMenu();
                        break;
                    }
            }
            return false;
        }
    }
}
