using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using Components;

namespace Infection
{
    /// <summary>
    ///  A simple agent-based simulation with a Covid-19-related time-delayed infection model 
    /// one day is 1f
    /// mean incubation is about 5.1f: from being exposed to the virus to being infectious (we assume a gamma distribution)
    /// mean infectious period is 3f: from being infectious to recovery (we assume a uniform distribution)
    /// the basic reproductive number is between 2-4 which affects the probability of infection per contact "InfectionRate = 3f"
    /// further numbers: https://elifesciences.org/articles/57309
    /// and maybe for later use // Gamma(18.8, 0.45)t ime between  onset of  symptoms and  death(onset - to - death).
    /// </summary>
    public class TimeDelayInfection : AbstractInfection
    {
        /// <summary>
        /// Time that passes until I recover or die
        /// </summary>
        public float t_personal_infectious = float.MaxValue;

        /// <summary>
        /// At this point int time I was exposed to the virus
        /// </summary>
        public float t_start_incubation = float.MaxValue;

        /// <summary>
        /// At this point int time I showed symptoms and was a clear danger to others
        /// </summary>
        public float t_start_infectious = float.MaxValue;

        /// <summary>
        /// Time that passes until I am infectious
        /// </summary>
        public float t_personal_incubation = float.MaxValue;

        public override void Update()
        {
            UpdateCondition();
        }
        public override void Start()
        {
            base.Start();

        }
        /// <summary>
        /// Notify the infection that the human has been exposed.
        /// </summary>
        public override void Expose()
        {
            StartExposeTimer();
        }

        /// <summary>
        /// Update my human's condition
        /// </summary>

        /// <summary>
        /// The probability by which an EXPOSED person infects another person that also becomes infectious
        /// https://www.thelancet.com/journals/lancet/article/PIIS0140-6736(20)30462-1/fulltextS
        /// </summary>
        public float InfectionRate = 0.2f;

        /// <summary>
        /// The probability for an INFECTIOUS person to die on a given day.
        /// </summary>
        public float DeathRate = 0.03f;

        /// <summary>
        /// latency period
        /// if I am exposed I will not infect others during this period
        /// </summary>
        private float t_latent = 3f;

        /// <summary>
        /// A human's personal t_infectious is assumed as a uniform distribution between 2 and 4 days
        /// </summary>
        /// <returns>personal t_infectious</returns>
        public float GetPersonal_t_infectious()
        {
            return (UnityEngine.Random.Range(2.0f, 4.0f));
        }

        /// <summary>
        /// A human's personal t_incubation is assumed as a gamma distribution with shape 5.807 and rate 0.948
        /// from https://papers.ssrn.com/sol3/papers.cfm?abstract_id=3551006 or 
        /// https://www.imperial.ac.uk/media/imperial-college/medicine/sph/ide/gida-fellowships/Imperial-College-COVID19-Europe-estimates-and-NPI-impact-30-03-2020.pdf 
        /// </summary>
        /// <returns>personal t_incubation</returns>
        public float GetPersonal_t_incubation()
        {
            return ((float)gamma(5.807, 0.948));
        }

        /// <summary>
        /// we need to specify a serial interval distribution
        /// (the  time  between when a person gets infected and when they subsequently infect another other people), 
        /// which we choose to be Gamma distributed:g∼𝐺amma(6.5,0.62)
        /// </summary>
        public float GetPersonal_t_latent()
        {
            return ((float)gamma(6.5, 0.62));
        }
        // random number generator needed for our probability distribution
        private static System.Random r = new System.Random();


        public void StartExposeTimer()
        {
            // start my incubation timer 
            t_start_incubation = Time.fixedTime;
            t_personal_incubation = GetPersonal_t_incubation();
            t_personal_infectious = GetPersonal_t_infectious();
        }

        public void StartInfectiousTimer()
        {
            // start my infection timer (only used for TimeDelay model)
            t_start_infectious = Time.fixedTime;
        }

        /// <summary>
        /// Infects this human with a probability of InfectionRate per contact.
        /// </summary>
        public override bool IsInfectionSuccessful()
        {
            if (UnityEngine.Random.value < InfectionRate)
            {
                return true;
            }

            else
                return false;
        }

        // <summary>
        /// Checks if this human is infectious.
        /// Based on if the humans is infectious and if the latency period has passed
        /// </summary>
        /// <returns>True if this human is infectious, false otherwise.</returns>
        public override bool IsInfectious(int condition)
        {
            // if I show symptoms I am always infectious and can infect others
            if (condition == HumanBase.INFECTIOUS)
                return true;
            //if I do not show symptoms yet, I only infect others after the latency period
            else if (condition == HumanBase.EXPOSED && Time.fixedTime - t_start_incubation >= t_latent)
            {
                UpdateCondition();
                return true;
            }

            else
                return false;
        }


        /// <summary>
        /// Update my human's condition
        /// </summary>
        private void UpdateCondition()
        {

            int condition = myHuman.GetCondition();
            switch (condition)
            {
                case HumanBase.EXPOSED:
                    {
                        // check if the human's personal incubation time has passed
                        if (Time.fixedTime - t_start_incubation > t_personal_incubation)
                        {
                            myHuman.SetCondition(HumanBase.INFECTIOUS);
                            StartInfectiousTimer();
                            return;
                        }
                        return;
                    }

                case HumanBase.INFECTIOUS:
                    {

                        // check if my human's personal infectious time has passed
                        if (Time.fixedTime - t_start_infectious > t_personal_infectious)
                        {
                            // there is a chance we die
                            if (UnityEngine.Random.value <= DeathRate)
                            {
                                myHuman.SetCondition(HumanBase.DEAD);
                                return;
                            }
                            // or a chance to recover
                            myHuman.SetCondition(HumanBase.RECOVERED);
                            return;
                        }
                        return;
                    }
                default: return;
            }
        }



        /// <summary>
        /// gamma distribution with shape a and rate b 
        /// from https://archive.codeplex.com/?p=rng
        /// </summary>
        /// <param name="a">shape</param>
        /// <param name="b">rate</param>
        /// <returns>random gamma distributed value</returns>
        static System.Double gamma(System.Double a = 2.0, System.Double b = 0.5)
        {
            if (a < 1.0)
            {
                return (gamma(a + 1.0, b) * System.Math.Pow(uniform(), 1.0 / a));
            }   // special case when alpha is less than 1.0

            System.Double d = a - 1.0 / 3.0; System.Double c = 1.0 / System.Math.Sqrt(9.0 * d);
            System.Double z, v, p;

            do
            {  // iteratively find the random number
                z = normal(); v = System.Math.Pow(1.0 + c * z, 3.0);
                p = 0.5 * System.Math.Pow(z, 2.0) + d - d * v + d * System.Math.Log(v);
            } while ((z < -1.0 / c) || (System.Math.Log(uniform()) > p));

            return ((d * v) / b);
        }  // gamma distribution with shape 2.0 and rate 0.5


        /// <summary>
        /// uniform distribution in [a,b[ 
        /// from https://archive.codeplex.com/?p=rng
        /// </summary>
        /// <param name="a">lower inclusive bound</param>
        /// <param name="b">upper exclusive bound</param>
        /// <returns>random uniform distributed value</returns>
        static System.Double uniform(System.Double a = 0.0, System.Double b = 1.0)
        {
            return (a + (b - a) * r.NextDouble());
        }
        /// <summary>
        /// gauss distribution  
        /// modified from https://archive.codeplex.com/?p=rng
        /// </summary>
        /// <param name="u">mean</param>
        /// <param name="s">standard deviation</param>
        /// <returns>random gauss distributed value</returns>
        static System.Double gaussian(System.Double u = 0.0, System.Double s = 1.0)
        {
            return (s * System.Math.Sqrt(-2.0 * System.Math.Log(uniform())) *
                System.Math.Cos(2.0 * System.Math.PI * uniform()) + u);
        }   // normal distribution with mean 0 stadnard deviation 1

        /// <summary>
        /// normal distribution  
        /// from https://archive.codeplex.com/?p=rng
        /// </summary>
        /// <returns>gauss distributed value with mean 0 and standard devation 1</returns>
        static System.Double normal()
        {
            return (gaussian());
        }
    }
}
