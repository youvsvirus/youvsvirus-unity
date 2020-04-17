using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using Components;

namespace Infection
{
    public class ProbabilityBasedInfection : MonoBehaviour
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
        private int daysSinceExposure = 0;

        /// <summary>
        /// My Human. Is set once in Start() and can be accessed by derived classes.
        /// </summary>
        protected HumanBase myHuman { get; private set; }

        /// <summary>
        /// Initialize this infection.
        /// </summary>
        public void Start()
        {
            myHuman = GetComponent<HumanBase>();
        }

        public  void Update()
        {
            if (IsNewDay())
            {
                UpdateCondition();
            }
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
        /// Checks if a new simulated day has begun this frame.
        /// Call this only in Update() or FixedUpdate()!
        /// </summary>
        /// <returns>true if a new day has begun, false otherwise.</returns>
        public bool IsNewDay()
        {
            return Time.time - lastDayTick > DayLength;
        }

        /// <summary>
        /// Always infects this human if it is susceptible.
        /// </summary>
        public void Infect()
        {
            if (myHuman.IsSusceptible())
            {
                myHuman.SetCondition(HumanBase.EXPOSED);
            }
        }

        /// <summary>
        /// Checks if this human is infectious.
        /// Based on if the humans is exposed or infectious
        /// </summary>
        /// <returns>True if this human is infectious, false otherwise.</returns>
        public bool IsInfectious()
        {
            return myHuman.GetCondition() == HumanBase.EXPOSED || myHuman.GetCondition() == HumanBase.INFECTIOUS;
        }


        /// <summary>
        /// Notify the infection that the human has been exposed.
        /// </summary>
        public void Expose()
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
                        if (daysSinceExposure > TimeToRecovery)
                        {
                            myHuman.SetCondition(HumanBase.RECOVERED);
                            return;
                        }

                        //  Maybe it breaks out today?
                        if (Random.value <= OutbreakRate)
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
                        if (Random.value <= RecoveryRate)
                        {
                            myHuman.SetCondition(HumanBase.RECOVERED);
                            return;
                        }

                        // Maybe we die today.
                        if (Random.value <= DeathRate)
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
