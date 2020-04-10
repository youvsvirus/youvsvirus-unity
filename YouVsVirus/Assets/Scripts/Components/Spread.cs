using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;


namespace Components
{
    public class Spread : MonoBehaviour
    {
        /// <summary>
        /// Stuff set in the main menu.
        /// </summary>
        private LevelSettings levelSettings;

        /// <summary>
        /// Our list of humans
        /// </summary>
        GameObject HumanInstants;
        private HumanInstants humans;

        /// <summary>
        /// The SEIR model for modeling the spread of infection
        /// </summary>
        private SEIR seir;

        /// <summary>
        /// Sum of at this time exposed, infected and recovered
        /// for comparison with SEIR-values
        /// </summary>
        private int sum_e, sum_i, sum_r;

        /// <summary>
        /// our list of still well humans
        /// will be sorted depending on contacts
        /// with infectious
        /// </summary>
        public List<NPC> susceptible;

        /// <summary>
        /// list of exposed, sorted by time of exposure
        /// </summary>
        public List<int> exp;
        /// <summary>
        /// list of infected, sorted by time of infection
        /// </summary>
        public List<int> inf;


        // Start is called before the first frame update
        void Start()
        {
            // Get our list of humans
            HumanInstants = GameObject.Find("HumanInstants");
            humans = HumanInstants.GetComponent<HumanInstants>();

            // Get up to date values from SEIR model
            seir = GetComponent<SEIR>();

            levelSettings = LevelSettings.GetActiveLevelSettings();

            // Set one time entries in our list
            // due to initially exposed or infected
            SetInitialEntries();
        }

        /// <summary>
        /// If there are initally exposed or infected humans
        /// we need them as inital entries in our list once 
        /// so that they are allowed to recover or get dead
        /// later on
        /// </summary>
        private void SetInitialEntries()
        {

            for (int i = 0; i < levelSettings.NumberOfNPCs; i++)
            {
                if (humans.npcs[i].GetCondition() == NPC.EXPOSED)
                {
                    exp.Add(humans.npcs[i].myID);

                }
                if (humans.npcs[i].GetCondition() == NPC.INFECTIOUS)
                {
                    inf.Add(humans.npcs[i].myID);
                }
            }
        }

        // Update is called once per frame
        void Update()
        {
            // build our list of susceptibles
            // and count current number of exposed, infected and recovred
            SortAndCountHumans();

            // get difference between current number of exposed, infected and recovered
            // compared to SEIR model predicitons
            int diff_E = (int)Math.Round(seir.E) - sum_e;
            int diff_I = (int)Math.Round(seir.I) - sum_i;
            int diff_R = (int)Math.Round(seir.R) - sum_r;

            // if there are too less exposed humans
            if (diff_E > 0)
            {
                // sort our susceptible list in order of their number of contacts with infectious
                // from top to bottom: highest to lowest contacts
                susceptible.Sort((x, y) => x.num_contacts.CompareTo(y.num_contacts));

                // iterate list of susceptibles startin with those with the most contacts
                // end of iteration is given by diff E or our list length
                for (int i = susceptible.Count - 1; i >= Math.Max(susceptible.Count - diff_E - 1, 0); i--)
                {
                    // make sure that these humans really had contact with infectious
                    if (susceptible[i].num_contacts > 0)
                    {
                        // these humans are than exposed, myID gives us the correct index after sorting
                        humans.npcs[susceptible[i].myID].SetCondition(NPC.EXPOSED);
                        // make a list of those humans who have been exposed first
                        exp.Add(susceptible[i].myID);
                    }
                }
            }

            // it too less are infected
            if (diff_I > 0)
            {
                // iterate up to diffI or the end of list exp.Count:
                // only those can become infecitous who have been exposed first
                for (int i = 0; i < Math.Min(diff_I, exp.Count); i++)
                {
                    // the incubation time might be a normal distribution
                    float rand_incubation_time = NextGaussian(seir.t_incubation, Mathf.Sqrt(seir.t_incubation - 0.2f));
                    // only if the incubation time is over, the human gets infectious
                    if (Time.fixedTime - humans.npcs[exp[i]].t_incubation > rand_incubation_time)
                    {
                        // infect the previously and longest exposed humans, again exp[i] stores myID
                        humans.npcs[exp[i]].SetCondition(NPC.INFECTIOUS);
                        // add human to list of infected, 
                        // again the order in this list tells us who has been infected first 
                        // and should recover first later on
                        inf.Add(exp[i]);
                        // remove from list of exposed
                        exp.RemoveAt(i);
                    }
                }
            }
            // eventually all exposed are allowed to recover / must die
            for (int i = 0; i < Math.Min(diff_R, inf.Count); i++)
            {
                // gaussian distribution around mean infectious time
                // this is the time in which the human is infecitous
                float rand_inf = NextGaussian(seir.t_infectious, Mathf.Sqrt(seir.t_infectious - 0.2f));
                // only if we have been infectious long enough
                if (Time.fixedTime - humans.npcs[inf[i]].t_infectious > seir.t_infectious)
                {
                    // 2% chance of death or else recovery
                    if (UnityEngine.Random.value < 0.02f)
                        humans.npcs[inf[i]].SetCondition(NPC.DEAD);
                    else
                        humans.npcs[inf[i]].SetCondition(NPC.RECOVERED);
                    //remove from infectious list
                    inf.RemoveAt(i);
                }
            }
            // ckear the list of susceptibles
            // keep all other lists
            susceptible.Clear();
        }

        /// <summary>
        /// Put all susceptible in one list
        /// Count current exposed, infectious and recovered
        /// </summary>
        private void SortAndCountHumans()
        {
            sum_e = sum_i = sum_r = 0;
            for (int i = 0; i < levelSettings.NumberOfNPCs; i++)
            {
                // list of susceptibles
                if (humans.npcs[i].GetCondition() == NPC.WELL)
                {
                    susceptible.Add(humans.npcs[i]);
                }
                // count others
                if (humans.npcs[i].GetCondition() == NPC.EXPOSED)
                    sum_e++;
                if (humans.npcs[i].GetCondition() == NPC.INFECTIOUS)
                    sum_i++;
                if (humans.npcs[i].GetCondition() == NPC.RECOVERED)
                    sum_r++;
            }
        }


        /// <summary>
        /// Function to sample from N(0,1) (Gaussian distribution)
        /// The Marsaglia polar method
        /// </summary>
        /// <returns> random sample from normal distribution </returns>
        public static float NextGaussian()
        {
            float v1, v2, s;
            //starts from point s on a uniformly distributed point in the interval(-1,+1). 
            do
            {
                // note: UnityEngine.Random.Range returns random float number 
                // between min [inclusive] and max [inclusive] 
                v1 = 2.0f * UnityEngine.Random.Range(0f, 1f) - 1.0f;
                v2 = 2.0f * UnityEngine.Random.Range(0f, 1f) - 1.0f;
                s = v1 * v1 + v2 * v2;
                // point should not be the origin
            } while (s >= 1.0f || Mathf.Abs(s) < 1e-06f);

            s = Mathf.Sqrt((-2.0f * Mathf.Log(s)) / s);
            return v1 * s;
        }

        /// <summary>
        /// Maps N(0,1) to arbitrary Gaussian curves
        /// </summary>
        /// <param name="mean"> the mean or expectation of the distribution </param>
        /// <param name="standard_deviation"> a measure of the amount of variation or dispersion of the values </param>
        /// <returns> return rand point from Gaussian distribution </returns>
        public static float NextGaussian(float mean, float standard_deviation)
        {
            return mean + NextGaussian() * standard_deviation;
        }
    }
}
