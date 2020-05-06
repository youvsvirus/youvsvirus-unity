using UnityEngine;
using System.Collections;

namespace Components
{
    /// <summary>
    /// This is our dance floor, a circle-shaped edge collider
    /// </summary>
    [ExecuteInEditMode]
    [RequireComponent(typeof(EdgeCollider2D))]
    public class CircleEdgeCollider2D : MonoBehaviour
    {
        /// <summary>
        /// Radius of collider
        /// </summary>
        public float Radius = 5.0f;

        /// <summary>
        /// Number of points to build it
        /// </summary>
        public int NumPoints = 32;

        /// <summary>
        /// this is attached to an existing edge collider
        /// </summary>
        EdgeCollider2D EdgeCollider;

        /// <summary>
        /// helper variable to store the current radius
        /// </summary>
        float CurrentRadius = 0.0f;

        /// <summary>
        /// Start this instance.
        /// </summary>
        void Awake()
        {
            EdgeCollider = GetComponent<EdgeCollider2D>();
            CreateCircle();
        }

        /// <summary>
        /// Update this instance.
        /// </summary>
        void Update()
        {
            // If the radius or point count has changed, update the circle
            if (NumPoints != EdgeCollider.pointCount || CurrentRadius != Radius)
            {
                CreateCircle();
            }
        }

        /// <summary>
        /// Creates the circle.
        /// </summary>
        void CreateCircle()
        {
            Vector2[] edgePoints = new Vector2[NumPoints + 1];

            for (int loop = 0; loop <= NumPoints; loop++)
            {
                float angle = (Mathf.PI * 2.0f / NumPoints) * loop;
                edgePoints[loop] = new Vector2(Mathf.Sin(angle), Mathf.Cos(angle)) * Radius;
            }
            // give the Edge collider the points that define it
            EdgeCollider.points = edgePoints;
            // store current radius
            CurrentRadius = Radius;
        }
    }
}
