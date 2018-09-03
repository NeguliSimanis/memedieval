using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Required when Using UI elements.

namespace MeMedieval
{
    /// <summary>
    /// Handles resources during battle (only meat is relevant)
    /// </summary>
    public class Resources : MonoBehaviour
    {
        [SerializeField] private bool isPlayerResources = false;
        [SerializeField] private int resources;
        [SerializeField] private int resourcesPerFiveSeconds;
        [SerializeField] Image resourceBar;
        [SerializeField] Text resourceText;

        private float second;

        void Start()
        {
            if (isPlayerResources)
            {
                ChampionEffect championEffect = PlayerProfile.Singleton.gameObject.GetComponent<ChampionEffect>();
                resources = Mathf.RoundToInt(PlayerProfile.Singleton.MeatCurrent * championEffect.startingMeatCoefficient);
            }
            second = 0;
        }

        void Update()
        {
            if (Health.Victory || Health.GameOver) return;
            if (resourceBar != null) resourceBar.fillAmount = resources / 1000f;
            if (resourceText != null) resourceText.text = resources.ToString();
            second += Time.deltaTime;
            if (second >= 5)
            {
                second -= 5;
                resources += resourcesPerFiveSeconds;
            }
        }

        public bool WasEnoughResources(int resources)
        {
            if (this.resources >= resources)
            {
                this.resources -= resources;
                return true;
            }
            return false;
        }

        public bool IsEnoughResources(int resources)
        {
            return this.resources >= resources;
        }

        public int Amount
        {
            get { return resources; }
            set { resources += value; }
        }
    }
}
