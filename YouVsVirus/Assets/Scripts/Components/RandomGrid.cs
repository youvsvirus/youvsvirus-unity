using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

namespace Components
{
    public class RandomGrid : MonoBehaviour
    {
        /// <summary>
        /// a safety margin to ensure that all cells are large enough
        /// </summary>
        public float safetyMarginCellRadius = 0.2f;

        /// <summary>
        /// to place the NPCs more in the middle of the screen
        /// we can shrink the bounds
        /// e.g. when we want the player to spawn at the edge of the screen
        /// and the NPCs not to get him directly
        /// </summary>
        public void shrinkScreenBounds(float factor)
        {
            screenBounds = screenBounds * factor;
        }

        /// <summary>
        /// a list of random coords generated so that all prefab clones
        /// are placed on a grid in safe distance from each other
        /// </summary>
        public List<Vector2> RandomCoords;

        /// <summary>
        /// the screen dimensions
        /// </summary>
        public Vector2 screenBounds;


        Camera MainCamera;

        private void Start()
        {
            // this gets the Main Camera from the Scene
            MainCamera = Camera.main;
            // set screen bounds
            screenBounds = MainCamera.GetComponent<CameraResolution>().GetMapExtents();
        }


        /// <summary>
        /// compute a list of random coords generated so that all prefab clones
        /// are placed on a grid in safe distance from each other
        /// <param name="scale"> e.g. the player's scale needed for grid size </param>
        /// <param name="infection_radius"> e.g. the player's infection radius for grid size </param>
        /// <param name="npcNumber"> number of prefabs clones to be placed, player is +1 </param>
        /// <param name="nonSpawnable"> space in which no NPC should spawn </param>
        /// </summary>
        public void GenerateRandomCoords(float scale, float infection_radius, int npcNumber, nonSpawnableSpace nonSpawnable)
        {
            //  Determine the size of a single cell
            float cellRadius = GetCellRadius(scale, infection_radius);
            float cellSidelength = 2f * cellRadius;

            //  Determine the total size of the grid
            int[] gridSize = GetGridSize(cellSidelength);
            int rows = gridSize[0];
            int columns = gridSize[1];
            int cellCount = rows * columns;
            bool notAllAreSpawnable = true;

            Vector2 origin = -screenBounds;

            // Create random coordinates until all are in spawnable space
            // This way it is really ineffective, because we create all coords and
            // then check if they are spawnable and if not create all again.
            // However, we should take the isSpawnable check into ChooseUnique
            // and i currently do not know how to do this.
            //
            // Since this may crash the program, either because it just
            // takes too long, or because the spawnable space is just too small to
            // spawn all smileys, we give up after 50 attempts and just use
            // the last configuration. Even though some may now live in unspawnable space.
            int[] indices = null;
            int countAttempt = 0;
            const int maxAttempts = 50;
            while (notAllAreSpawnable && countAttempt < maxAttempts) 
            {
                //  Randomly select grid indices
                indices = ChooseUnique(npcNumber + 1, 0, cellCount);

                notAllAreSpawnable = false;
                for (int i = 0; i < npcNumber + 1; i++)
                {
                    if (!nonSpawnable.coordinatesAreSpawnable2D(GetCoordinatesInGrid(indices[i], columns, cellRadius, origin)))
                    {
                        // This NPC is non-spawnable. Set notAllAreSpawnable to true and exit the for loop (by setting i = npcNumber, (i dont like break))
                        i = npcNumber;
                        notAllAreSpawnable = true;
                    }
                }
            }
            for (int i = 0; i < npcNumber + 1; i++)
            {
                RandomCoords.Add(GetCoordinatesInGrid(indices[i], columns, cellRadius, origin));
            }
        }

        /// <summary>
        /// Compute the grid coordinates
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
        /// Calculates the radius of a single grid cell for placement on the map. The cell
        /// radius is HALF it's sidelength.
        /// </summary>
        /// <returns>The cell radius.</returns>
        private float GetCellRadius(float scale, float infection_radius)
        {
            //  Get the player's radius as the minimum cell size
            float cellRadius = (1f + safetyMarginCellRadius) * (infection_radius * scale);
            return cellRadius;
        }

        /// <summary>
        /// Calclates the number of rows and columns of a grid of quadratic cells of given cell sidelength on the map.
        /// </summary>
        /// <param name="cellSidelength">The side length of a single cell</param>
        /// <returns>An array of shape { rows, columns }</returns>
        private int[] GetGridSize(float cellSidelength)
        {
            Vector2 mapExtents = 2f * screenBounds; ;

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
