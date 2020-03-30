using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Model;

namespace Components
{
    public abstract class HumanBase : MonoBehaviour
    {
        private HumanCondition _condition = HumanCondition.WELL;

        /// <summary>
        /// This human's condition. Setting this also updates the sprite image accordingly.
        /// </summary>
        public HumanCondition Condition {
            get { return _condition; }
    
            private set 
            {
                _condition = value;
                UpdateSpriteImage();
            }
        }

        /// <summary>
        /// This human's condition at the start of the simulation
        /// </summary>
        public string initialCondition = "WELL";
        
        public Sprite WellSprite, InfectedSprite, IllSprite, RecoveredSprite, DeadSprite;

        protected SpriteRenderer mySpriteRenderer;

        public virtual void Start()
        {
            mySpriteRenderer = GetComponent<SpriteRenderer>();

            switch (initialCondition.ToUpper())
            {
                case "WELL":
                    {
                        Condition = HumanCondition.WELL;
                        break;
                    }
                case "INFECTED":
                    {
                        Condition = HumanCondition.INFECTED;
                        break;
                    }
                case "ILL":
                    {
                        Condition = HumanCondition.ILL;
                        break;
                    }
                case "DEAD":
                    {
                        Condition = HumanCondition.DEAD;
                        break;
                    }
                case "RECOVERED":
                    {
                        Condition = HumanCondition.RECOVERED;
                        break;
                    }
                default: break;
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
                Condition = HumanCondition.INFECTED;
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
            return Condition == HumanCondition.INFECTED || Condition == HumanCondition.ILL;
        }

        /// <summary>
        /// Checks if this human is susceptible to infection.
        /// </summary>
        /// <returns>True if this human is susceptible, false otherwise.</returns>
        public bool IsSusceptible()
        {
            return Condition == HumanCondition.WELL;
        }

        /// <summary>
        /// Updates this human's sprite image depending on its infection state.
        /// </summary>
        public void UpdateSpriteImage()
        {
            switch (Condition)
            {
                case HumanCondition.WELL:
                    {
                        mySpriteRenderer.sprite = WellSprite;
                        break;
                    }
                case HumanCondition.INFECTED:
                    {
                        mySpriteRenderer.sprite = InfectedSprite;
                        break;
                    }
                case HumanCondition.ILL:
                    {
                        mySpriteRenderer.sprite = IllSprite;
                        break;
                    }
                case HumanCondition.DEAD:
                    {
                        mySpriteRenderer.sprite = DeadSprite;
                        break;
                    }
                case HumanCondition.RECOVERED:
                    {
                        mySpriteRenderer.sprite = RecoveredSprite;
                        break;
                    }
                default: break;
            }
        }

    }
}
