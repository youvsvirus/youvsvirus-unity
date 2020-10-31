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
       // protected HumanBase myHuman { get; private set; }


        public abstract bool IsInfectious(int condition, float incubation_time);

        public abstract bool IsInfectionSuccessful();

        public abstract int UpdateCondition(int condition, HumanBase human);

        public virtual void Awake()
        {
            //myHuman = GetComponent<HumanBase>();
        }


        /// <summary>
        /// Notify the infection that the human has been exposed.
        /// </summary>
        public abstract void StartExposeTimer(HumanBase human);

        public abstract void StartInfectiousTimer(HumanBase human);
    }
}
