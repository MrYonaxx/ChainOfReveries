/*****************************************************************
 * Product:    #PROJECTNAME#
 * Developer:  #DEVELOPERNAME#
 * Company:    #COMPANY#
 * Date:       #CREATIONDATE#
******************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Menu;

namespace VoiceActing
{
    public class AttackC_Scan: AttackComponent
    {
        [SerializeField]
        MenuScan menuScan = null;

        CharacterBase user = null;
        Transform camTarget = null;

        public override void StartComponent(CharacterBase character, AttackController attack)
        {
            if(character.LockController.TargetLocked == null)
            {
                character.CharacterAction.EndAction();
                return;
            }

            user = character;
            camTarget = character.LockController.TargetLocked.SpriteRenderer.transform;
            BattleFeedbackManager.Instance?.CameraController.AddTarget(camTarget, 999);
            BattleFeedbackManager.Instance?.SetBattleMotionSpeed(0);

            character.SetCharacterMotionSpeed(0);
            character.Inputs.SetControllable(menuScan, true);

            menuScan.DrawCharacter(character.LockController.TargetLocked.CharacterData);
            menuScan.InitializeMenu();
            menuScan.OnEnd += Quit;
        }

        /*public override void UpdateComponent(CharacterBase character)
        {
            base.UpdateComponent(character);

            menuScan.UpdateControl(character.Inputs);
        }*/

        private void Quit()
        {
            menuScan.OnEnd -= Quit;
            BattleFeedbackManager.Instance?.CameraController.RemoveTarget(camTarget);
            BattleFeedbackManager.Instance?.SetBattleMotionSpeed(1);

            user.SetCharacterMotionSpeed(1);
            user.Inputs.SetControllable(user);
            user.CharacterAction.EndAction();
        }






    } 

} // #PROJECTNAME# namespace