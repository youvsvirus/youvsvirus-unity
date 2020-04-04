using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Components
{
    /// <summary>
    /// All elements in our game are human
    /// </summary>
    public abstract class HumanBase : MonoBehaviour
    {
        /// <summary>
        /// Contains parmams about how fast infection spreads
        /// </summary>
        private InfectionControl infection;

        /// <summary>
        /// How long this human has been exposed to the virus
        /// </summary>
        private int daysSinceExposure = 0;

        /// <summary>
        /// This human's infection stages
        /// </summary>
        public const int WELL        = 0;
        public const int EXPOSED     = 1;
        public const int INFECTIOUS  = 2;
        public const int RECOVERED   = 3;
        public const int DEAD        = 4;


        /// <summary>
        /// This human's inital condition
        /// </summary>
        protected int _initialCondition = WELL;

        /// <summary>
        /// Set the human's initial condition
        /// Default: well
        /// This function is called by base classes even before Start() who are allowed
        /// to modify _initialCondition in advance. In Start() the _initialCondition
        /// value ist transferred to _mycondition
        /// </summary>
        public void SetInitialCondition(int condition)
        {
            _initialCondition = condition;
        }

        /// <summary>
        /// This human's condition in the game
        /// </summary>
        private int _mycondition =  WELL;

        /// <summary>
        /// Get stage of infection
        /// </summary>
        public int GetCondition()
        {
            return _mycondition;
        }

        /// <summary>
        /// Set stage of infection and update smiley's image
        /// </summary>
        public void SetCondition(int condition)
        {
            _mycondition = condition;
            UpdateSpriteImage();
        }
        /// <summary>
        /// The Sprites corresponding to the images for the different conditions
        /// Images have to be set in the derived classes
        /// </summary>     
        protected Sprite WellSprite=null, ExposedSprite = null, InfectiousSprite = null, RecoveredSprite = null, DeadSprite = null;

        /// <summary>
        /// Set images corresponding to stages of infection
        /// Have to be set in player and npc class
        /// </summary>   
        public abstract void SetSpriteImages();

        /// <summary>
        /// With the help of the SpriteRenderer we can change the smiley's images when infected
        /// </summary>
        protected SpriteRenderer mySpriteRenderer;

        /// <summary>
        /// Start is called on the frame when a script is enabled just before any of the Update methods are called the first time.
        /// </summary>
        public virtual void Start()
        {
            /// <summary>
            /// Need this to decide if this human is infected
            /// This is how we get an instance from that class 
            /// </summary>
            GameObject  InfectionControl = GameObject.Find("InfectionControl");
            infection = InfectionControl.GetComponent< InfectionControl>();
            // We want to change smiley's images and do not want use GetComponent again
            // and again in the corresponding function
            mySpriteRenderer = GetComponent<SpriteRenderer>();
            // The player and npc class set their corresponding sprite images
            SetSpriteImages();
            // _initialCondition may have been modified by base classes
            // this becomes _mycondition during Start()
            SetCondition(_initialCondition);
        }

        /// <summary>
        /// Every day updates the human's condition
        /// </summary>
        public virtual void Update()
        {
            if(infection.IsNewDay())
            {
                UpdateCondition();
            }
        }

        /// <summary>
        /// Update the human's condition
        /// </summary>
        private void UpdateCondition()
        {
            switch (GetCondition())
            {
                case EXPOSED:
                    {
                        // Damn, we're EXPOSED. But wINFECTIOUS the sickness actually break out?
                        //  Incubation time has passed without infection --> recovered!
                        if (daysSinceExposure > infection.IncubationTime)
                        {
                            SetCondition(RECOVERED);
                            return;
                        }

                        //  Maybe it breaks out today?
                        if(Random.value <= infection.OutbreakRate)
                        {
                            SetCondition(INFECTIOUS);
                            return;
                        }

                        daysSinceExposure++;
                        return;
                    }

                case INFECTIOUS:
                    {
                        // Maybe we recover today...
                        if(Random.value <= infection.RecoveryRate)
                        {
                            SetCondition(RECOVERED);
                            return;
                        }

                        // Maybe we die today.
                        if (Random.value <= infection.DeathRate)
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
        /// <returns>True if this human became EXPOSED, false otherwise.</returns>
        public bool Infect()
        {
            if (IsSusceptible())
            {
                SetCondition(EXPOSED);
                daysSinceExposure = 0;
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
            return GetCondition() == EXPOSED || GetCondition() == INFECTIOUS;
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
                case EXPOSED:
                    {
                        if (ExposedSprite != null)
                            mySpriteRenderer.sprite = ExposedSprite;
                        break;
                    }
                case INFECTIOUS:
                    {
                        if (InfectiousSprite != null)
                            mySpriteRenderer.sprite = InfectiousSprite;
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
                    Debug.LogError("Sprite not known or its image not set.");
                    break;
            }
        }

    }
}
