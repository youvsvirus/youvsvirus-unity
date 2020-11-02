using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using Components;

namespace Infection
{
    /// <summary>
    /// Base class for different infection models.
    /// Attach to a human to control its infection behaviour.
    /// </summary>
    [RequireComponent(typeof(HumanBase))]
    public abstract class AbstractInfection : MonoBehaviour
    {
        /// <summary>
        /// My Human. Is set once in Start() and can be accessed by derived classes.
        /// </summary>
        protected HumanBase myHuman { get; private set; }

        /// <summary>
        /// Infects this human with a probability of InfectionRate per contact.
        /// </summary>
        public abstract bool IsInfectionSuccessful();
  
        public virtual void Start()
        {
            myHuman = GetComponent<HumanBase>();
        }

        public abstract void Update();

        /// <summary>
        /// Notify the infection that the human has been exposed.
        /// </summary>
        public abstract void Expose();
        public abstract bool IsInfectious(int condition);
    }
}
