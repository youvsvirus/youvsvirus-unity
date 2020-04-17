using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using Components;

namespace Infection
{
    /// <summary>
    /// Manage different infection models.
    /// Infection models have to be attached to humans.
    /// </summary>
    public class InfectionManager : MonoBehaviour
    {
        /// <summary>
        /// Agent based time delayed infection model
        /// </summary>
        private TimeDelayInfection tdi;

        /// <summary>
        /// Probability based infection model
        /// </summary>
        private ProbabilityBasedInfection pbi;

        /// <summary>
        /// get the duration of the human's infectiousness
        /// </summary>
        /// <returns></returns>
        public float GetInfectionTime()
        {
            if (levelSettings.InfectionModel == "TimeDelay")
                return tdi.GetPersonal_t_infectious();
            else return 0f;
        }

        /// <summary>
        /// // get the duration of the human's whole illness or start exposure timer for probability based approach
        /// </summary>
        /// <returns></returns>
        public float GetIncubationTime()
        {
            if (levelSettings.InfectionModel == "TimeDelay")
                return tdi.GetPersonal_t_incubation();
            else
            {
                // start the counter of the probability based infection model
                // handled internally
                pbi.Expose();
                return 0f;
            }
        }

        /// <summary>
        /// Infects this human if it is susceptible.
        /// </summary>
        public void Infect()
        {
            if (levelSettings.InfectionModel == "TimeDelay")
                tdi.Infect();
            else
                pbi.Infect();
        }

        /// <summary>
        /// Checks if this human is infectious.
        /// </summary>
        /// <returns>True if this human is infectious, false otherwise.</returns>
        public bool IsInfectious()
        {
            if (levelSettings.InfectionModel == "TimeDelay")
                return(tdi.IsInfectious());
            else
                return(pbi.IsInfectious());
        }

        /// <summary>
        /// Stuff set in the main menu.
        /// </summary>
        private LevelSettings levelSettings;

        /// <summary>
        /// which infection model to choose
        /// </summary>
        public void Awake()
        {
            levelSettings = LevelSettings.GetActiveLevelSettings();
            if (levelSettings.InfectionModel == "TimeDelay")
                tdi = GetComponent<TimeDelayInfection>();
            else
                pbi = GetComponent<ProbabilityBasedInfection>();
        }
    }
}
