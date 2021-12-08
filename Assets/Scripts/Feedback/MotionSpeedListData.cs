using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace VoiceActing
{
    [CreateAssetMenu(fileName = "BattleFeedbackManagerData", menuName = "BattleFeedbackManagerData", order = 1)]
    public class MotionSpeedListData : ScriptableObject
    {
        [Title("Global Parameters")]
        [SerializeField]
        float value = 0;



        private List<IMotionSpeed> charactersScene = new List<IMotionSpeed>();
        public List<IMotionSpeed> CharactersScene
        {
            get { return charactersScene; }
        }






        public void AddCharacter(IMotionSpeed character)
        {
            charactersScene.Add(character);
        }

        public void RemoveCharacter(IMotionSpeed character)
        {
            charactersScene.Remove(character);
        }





        public void SetBattleMotionSpeed(float motionSpeed, float time)
        {
            for (int i = 0; i < charactersScene.Count; i++)
            {
                charactersScene[i].SetCharacterMotionSpeed(motionSpeed, time);
            }
        }


    }
}
