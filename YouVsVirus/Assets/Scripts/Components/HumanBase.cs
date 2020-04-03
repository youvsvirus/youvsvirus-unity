using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Model;

namespace Components
{
    public abstract class HumanBase : MonoBehaviour
    {
        private SimulationController simulationController;


        private int daysSinceInfection = 0;

        public const int WELL = 0;
        public const int INFECTED = 1;
        protected const int ILL = 2;
        protected const int RECOVERED = 3;
        protected const  int DEAD = 4;

        private int _mycondition = WELL;

        public int GetCondition()
        {
            return _mycondition;
        }
        public void SetCondition(int condition)
        {
            _mycondition = condition;
            UpdateSpriteImage();
        }
                       
        // The Sprites corresponding to the images for the different conditions
        // Images have to be set in the derived classes
        protected  Sprite WellSprite=null, InfectedSprite = null, IllSprite = null, RecoveredSprite = null, DeadSprite = null;

        // Set the sprite images which correspond to the condition
        public abstract void SetSpriteImages();

        public void SetInitialHealthCondition(int condition)
        {
            SetCondition(condition);
        }

        protected SpriteRenderer mySpriteRenderer;

        public virtual void Start()
        {
            GameObject SimulationController = GameObject.Find("SimulationController");
            simulationController = SimulationController.GetComponent<SimulationController>();
            SetSpriteImages();
            mySpriteRenderer = GetComponent<SpriteRenderer>();
        }
       
        public virtual void Update()
        {
            if(simulationController.IsNewDay())
            {
                UpdateCondition();
            }
        }

        private void UpdateCondition()
        {
            switch (GetCondition())
            {
                case INFECTED:
                    {
                        // Damn, we're infected. But will the sickness actually break out?

                        //  Incubation time has passed without infection --> recovered!
                        if (daysSinceInfection > simulationController.IncubationTime)
                        {
                            SetCondition(RECOVERED);
                            return;
                        }

                        //  Maybe it breaks out today?
                        if(Random.value <= simulationController.OutbreakRate)
                        {
                            SetCondition(ILL);
                            return;
                        }

                        daysSinceInfection++;
                        return;
                    }

                case ILL:
                    {
                        // Maybe we recover today...
                        if(Random.value <= simulationController.RecoveryRate)
                        {
                            SetCondition(RECOVERED);
                            return;
                        }

                        // Maybe we die today.
                        if (Random.value <= simulationController.DeathRate)
                        {
                            SetCondition(DEAD);
                            return;
                        }

                        return;
                    }

                default: return;
            }
        }

        /// <summary>
        /// Infects this human if it is susceptible.
        /// </summary>
        /// <returns>True if this human became infected, false otherwise.</returns>
        public bool Infect()
        {
            if (IsSusceptible())
            {
                SetCondition(INFECTED);
                daysSinceInfection = 0;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Checks if this human is infectious.
        /// </summary>
        /// <returns>True if this human is infectious, false otherwise.</returns>
        public bool IsInfectious()
        {
            return GetCondition() == INFECTED || GetCondition() == ILL;
        }

        /// <summary>
        /// Checks if this human is susceptible to infection.
        /// </summary>
        /// <returns>True if this human is susceptible, false otherwise.</returns>
        public bool IsSusceptible()
        {
            return GetCondition() == WELL;
        }

        /// <summary>
        /// Updates this human's sprite image depending on its infection state.
        /// </summary>
        public void UpdateSpriteImage()
        {
            switch (GetCondition())
            {
                case WELL:
                    {
                        if (WellSprite != null)
                               mySpriteRenderer.sprite = WellSprite;                                    
                        break;
                    }
                case INFECTED:
                    {
                        if (InfectedSprite != null)
                            mySpriteRenderer.sprite = InfectedSprite;
                        break;
                    }
                case ILL:
                    {
                        if (IllSprite != null)
                            mySpriteRenderer.sprite = IllSprite;
                        break;
                    }
                case DEAD:
                    {
                        if (DeadSprite != null)
                            mySpriteRenderer.sprite = DeadSprite;
                        break;
                    }
                case RECOVERED:
                    {
                        if (RecoveredSprite != null)
                            mySpriteRenderer.sprite = RecoveredSprite;
                        break;
                    }
                default:
                    Debug.LogError("Sprite now known or its image not set.");
                    break;
            }
        }

    }
}
