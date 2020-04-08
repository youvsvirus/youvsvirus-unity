using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Infection;

namespace Components
{
    /// <summary>
    /// All elements in our game are human
    /// </summary>
    public abstract class HumanBase : MonoBehaviour
    {

        /// <summary>
        /// This human's rigidbody component.
        /// </summary>
        protected Rigidbody2D myRigidbody = null;


        // The statistics object that counts the number of infected humans
        private LevelStats levelStats;

        /// <summary>
        /// This human's infection stages
        /// </summary>
        public const int WELL        = 0;
        public const int EXPOSED     = 1;
        public const int INFECTIOUS  = 2;
        public const int RECOVERED   = 3;
        public const int DEAD        = 4;


        
        /// <summary>
        /// Set the human's initial condition
        /// </summary>
        public void SetInitialCondition(int condition)
        {
            _mycondition = condition;
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
        /// Checks if this Human is allowed to move.
        /// Currently, it can always move, unless it is dead.
        /// </summary>
        /// <returns></returns>
        protected bool CanMove()
        {
            return GetCondition() != DEAD;
        }

        /// <summary>
        /// Set stage of infection and update smiley's image
        /// </summary>
        public virtual void SetCondition(int condition)
        {
            _mycondition = condition;

            if(_mycondition == DEAD)
            {
                //  Removes this human's Rigidbody from the physics simulation, 
                //  which disables movement and collision detection.
                myRigidbody.simulated = false;

                //  Set the simulated velocity to zero.
                myRigidbody.velocity = Vector3.zero;

                //  Put this human's sprite on the 'Dead' sorting layer.
                //  This layer is below the others, causing Dead humans to be rendered
                //  below the living.
                GetComponent<SpriteRenderer>().sortingLayerName = "Dead";
            }

            if(_mycondition == EXPOSED)
            {
                //  Notify our infection model that we've been exposed
                GetComponent<AbstractInfection>().Expose();
            }

            // Update the stats
            switch (condition)
            {
                case EXPOSED:
                    {
                        levelStats.aHumanGotExposed();
                        break;
                    }
                case INFECTIOUS:
                    {
                        levelStats.aHumanGotInfected();
                        break;
                    }
                case DEAD:
                    {
                        levelStats.aHumanDied();
                        break;
                    }
                case RECOVERED:
                    {
                        levelStats.aHumanRecovered();
                        break;
                    }
            }

            // Update the sprite image
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
            // Get the statistics object that counts the numbers of infected/dead etc players
            levelStats = LevelStats.GetActiveLevelStats();
            // We want to change smiley's images and do not want use GetComponent again
            // and again in the corresponding function
            mySpriteRenderer = GetComponent<SpriteRenderer>();
            myRigidbody = GetComponent<Rigidbody2D>();
            // The player and npc class set their corresponding sprite images
            SetSpriteImages();
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
