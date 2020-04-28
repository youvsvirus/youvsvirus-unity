using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using Components;

namespace Infection
{
    public class ProbabilityBasedInfection : AbstractInfection
    {
        /// <summary>
        /// How long my human has been exposed to the virus
        /// </summary>
        private int daysSinceExposure = 0;

        /// <summary>
        /// Contains parmams about how fast infection spreads
        /// </summary>
        private InfectionControl infectionControl;

        /// <summary>
        /// Initialize this infection.
        /// </summary>
        public override void Start()
        {
            base.Start();
            //  Find the infection control instance
            GameObject InfectionControl = GameObject.Find("InfectionControl");
            infectionControl = InfectionControl.GetComponent<InfectionControl>();
        }

        public override void Update()
        {
            if (infectionControl.IsNewDay())
            {
                // nobody becomes infectious or dies at the party
                // due to the much shorter time scale
                if(LevelSettings.GetActiveSceneName() != "YouVsVirus_Level3")
                    UpdateCondition();
            }
        }

        /// <summary>
        /// Notify the infection that the human has been exposed.
        /// </summary>
        public override void Expose()
        {
            daysSinceExposure = 0;
        }

        /// <summary>
        /// Update my human's condition
        /// </summary>
        private void UpdateCondition()
        {

            switch (myHuman.GetCondition())
            {
                case HumanBase.EXPOSED:
                    {
                        // Damn, we're EXPOSED. But wINFECTIOUS the sickness actually break out?
                        //  Incubation time has passed without infection --> recovered!
                        if (daysSinceExposure > infectionControl.IncubationTime)
                        {
                            myHuman.SetCondition(HumanBase.RECOVERED);
                            return;
                        }

                        //  Maybe it breaks out today?
                        if (Random.value <= infectionControl.OutbreakRate)
                        {
                            myHuman.SetCondition(HumanBase.INFECTIOUS);
                            return;
                        }

                        daysSinceExposure++;
                        return;
                    }

                case HumanBase.INFECTIOUS:
                    {
                        // Maybe we recover today...
                        if (Random.value <= infectionControl.RecoveryRate)
                        {
                            myHuman.SetCondition(HumanBase.RECOVERED);
                            return;
                        }

                        // Maybe we die today.
                        if (Random.value <= infectionControl.DeathRate)
                        {
                            myHuman.SetCondition(HumanBase.DEAD);
                            return;
                        }

                        return;
                    }

                default: return;
            }
        }
    }
}
