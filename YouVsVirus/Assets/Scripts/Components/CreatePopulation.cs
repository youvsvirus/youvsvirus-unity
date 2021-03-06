﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

namespace Components
{
    public class CreatePopulation : MonoBehaviour
    {
        private float safetyMargin = 0.2f;

        Camera MainCamera;
        /// <summary>
        /// Player prefab to be set in the editor.
        /// </summary>
        public GameObject playerPrefab;

        /// <summary>
        /// NPC prefab to be set in the editor.
        /// </summary>
        public GameObject npcPrefab;

        /// <summary>
        /// The instantiated player object.
        /// </summary>
        public Player Player { get; private set; }

        /// <summary>

        /// All instantiated NPCs. This is a dynamic list, it can be extended during runtime.
        /// </summary>
        public List<NPC> NPCs { get; private set; }

        private LevelSettings levelSettings;
        /// <summary>
        /// the screen dimensions
        /// </summary>
        private Vector2 screenBounds;

        public CreatePopulation()
        {
            NPCs = new List<NPC>(30);
        }

        // Awake is called the moment this component is created
        void Awake()
        {
            levelSettings = LevelSettings.GetActiveLevelSettings();
            LevelSettings.GetActiveLevelSettings().NumberOfNPCs = 25;
            //This gets the Main Camera from the Scene
            MainCamera = Camera.main;
            // transform screen dimenensions into world space
            screenBounds = MainCamera.GetComponent<CameraResolution>().GetMapExtents();
            PlaceHumans();
        }

        /// <summary>
        /// Places the player and all NPCs on the map randomly.
        /// </summary>
        private void PlaceHumans()
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
            int[] indices = ChooseUnique(levelSettings.NumberOfNPCs + 1, 0, cellCount);

            Vector2 origin = -screenBounds;

            //  Place the player
            Player = Instantiate(   playerPrefab.GetComponent<Player>(), 
                                    GetCoordinatesInGrid(indices[0], columns, cellRadius, origin), 
                                    Quaternion.identity);
            // give the player a unique id
            Player.myID = levelSettings.NumberOfNPCs;

            //  Place the NPCs in the grid
            for (int i = 1; i <= levelSettings.NumberOfNPCs; i++)
            {
                NPCs.Add(Instantiate(   npcPrefab.GetComponent<NPC>(),
                                        GetCoordinatesInGrid(indices[i], columns, cellRadius, origin),
                                        Quaternion.identity));
                // give all npcs a unique id
                NPCs[i-1].myID = i - 1;
            }

            //  Infect a few of them.
            //  If (for some reason) NumberInitiallyExposed > NumberOfNPCs, just infect all of them.
            for (int i = 0; i < Math.Min(levelSettings.NumberOfNPCs, levelSettings.NumberInitiallyExposed); i++)
            {
                NPCs[i].SetInitialCondition(NPC.EXPOSED);
            }
        }


        /// <summary>
        ///     Compute the grid coordinates
        /// </summary>
        private Vector2 GetCoordinatesInGrid(int idx, int gridColumns, float cellRadius, Vector2 origin)
        {
            int row = idx / gridColumns;
            int col = idx % gridColumns;

            float x = (col + 1) * cellRadius;
            float y = (row + 1) * cellRadius;

            return origin + new Vector2(x, y);
        }

        /// <summary>
        ///     Calculates the radius of a single grid cell for placement on the map. The cell
        ///     radius is HALF it's sidelength.
        /// </summary>
        /// <returns>The cell radius.</returns>
        private float GetCellRadius()
        {
            //  Get the player's radius as the minimum cell size
            float infectionRadius = playerPrefab.GetComponentInChildren<InfectionTrigger>().InfectionRadius;

            //  Get the player's scale.
            //  Assuming X and Y components of the scale to be identical.
            float playerScale = playerPrefab.transform.localScale.x;

            float cellRadius = (1f + safetyMargin) * (infectionRadius * playerScale);

            return cellRadius;
        }

        /// <summary>
        /// Calclates the number of rows and columns of a grid of quadratic cells of given cell sidelength on the map.
        /// </summary>
        /// <param name="cellSidelength">The side length of a single cell</param>
        /// <returns>An array of shape { rows, columns }</returns>
        private int[] GetGridSize(float cellSidelength)
        {
            Vector2 mapExtents = 2f * screenBounds;

            float mapWidth = 2f * mapExtents.x;
            float mapHeight = 2f * mapExtents.y;

            int columns = (int) (mapWidth / cellSidelength);
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
            if (count < N) throw new ArgumentException("N was larger than the range! ");

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
            for(int i = list.Length - 1; i > 0; i--)
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
