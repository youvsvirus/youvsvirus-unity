using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using UnityEngine.SceneManagement;

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
        /// NPC prefab to be set in the editor.
        /// </summary>
        public GameObject friendPrefab;

        /// <summary>
        /// The instantiated player object.
        /// </summary>
        public Player Player { get; private set; }

        public Friend drunkFriend { get; private set; }
        

        /// <summary>

        /// All instantiated NPCs. This is a dynamic list, it can be extended during runtime.
        /// </summary>
        public List<NPC> NPCs { get; private set; }

        private LevelSettings levelSettings;

        private int numNPCs;
        //private SceneManager Scene;
        private Scene scene;
        public CreatePopulation()
        {
            NPCs = new List<NPC>(30);
        }

        // Awake is called the moment this component is created
        void Awake()
        {
            levelSettings = LevelSettings.GetActiveLevelSettings();

          

                //This gets the Main Camera from the Scene
                MainCamera = Camera.main;
            PlaceHumans();

        }

        private void Start()
        {



            //Debug.Log("Active Scene is '" + scene.name + "'.");
        }

        /// <summary>
        /// Places the player and all NPCs on the map randomly.
        /// </summary>
        private void PlaceHumans()
        {
            scene = SceneManager.GetActiveScene();
            numNPCs = levelSettings.NumberOfNPCs;
            if (scene.name == "YouVsVirus_Level2")
            {
                print(numNPCs);

                numNPCs = 110;
            }
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

            Vector3 origin = -MainCamera.GetComponent<ScreenEdgeColliders>().GetMapExtents();

            Vector2 screenBounds = MainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, MainCamera.transform.position.z));

            Vector3 coords = (scene.name == "YouVsVirus_Level1") ? GetCoordinatesInGrid(indices[0], columns, cellRadius, origin) : new Vector3(screenBounds.x-0.2f,screenBounds.y-0.2f,0f);

            if (friendPrefab.GetComponent<Friend>() == null)
                print("cheese");

            if (playerPrefab.GetComponent<Player>() == null)
                print("cheese2");
            //  if (scene.name == "YouVsVirus_Level2")
            // {
            drunkFriend = Instantiate(friendPrefab.GetComponent<Friend>(),
                                        new Vector3(-screenBounds.x + 0.2f, -screenBounds.y + 0.2f, 0f),    
                                        Quaternion.identity);
           // }

            //  Place the player
            Player = Instantiate(   playerPrefab.GetComponent<Player>(), 
                                    coords, 
                                    Quaternion.identity);
            // give the player a unique id
            Player.myID = numNPCs;
  
          
            // place 100 NPCs on dancefloor
           
            //  Place the NPCs in the grid
            for (int i = 1; i <= numNPCs; i++)
            {
                coords = (scene.name == "YouVsVirus_Level1") ? GetCoordinatesInGrid(indices[i], columns, cellRadius, origin) : new Vector3(2.5f, 0f, 0f);

                NPCs.Add(Instantiate(   npcPrefab.GetComponent<NPC>(),
                                        coords,
                                        Quaternion.identity));
                // give all npcs a unique id
                NPCs[i-1].myID = i - 1;
            }


          //  }

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
            Vector3 mapExtents = 2f * MainCamera.GetComponent<ScreenEdgeColliders>().GetMapExtents();

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
