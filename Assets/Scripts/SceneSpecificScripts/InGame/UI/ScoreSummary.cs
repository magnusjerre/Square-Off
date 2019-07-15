﻿using Jerre.Events;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Jerre
{
    public class ScoreSummary : MonoBehaviour, IAFEventListener
    {
        public Text leaderScoreText, totalScoreText;

        private int leaderScore;
        private int leaderPlayerNumber;
        private Dictionary<int, Color> playerColorMap;

        void Awake() {
            AFEventManager.INSTANCE.AddListener(this);
            InitializeColorMap();
        }
        
        void Start()
        {
            leaderScoreText.text = "0";
            var scoreManager = GameObject.FindObjectOfType<FreeForAllGameModeManager>();
            if (scoreManager != null)
            {
                totalScoreText.text = scoreManager.maxScore + "";
            }
        }

        public bool HandleEvent(AFEvent afEvent)
        {
            switch(afEvent.type) {
                case AFEventType.SCORE: {
                    var payload = (ScorePayload)afEvent.payload;
                    if (payload.playerScore > leaderScore) {
                        leaderScore = payload.playerScore;
                        leaderPlayerNumber = payload.playerNumber;
                        if (!playerColorMap.ContainsKey(leaderPlayerNumber)) {
                            InitializeColorMap();
                        }
                        leaderScoreText.color = playerColorMap[leaderPlayerNumber];
                        leaderScoreText.text = leaderScore + "";
                    }
                    break;
                }
            }
            return false;
        }
        void InitializeColorMap() {
            playerColorMap = new Dictionary<int, Color>();
            for (var i = 0; i < PlayersState.INSTANCE.PlayerInfos.Count; i++)
            {
                var playerMenuSettings = PlayersState.INSTANCE.PlayerInfos[i];
                playerColorMap.Add(playerMenuSettings.Number, playerMenuSettings.Color);
            }
        }
    }
}
