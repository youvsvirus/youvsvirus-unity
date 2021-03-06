﻿using UnityEngine;
using Pathfinding;


namespace Components
{
    /// <summary>
    /// Our NPC_AIs which do not move randomly but follow a path
    /// Velocity is set in editor
    /// </summary>
    public class NPC_AI : HumanBase
    {
        /// <summary>
        /// use pathfinding behaviour
        /// </summary>
        IAstarAI ai;

        /// <summary>
        /// The NPCs current destination, set by CreateHumans
        /// </summary>
        public Vector2 currentDest;

        /// <summary>
        /// The NPCs next destination, set by CreateHumans
        /// </summary>
        public Vector2 nextDest;



        // Start is called before the first frame update
        public override void Start()
        {
            base.Start(); // call base class
            ai = GetComponent<IAstarAI>();
        }

        /// <summary>
        /// Sprite images corresponding to infections states of npc
        /// </summary>
        public override void SetSpriteImages()
        {
            WellSprite = Resources.Load<Sprite>("SmileyPictures/npc_healthy");
            ExposedSprite = Resources.Load<Sprite>("SmileyPictures/npc_exposed");
            InfectiousSprite = Resources.Load<Sprite>("SmileyPictures/npc_infectious");
            RecoveredSprite = Resources.Load<Sprite>("SmileyPictures/recovered");
            DeadSprite = Resources.Load<Sprite>("SmileyPictures/npc_dead");
        }

        private void Update()
        {
            // Update the destination of the AI if
            // the AI is not already calculating a path and
            // the ai has reached the end of the path or it has no path at all
            // the last ifnrequent check sees if the ai is stuck, then it will go back to its last destination
            if (!ai.pathPending && (ai.reachedEndOfPath || !ai.hasPath) || (Time.frameCount%100 ==0 && Vector2.SqrMagnitude(ai.velocity) < 0.01f))
            {
                ai.destination = currentDest;
                ai.SearchPath();
                // swap current and next destination
                Vector2 tempDest = currentDest;
                currentDest = nextDest;
                nextDest = tempDest;
            }
        }
    }
}
