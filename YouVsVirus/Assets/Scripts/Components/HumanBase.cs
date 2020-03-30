using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Model;

namespace Components
{
    public class HumanBase : MonoBehaviour
    {
        public HumanCondition Condition { get; private set; }

        public virtual void Start()
        {

        }

        /// <summary>
        /// Infects this human if it is susceptible.
        /// </summary>
        /// <returns>True if this human became infected, false otherwise.</returns>
        public bool Infect()
        {
            if (IsInfectious())
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

    }
}
