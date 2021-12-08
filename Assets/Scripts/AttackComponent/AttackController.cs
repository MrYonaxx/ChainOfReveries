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
using Sirenix.Serialization;

namespace VoiceActing
{
    public class AttackController: MonoBehaviour, IAttack
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */
        private Card card = null;
        public Card Card
        {
            get { return card; }
        }


        [SerializeField]
        Collider2D hitbox = null;
        [SerializeField]
        BoxCollider2D hitboxY = null;



        [Title("Attack Behaviour")]
        [SerializeField]
        bool linkHitboxYToAttack = false;

        [SerializeField]
        [ListDrawerSettings(Expanded = true)]
        [ReadOnly]
        AttackProcessorBattle[] attackProcessorsComponents = null;

        [SerializeField]
        [ListDrawerSettings(Expanded = true)]
        [ReadOnly]
        private AttackComponent[] attackComponents = null;




        CharacterBase user = null;
        /*public CharacterBase User
        {
            get { return user; }
        }*/

        private int direction;
        public int Direction
        {
            get { return direction; }
            set { direction = value; }
        }


        private List<AttackProcessor> attackProcessors = null;
        public List<AttackProcessor> AttackProcessors
        {
            get { return attackProcessors; }
        }


        //float motionSpeed = 1f;

        #endregion


        #region Functions 
        /* ======================================== *\
         *                FUNCTIONS                 *
        \* ======================================== */

        [Button]
        public void SetAttackComponents()
        {
            attackComponents = null;
            attackComponents = GetComponentsInChildren<AttackComponent>(true);
            attackProcessorsComponents = null;
            attackProcessorsComponents = GetComponentsInChildren<AttackProcessorBattle>(true);
        }

        // Pour l'interface IAttack
        public AttackController GetAttack()
        {
            return this;
        }


        public void Update()
        {
            // À opti (un jour)
            if(hitboxY != null && linkHitboxYToAttack == false)
                hitboxY.transform.position = new Vector3(hitboxY.transform.position.x, user.transform.position.y, hitboxY.transform.position.y);
            UpdateComponents();
        }

        public void InitAttack(Card attack, CharacterBase cUser)
        {
            card = attack;
            user = cUser;
            hitbox.gameObject.tag = cUser.tag;

            direction = user.CharacterMovement.Direction;

            // Hitbox
            hitbox.enabled = false;

            // Attack Processor
            attackProcessors = new List<AttackProcessor>(attackProcessorsComponents.Length);
            for (int i = 0; i < attackProcessorsComponents.Length; i++)
            {
                attackProcessors.Add(attackProcessorsComponents[i].GetAttackProcessor());
            }

            // Attack Components
            for (int i = 0; i < attackComponents.Length; i++)
            {
                attackComponents[i].StartComponent(user, this);
            }

        }



        public bool CheckCollisionY(float posY)
        {
            if (hitboxY == null)
                return true;
            float scaleY = user.SpriteRenderer.transform.localScale.y;
            return ((hitboxY.transform.position.y + ((hitboxY.offset.y - (hitboxY.size.y / 2)) * scaleY)) <= posY && posY <= (hitboxY.transform.position.y + ((hitboxY.offset.y + (hitboxY.size.y / 2)) * scaleY)));
        }


        public void HasHit(CharacterBase target)
        {
            for (int i = 0; i < attackComponents.Length; i++)
            {
                attackComponents[i].OnHitComponent(user, target);
            }
            user.CharacterAction.AttackHit(this, target);
        }





        public DamageMessage GetDamageMessage(CharacterBase target)
        {
            DamageMessage damageMessage = new DamageMessage();
            if (attackProcessors == null)
                return damageMessage;

            // Attack processor inhérent à l'attaque
            for (int i = 0; i < attackProcessors.Count; i++)
            {
                attackProcessors[i].ProcessAttack(user, target, this, ref damageMessage);
            }

            // Attack proccessor de l'utilisateur (généralement provenant de status)
            for (int i = 0; i < user.CharacterAction.AdditionalAttackProcessor.Count; i++)
            {
                user.CharacterAction.AdditionalAttackProcessor[i].ProcessAttack(user, target, this, ref damageMessage);
            }

            return damageMessage;
        }


        public void ApplyDamageMessage(CharacterBase target, DamageMessage damageMessage)
        {
            if (attackProcessors == null)
                return;

            // Applique les règles custom (genre le lifesteal)
            for (int i = 0; i < attackProcessors.Count; i++)
            {
                attackProcessors[i].ApplyCustomProcess(user, target, this, ref damageMessage);
            }

            // Attack proccessor de l'utilisateur (généralement provenant de status)
            for (int i = 0; i < user.CharacterAction.AdditionalAttackProcessor.Count; i++)
            {
                user.CharacterAction.AdditionalAttackProcessor[i].ApplyCustomProcess(user, target, this, ref damageMessage);
            }
        }


        public void UpdateComponents()
        {
            for (int i = 0; i < attackComponents.Length; i++)
            {
                attackComponents[i].UpdateComponent(user);
            }
        }


        public void ActionActive()
        {
            hitbox.enabled = true;
        }

        public void ActionUnactive()
        {
            hitbox.enabled = false;
        }

        public void CancelAction()
        {
            for (int i = 0; i < attackComponents.Length; i++)
            {
                attackComponents[i].EndComponent(user);
            }
        }


        #endregion

    } 

} // #PROJECTNAME# namespace