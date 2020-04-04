using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Components
{
    /// <summary>
    /// Our NPCs
    /// </summary>
    public class NPC : HumanBase
    {
        // Start is called before the first frame update
        public override void Start()
        {
            base.Start(); // call base class
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
    }
}
