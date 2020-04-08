using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Components
{
    public class Spread : MonoBehaviour
    {
        private SEIR seir;
        private LevelSettings levelSettings;
        private LevelStats levelStats;

        // Start is called before the first frame update
        void Start()
        {
            GameObject SEIR = GameObject.Find("SEIR");
            seir = SEIR.GetComponent<SEIR>();

           // GameObject CreateHumans = GameObject.Find("CreateHumans");
           // population = CreateHumans.GetComponent<CreatePopulation>();
           
            levelSettings = LevelSettings.GetActiveLevelSettings();
            // Get the statistics object that counts the numbers of infected/dead etc players
            levelStats = LevelStats.GetActiveLevelStats();
            //for (int i = 1; i <= levelSettings.NumberOfNPCs; i++)
              //  print(population.NPCs[i]);
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
