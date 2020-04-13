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
        /// Difference between our sum of E,I,R
        /// and the last max the SEIR model predicted
        /// </summary>
        private int diff_E, diff_I, diff_R;

        /// <summary>
        /// our list of still well humans
        /// will be sorted depending on contacts
        /// with infectious
        /// </summary>
        public List<HumanBase> susceptible;

        /// <summary>
        /// list of exposed, sorted by time of exposure
        /// </summary>
        public List<int> exp;
        /// <summary>
        /// list of infected, sorted by time of infection
        /// </summary>
        public List<int> inf;

        /// <summary>
        /// list of recovered, sorted by time of recovery
        /// </summary>
        public List<int> rec;

        /// <summary>
        /// number of npcs + player
        /// </summary>
        private int numHumans;

        // Start is called before the first frame update
        void Start()
        {
            // Get our list of humans
            HumanInstants = GameObject.Find("HumanInstants");
            humans = HumanInstants.GetComponent<HumanInstants>();

            // Get up to date values from SEIR model
            seir = GetComponent<SEIR>();

            levelSettings = LevelSettings.GetActiveLevelSettings();
            numHumans = levelSettings.NumberOfNPCs + 1;

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

            for (int i = 0; i < numHumans; i++)
            {
                if (humans.all[i].GetCondition() == NPC.EXPOSED)
                {
                    exp.Add(humans.all[i].myID);

                }
                if (humans.all[i].GetCondition() == NPC.INFECTIOUS)
                {
                    inf.Add(humans.all[i].myID);
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
            // compared to SEIR model predictions
            // if the maximum infectious spread has been reached, the SEIR values go down
            // at that point we let the infection spread on its own (this is why we take the max)
             diff_E = ((int)Math.Round(seir.max_E) - sum_e);
             diff_I = ((int)Math.Round(seir.max_I) - sum_i);
             diff_R = ((int)Math.Round(seir.max_R) - sum_r);

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
                        humans.all[susceptible[i].myID].SetCondition(NPC.EXPOSED);
                        // make a list of those humans who have been exposed first
                        exp.Add(susceptible[i].myID);
                    }
                }
            }


            // it too less are infected
            // iterate up to diffI or the end of list exp.Count:
            // only those can become infecitous who have been exposed first
            for (int i = 0; i < Math.Min(diff_I, exp.Count); i++)
            {
                // the incubation time might be a normal distribution
                float rand_incubation_time = seir.t_incubation; // NextGaussian(seir.t_incubation, Mathf.Sqrt(seir.t_incubation - 0.2f));
                // only if the incubation time is over, the human gets infectious
                if (Time.fixedTime - humans.all[exp[i]].t_incubation > rand_incubation_time)
                {              
                    // infect the previously and longest exposed humans, again exp[i] stores myID
                    humans.all[exp[i]].SetCondition(NPC.INFECTIOUS);
                    // add human to list of infected, 
                    // again the order in this list tells us who has been infected first 
                    // and should recover first later on
                    inf.Add(exp[i]);
                }
            }
            // remove infectious from list of exposed
            foreach (int item in inf) { exp.Remove(item); }

            // eventually all exposed are allowed to recover / must die
            for (int i = 0; i < Math.Min(diff_R, inf.Count); i++)
            {
                // gaussian distribution around mean infectious time
                // this is the time in which the human is infecitous
                float rand_inf = seir.t_infectious; // NextGaussian(seir.t_infectious, Mathf.Sqrt(seir.t_infectious - 0.2f));
                                                    // only if we have been infectious long enough
                if (Time.fixedTime - humans.all[inf[i]].t_infectious > seir.t_infectious)
                {
                    // 2% chance of death or else recovery
                    if (UnityEngine.Random.value < 0.02f)
                    {
                        humans.all[inf[i]].SetCondition(NPC.DEAD);
                    }
                    else
                        humans.all[inf[i]].SetCondition(NPC.RECOVERED);
                    // add to list of recovered, which we only use to remove from inf
                    rec.Add(inf[i]);
                }
            }
            //remove from infectious list
            foreach (int item in rec) { inf.Remove(item); }

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
            for (int i = 0; i < humans.all.Count; i++)
            {
                // list of susceptibles
                if (humans.all[i].GetCondition() == NPC.WELL)
                {
                    susceptible.Add(humans.all[i]);
                }
                // count others
                if (humans.all[i].GetCondition() == NPC.EXPOSED)
                    sum_e++;
                if (humans.all[i].GetCondition() == NPC.INFECTIOUS)
                    sum_i++;
                if (humans.all[i].GetCondition() == NPC.RECOVERED)
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
