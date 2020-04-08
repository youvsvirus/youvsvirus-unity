using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

namespace Components
{
    public class HumanInstants : MonoBehaviour
    {
        public List<Vector2> RandomCoords;
        private float safetyMarginCellRadius = 0.2f;

        /// <summary>
        /// NPC prefab to be set in the editor.
        /// </summary>
        public GameObject npcPrefab { get; private set; }

        /// <summary>
        /// Player prefab to be set in the editor.
        /// </summary>
        public GameObject playerPrefab { get; private set; }

        /// <summary>
        /// The instantiated player object.
        /// </summary>
        public Player player { get; private set; }


        public int npcNumber { get; private set; }

        /// <summary>
        /// All instantiated NPCs. This is a dynamic list, it can be extended during runtime.
        /// </summary>
        public List<NPC> npcs { get; private set; }
       
       
         


        private float playerInfectionRadius;

        private float playerScale;

        private int humanNumber;
        private LevelSettings levelSettings;

        // Start is called before the first frame update
        void Start()
        {
            levelSettings = LevelSettings.GetActiveLevelSettings();
            npcNumber = levelSettings.NumberOfNPCs;
            humanNumber = npcNumber + 1;
            playerPrefab = Resources.Load("Player")     as GameObject;
            npcPrefab    = Resources.Load("DefaultNPC") as GameObject;
            playerScale = playerPrefab.transform.localScale.x;
            playerInfectionRadius = playerPrefab.GetComponentInChildren<InfectionTrigger>().InfectionRadius;
            npcs = new List<NPC>(npcNumber);
            GenerateRandomCoords();
            CreateHumans();
        }

        // Update is called once per frame
        void Update()
        {

        }

        void CreateHumans()
        {
            //  Place the player
            player = Instantiate(playerPrefab.GetComponent<Player>(),
                                RandomCoords[0],
                                Quaternion.identity);

            //  Place the NPCs in the grid
            for (int i = 1; i <= levelSettings.NumberOfNPCs; i++)
            {
                print(RandomCoords[i].x);
                npcs.Add(Instantiate(npcPrefab.GetComponent<NPC>(),
                                       RandomCoords[i],
                                       Quaternion.identity));
            }

            //  Infect a few of them.
            //  If (for some reason) NumberInitiallyExposed > NumberOfNPCs, just infect all of them.
            for (int i = 0; i < Math.Min(npcNumber, levelSettings.NumberInitiallyExposed); i++)
            {
                npcs[i].SetInitialCondition(NPC.EXPOSED);
            }
        }


        // Generate Random Grid
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Places the player and all NPCs on the map randomly.
        /// </summary>
        //private void PlaceHumans()
        void GenerateRandomCoords()
        {
            //  Determine the size of a single cell
            float cellRadius = GetCellRadius();
            float cellSidelength = 2f * cellRadius;

            //  Determine the total size of the grid
            int[] gridSize = GetGridSize(cellSidelength);
            int rows = gridSize[0];
            int columns = gridSize[1];
            int cellCount = rows * columns;

            //  Randomly select grid indices
            int[] indices = ChooseUnique(humanNumber, 0, cellCount);

            Vector3 origin = -GameObject.Find("MapLimits").GetComponent<ViewportBoundMapLimit>().GetMapExtents();
            for (int i = 0; i < humanNumber; i++)
            {
                RandomCoords.Add(GetCoordinatesInGrid(indices[i], columns, cellRadius, origin));
            }
        }


        /// <summary>
        ///     Compute the grid coordinates
        /// </summary>
        private Vector3 GetCoordinatesInGrid(int idx, int gridColumns, float cellRadius, Vector3 origin)
        {
            int row = idx / gridColumns;
            int col = idx % gridColumns;

            float x = (col + 1) * cellRadius;
            float y = (row + 1) * cellRadius;

            return origin + new Vector3(x, y, 0f);
        }

        /// <summary>
        ///     Calculates the radius of a single grid cell for placement on the map. The cell
        ///     radius is HALF it's sidelength.
        /// </summary>
        /// <returns>The cell radius.</returns>
        private float GetCellRadius()
        {
            //  Get the player's radius as the minimum cell size
            float cellRadius = (1f + safetyMarginCellRadius) * (playerInfectionRadius * playerScale);
            return cellRadius;
        }

        /// <summary>
        /// Calclates the number of rows and columns of a grid of quadratic cells of given cell sidelength on the map.
        /// </summary>
        /// <param name="cellSidelength">The side length of a single cell</param>
        /// <returns>An array of shape { rows, columns }</returns>
        private int[] GetGridSize(float cellSidelength)
        {
            Vector3 mapExtents = 2f * GameObject.Find("MapLimits").GetComponent<ViewportBoundMapLimit>().GetMapExtents();

            float mapWidth = 2f * mapExtents.x;
            float mapHeight = 2f * mapExtents.y;

            int columns = (int)(mapWidth / cellSidelength);
            int rows = (int)(mapHeight / cellSidelength);

            return new int[] { rows, columns };
        }

        /// <summary>
        /// Randomly chooses N unique integer values from the range [from, to - 1].
        /// </summary>
        /// <param name="N">The desired number of values.</param>
        /// <param name="from">The lower bound for the range to choose from. Inclusive.</param>
        /// <param name="to">The upper bound for the range to choose from. Exclusive.</param>
        /// <returns>Sequence of unique random integers from the given range.</returns>
        private int[] ChooseUnique(int N, int from, int to)
        {
            int count = to - from;
            if (to - from < count) throw new ArgumentException("N was larger than the range! ");

            int[] indices = Enumerable.Range(from, count).ToArray();
            shuffleInPlace(indices);
            return indices.Take(N).ToArray();
        }

        /// <summary>
        /// Shuffles a list of integers in-place using the Fisher-Yates permutation algorithm.
        /// </summary>
        /// <param name="list">The list to be shuffled</param>
        private void shuffleInPlace(int[] list)
        {
            for (int i = list.Length - 1; i > 0; i--)
            {
                //  Find source index.
                int j = UnityEngine.Random.Range(0, i + 1);

                //  Swap.
                int tmp = list[i];
                list[i] = list[j];
                list[j] = tmp;
            }
        }
    }
}
