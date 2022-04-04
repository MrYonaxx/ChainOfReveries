using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace VoiceActing
{
    // Utilisé par Shimérie
    public class CharAnimation_MoveToPoint4 : CharacterAnimationEvent
    {
        [Space]
        [InfoBox("Move à un point random avec une distance minmum entre le perso et la target")]
        [SerializeField]
        float distance = 3;
        [SerializeField]
        Vector2 arenaBoundX =  new Vector2(-5, 5);

        public override void Execute(CharacterBase character)
        {
            CharacterBase target = character.LockController.TargetLocked;

            int canTPBoth = 0;
            Vector3 pointLeft = Vector3.zero;
            Vector3 pointRight = Vector3.zero;
            Vector3 point = Vector3.zero;
            if ((target.transform.position.x - distance) > BattleUtils.Instance.BattleCenter.position.x + arenaBoundX.x)
            {
                // On peut se TP à gauche
                pointLeft = new Vector3(Random.Range(BattleUtils.Instance.BattleCenter.position.x + arenaBoundX.x, (target.transform.position.x - distance)), Random.Range(-1, 1), 0);
                point = pointLeft;
                canTPBoth += 1;
            }
            if ((target.transform.position.x + distance) < BattleUtils.Instance.BattleCenter.position.x + arenaBoundX.y)
            {
                pointRight = new Vector3(Random.Range((target.transform.position.x + distance), BattleUtils.Instance.BattleCenter.position.x + arenaBoundX.y), Random.Range(-1, 1), 0);
                point = pointRight;
                canTPBoth += 1;
            }

            if(canTPBoth == 2)
            {
                if(Random.Range(0, 2) == 0)
                    character.transform.position = pointLeft;
                else
                    character.transform.position = pointRight;
                return;
            }
            character.transform.position = point;
        }


    }
}
