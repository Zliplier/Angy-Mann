using System;
using Gameplay.Combat;
using UnityEngine;
using UnityEngine.UI;
using Zlipacket.Core.Tools.Utilities;

namespace Gameplay.Player
{
    public class PlayerHealth : Health
    {
        [Header("Health Decay")]
        [SerializeField] private float healthDecayRate = 1f;
        [SerializeField] private float minHealthDecayMultiplier = 1f;
        [SerializeField] private float maxHealthDecayMultiplier = 3f;
        [SerializeField] private float timeToMaxDecay = 300f;
        
        [Header("Lerp Slider")]
        public float lerpSpeed;
        
        [Header("Components")]
        [SerializeField] private Slider healthSlider;
        [SerializeField] private Slider lerpSlider;

        [Header("Enabled")]
        public bool decayEnabled = true;
        
        private Timer healthDecayTimer;
        
        private void Awake()
        {
            healthSlider.value = HealthPercentage;
            onHealthChanged.AddListener((float health, float maxHealth) =>
            {
                healthSlider.value = HealthPercentage;
            });
            
            healthDecayTimer = new Timer(timeToMaxDecay);
            healthDecayTimer.Start();
        }

        private void Update()
        {
            if (decayEnabled)
                healthDecayTimer.Tick(Time.deltaTime);
            
            float healthDecayMultiplier = Mathf.Lerp(minHealthDecayMultiplier, maxHealthDecayMultiplier, 1 - healthDecayTimer.NormalizedTime);
            HealthPoints -= healthDecayRate * healthDecayMultiplier * Time.deltaTime;
            
            lerpSlider.value = Mathf.Lerp(lerpSlider.value, healthSlider.value, lerpSpeed * Time.deltaTime);
        }
    }
}