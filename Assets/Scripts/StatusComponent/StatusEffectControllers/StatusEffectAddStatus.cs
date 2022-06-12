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

namespace VoiceActing
{
    public class StatusEffectAddStatus : StatusEffect
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */
        [SerializeField]
        public StatusEffectData Status;
        [SerializeField]
        public bool ManualRemove = false;
        [SerializeField]
        public bool AddOnRemove = false;
        [SerializeField]
        public bool AddToTargets = false;

        CharacterBase target = null;
        #endregion

        #region GettersSetters 

        /* ======================================== *\
         *           GETTERS AND SETTERS            *
        \* ======================================== */

        #endregion

        #region Functions 

        /* ======================================== *\
         *                FUNCTIONS                 *
        \* ======================================== */
        public StatusEffectAddStatus()
        {
        }

        public StatusEffectAddStatus(StatusEffectAddStatus data)
        {
            Status = data.Status;
            ManualRemove = data.ManualRemove;
            AddToTargets = data.AddToTargets;
            AddOnRemove = data.AddOnRemove;
        }

        public override StatusEffect Copy()
        {
            return new StatusEffectAddStatus(this);
        }

        public override void ApplyEffect(CharacterBase character)
        {
            if (AddOnRemove)
                return;
            else if (AddToTargets)
            {
                if(character.tag == "Player") // On ajoute sur les ennemis 
                {
                    AddStatusToTag("Enemy", true);
                }
                else if (character.tag == "Enemy")
                {
                    AddStatusToTag("Player", true);
                }
            }
            else
            {
                character.CharacterStatusController.ApplyStatus(Status, 100);
            }
        }

        public override void RemoveEffect(CharacterBase character)
        {
            if(ManualRemove)
            {
                if (AddToTargets)
                {
                    if (character.tag == "Player") // On ajoute sur les ennemis 
                    {
                        AddStatusToTag("Enemy", false);
                    }
                    else if (character.tag == "Enemy")
                    {
                        AddStatusToTag("Player", false);
                    }
                }
                else
                {
                    character.CharacterStatusController.RemoveStatus(Status);
                }
            }
            else if (AddOnRemove)
            {
                character.CharacterStatusController.ApplyStatus(Status, 100);
            }
        }


        private void AddStatusToTag(string tag, bool add)
        {
            for (int i = 0; i < BattleUtils.Instance.Characters.Count; i++)
            {
                if(BattleUtils.Instance.Characters[i].tag == tag)
                {
                    if(add)
                    {
                        BattleUtils.Instance.Characters[i].CharacterStatusController.ApplyStatus(Status, 100);
                    }
                    else
                    {
                        BattleUtils.Instance.Characters[i].CharacterStatusController.RemoveStatus(Status);
                    }
                }
            }
        }

        #endregion

    } 

} // #PROJECTNAME# namespace