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
       
        // No special superpowers here.
        public override void SetSpriteImages()
        {
            WellSprite = Resources.Load<Sprite>("SmileyPictures/healty");
            InfectedSprite = Resources.Load<Sprite>("SmileyPictures/infected");
            IllSprite = Resources.Load<Sprite>("SmileyPictures/infected2");
            RecoveredSprite = Resources.Load<Sprite>("SmileyPictures/recovered3");
            DeadSprite = Resources.Load<Sprite>("SmileyPictures/dead2");
        }
    }
}
