﻿using Jerre.GameSettings;
using UnityEngine;

namespace Jerre
{
    public class BombSettings : MonoBehaviour
    {
        public int BlastDamage = 2;
        public float MaxBlastDamageSetting = 100f;
        public float BlastAcceleration = 1000f;
        public float BlastAccelerationDuration = 0.2f;
        public float BlastRadius = 10f;
        public float MaxLifeTimeWithoutExploding = 10f;
        public int PlayerOwnerNumber;
        public Color color;

        void Awake()
        {
            var settings = GameSettingsState.INSTANCE.BasicWeaponsSettings;
            BlastDamage = settings.BombDamage;
            BlastRadius = settings.BombExplosionRadius;
            MaxLifeTimeWithoutExploding = settings.BombMaxLifetime;
            BlastAcceleration = settings.BombExplosionAcceleration;
        }

        void Start()
        {
            var children = GetComponentsInChildren<SpriteRenderer>();
            for (var i = 0; i < children.Length; i++)
            {
                children[i].color = color;
            }

            var meshChildren = GetComponentsInChildren<MeshRenderer>();
            for (var i = 0; i < meshChildren.Length; i++)
            {
                var meshChild = meshChildren[i];
                meshChild.material.color = color;
                meshChild.transform.localScale = Vector3.one * (BlastRadius / 10f) * meshChild.transform.localScale.x;
            }
            
        }
    }
}
