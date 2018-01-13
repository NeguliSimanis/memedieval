using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Required when Using UI elements.

namespace MeMedieval
{
    public class Resources : MonoBehaviour
    {
        [SerializeField] private int resources;
        [SerializeField] private int resourcesPerFiveSeconds;
        [SerializeField] Image resourceBar;

        private float second;

        void Start()
        {
            second = 0;
        }

        void Update()
        {
            if (Health.Victory || Health.GameOver) return;
            if (resourceBar != null) resourceBar.fillAmount = resources / 1000f;
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
