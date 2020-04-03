using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Components
{
    public class NPC : HumanBase
    {

        //private InfectionTrigger trigger;
        // Start is called before the first frame update
        //public override void Start()
        //{            
        //  base.Start();
        //   SetInitialHealthCondition(WELL);
        //GameObject InfectionTrigger = GameObject.Find("InfectionTrigger");
        //trigger = InfectionTrigger.GetComponent<InfectionTrigger>();
        //trigger.SetInfectionRadius(5);
        //}
        // Start is called before the first frame update
        public override void Start()
        {

            //	SetCondition(WELL);
            base.Start();
            //	SetInitialHealthCondition(WELL);
        }

        // No special superpowers here.
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
