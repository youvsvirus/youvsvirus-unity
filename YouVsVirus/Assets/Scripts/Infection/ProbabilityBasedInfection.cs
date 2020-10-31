using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using Components;
using System.Diagnostics;

namespace Infection
{
    public class ProbabilityBasedInfection : AbstractInfection
    {
        /// <summary>
        /// The probability by which an EXPOSED person infects another person
        /// </summary>
        public float InfectionRate = 0.2f;

        /// <summary>
        /// The probability for an INFECTIOUS person to die on a given day.
        /// </summary>
        public float DeathRate = 0.02f;

        /// <summary>
        /// The probabilty that an EXPOSED person actually become INFECTIOUS on a given day.
        /// </summary>
        public float OutbreakRate = 0.01f;

        /// <summary>
        /// The probabilty for an INFECTIOUS person to recover on a given day.
        /// </summary>
        public float RecoveryRate = 0.04f;

        /// <summary>
        /// The length of a simulated day in seconds.
        /// </summary>
        public float DayLength = 1f;


        /// <summary>
        /// The time to recovery in days. Anyone not showing symptoms after this time is recovered.
        /// </summary>
        public int TimeToRecovery = 20;

        private float lastDayTick = 0;

        /// <summary>
        /// How long my human has been exposed to the virus
        /// </summary>
        //private int daysSinceExposure = 0;

 

        /// <summary>
        /// Initialize this infection.
        /// </summary>
        public override void Awake()
        {
            base.Awake();
          
        }

        /// <summary>
        /// Checks if a new simulated day has begun this frame.
        /// Call this only in Update() or FixedUpdate()!
        /// </summary>
        /// <returns>true if a new day has begun, false otherwise.</returns>
        public bool IsNewDay()
        {
            return Time.time - lastDayTick > DayLength;
        }
        /// <summary>
        ///  Runs after all other update calls, ensuring that lastDayTick gets updated AFTER all humans have called IsNewDay().
        ///  After all, we don't want a day to end early.
        /// </summary>
        void LateUpdate()
        {
            if (Time.time - lastDayTick > DayLength)
            {
                lastDayTick = Time.time;
            }
        }



        /// <summary>
        /// Notify the infection that the human has been exposed.
        /// </summary>
        public override void StartExposeTimer(HumanBase human)
        {
            human.daysSinceExposure = 0;
        }
        public override void StartInfectiousTimer(HumanBase human)
        {
            // not needed
        }

        public override bool IsInfectionSuccessful()
        {
            return true;
        }


     //   public override void Infect()
       // {
         //   myHuman.SetCondition(HumanBase.EXPOSED);
           // Debug.Log("Infect");
        //}

        public override bool IsInfectious(int condition, float incubation_time)
        {
                return condition == HumanBase.EXPOSED ||condition == HumanBase.INFECTIOUS;
          
        }

        /// <summary>
        /// Update my human's condition
        /// </summary>
        public override int UpdateCondition(int condition, HumanBase human)
        {
            if (IsNewDay())
            {
                switch (condition)
                {
                    case HumanBase.EXPOSED:
                        {
                            // Damn, we're EXPOSED. But will the sickness actually break out?
                            //  Incubation time has passed without infection --> recovered!
                            if (human.daysSinceExposure > TimeToRecovery)
                            {
                                return (HumanBase.RECOVERED);

                            }

                            //  Maybe it breaks out today?
                            if (Random.value <= OutbreakRate)
                            {
                                return (HumanBase.INFECTIOUS);

                            }

                            human.daysSinceExposure++;
                            return (condition);
                        }

                    case HumanBase.INFECTIOUS:
                        {
                            // Maybe we recover today...
                            if (Random.value <= RecoveryRate)
                            {
                                return (HumanBase.RECOVERED);

                            }

                            // Maybe we die today.
                            if (Random.value <= DeathRate)
                            {
                                return (HumanBase.DEAD);
                            }

                            return condition;
                        }

                    default: return condition;
                }
            }
            else 
                return condition;
        }
    }
}
