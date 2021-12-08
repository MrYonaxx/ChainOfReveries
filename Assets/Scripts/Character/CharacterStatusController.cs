/*****************************************************************
 * Product:    #PROJECTNAME#
 * Developer:  #DEVELOPERNAME#
 * Company:    #COMPANY#
 * Date:       #CREATIONDATE#
******************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;

namespace VoiceActing
{
    public class CharacterStatusController: MonoBehaviour
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */

        CharacterBase character;

        List<Status> characterStatusEffect = new List<Status>();
        public List<Status> CharacterStatusEffect
        {
            get { return characterStatusEffect; }
        }

        public delegate void ActionStatus(List<Status> status);
        public event ActionStatus OnStatusChanged;

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

        public void InitializeComponent(CharacterBase characterBase)
        {
            character = characterBase;
            character.CharacterKnockback.OnHit += ApplyStatusOnHit;
        }



        public void ApplyStatusOnHit(DamageMessage damageMessage)
        {
            for (int i = 0; i < damageMessage.statusEffects.Count; i++)
            {
                ApplyStatus(damageMessage.statusEffects[i], damageMessage.statusEffectsChance[i]);
            }
        }


        public void ApplyStatus(StatusEffectData statusData, float statusChance)
        {

            float chance = statusChance - character.CharacterStat.GetStatusResistance(statusData);
            if (chance > 0)
            {
                Status status = new Status(statusData);
                characterStatusEffect.Add(status);

                // Applique l'effet et l'update
                for(int i = 0; i < status.StatusController.Count; i++)
                    status.StatusController[i].ApplyEffect(character);
                for (int i = 0; i < status.StatusUpdate.Count; i++)
                    status.StatusUpdate[i].ApplyUpdate(character);

                // Event
                RefreshStatus();
            }
        }


        public void UpdateController(CharacterBase character)
        {
            for(int i = characterStatusEffect.Count-1; i >= 0; i--)
            {
                // Update des effets
                for (int j = 0; j < characterStatusEffect[i].StatusController.Count; j++)
                {
                    characterStatusEffect[i].StatusController[j].UpdateEffect(character);
                }
                // Update de si on remove le status
                for (int j = 0; j < characterStatusEffect[i].StatusUpdate.Count; j++)
                {
                    if(characterStatusEffect[i].StatusUpdate[j].Update() == false)
                    {
                        RemoveStatus(character, i);
                        break;
                    }
                }
            }
        }


        private void RemoveStatus(CharacterBase character, int statusIndex)
        {
            // Remove le update behavior
            for (int j = 0; j < characterStatusEffect[statusIndex].StatusUpdate.Count; j++)
            {
                characterStatusEffect[statusIndex].StatusUpdate[j].Remove(character);
            }
            // Remove le behavior
            for (int j = 0; j < characterStatusEffect[statusIndex].StatusController.Count; j++)
            {
                characterStatusEffect[statusIndex].StatusController[j].RemoveEffect(character);
            }
            //character.CharacterStat.RemoveStatusEffect(statusIndex);
            characterStatusEffect.RemoveAt(statusIndex);

            // Event
            RefreshStatus();
        }




        public void RemoveAllStatus(CharacterBase character)
        {
            for (int i = 0; i < characterStatusEffect.Count; i++)
            {
                RemoveStatus(character, i);
            }
        }

        // Called when status neeed to refresh UI (more simple this way)
        public void RefreshStatus()
        {
            OnStatusChanged?.Invoke(characterStatusEffect);
        }

        #endregion

    } 

} // #PROJECTNAME# namespace