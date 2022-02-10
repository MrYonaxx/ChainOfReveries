using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;
using Sirenix.OdinInspector;
using Rewired;

namespace VoiceActing
{
    public class AIController : InputController
    {


        BehaviorTree behaviorTree = null;

        [Title("AI")]
        [SerializeField]
        bool aiEnabled = true;
        [SerializeField]
        private CharacterBase character;
        public CharacterBase Character
        {
            get { return character; }
            set { character = value; }
        }

        [SerializeField]
        AISleightData aiSleightData;
        public AISleightData SleightData
        {
            get { return aiSleightData; }
        }

        protected override void Awake()
        {
            // Initialise les struct pour les inputs
            base.Awake();
            // Initialise le behavior tree
            behaviorTree = GetComponent<BehaviorTree>();
            behaviorTree.SetVariableValue("AI", this);
            aiSleightData = new AISleightData();
            if (aiEnabled)
            {
                StartBehavior();
            }

            // Set le character controll" par le behvior tree
            SetControllable(character);
        }

        public void StartBehavior()
        {
            behaviorTree.enabled = true;
        }


        protected override void Update()
        {
            UpdateBuffer();
            if (controllable != null)
                controllable.UpdateControl(this);
        }



    }
}
