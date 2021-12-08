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

    public class AttackManager: MonoBehaviour
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */

        [SerializeField]
        public string nameID;

        [Title("Parameters")]
        [SerializeField]
        [OnValueChanged("SetAnimationData")]
        private AnimationClip attackAnimation;
        public AnimationClip AttackAnimation
        {
            get { return attackAnimation; }
        }
        private void SetAnimationData()
        {
            characterAnimationData.AnimationClipDebug = attackAnimation;
            nameID = this.gameObject.name;
        }

        [SerializeField]
        private CharacterAnimationData characterAnimationData;
        public CharacterAnimationData CharacterAnimationData
        {
            get { return characterAnimationData; }
        }

        [Title("General Properties")]
        // ==============================
        // à bouger dans un component (mais pas tout de suite parce que flemme et faut refactor plein de prefab)
        [SerializeField]
        private bool linkToCharacter = true;
        public bool LinkToCharacter
        {
            get { return linkToCharacter; }
        }

        [ShowIf("linkToCharacter")]
        [SerializeField]
        private bool fixToGround = false;
        public bool FixToGround
        {
            get { return fixToGround; }
        }
        // ===============================

        [SerializeField]
        private bool subActionCardBreakable = false;
        public bool SubActionCardBreakable
        {
            get { return subActionCardBreakable; }
        }

        [Title("Attack Controllers")]
        [SerializeField]
        [ListDrawerSettings(Expanded = true)]
        [ReadOnly]
        private AttackController[] atkControllers;




        CharacterBase user;


        float motionSpeed = 1f;
        public float MotionSpeed
        {
            get { return motionSpeed; }
        }



        #endregion

        #region GettersSetters 

        /* ======================================== *\
         *           GETTERS AND SETTERS            *
        \* ======================================== */

        [Button]
        public void SetAttackControllers()
        {
            atkControllers = GetComponentsInChildren<AttackController>();
            for (int i = 0; i < atkControllers.Length; i++)
            {
                atkControllers[i].SetAttackComponents();
            }
        }

        #endregion

        #region Functions 

        /* ======================================== *\
         *                FUNCTIONS                 *
        \* ======================================== */
        private void Update()
        {
            characterAnimationData.UpdateAnimation(user, user.MotionSpeed);
        }


        public void CreateAttack(CharacterBase cUser)
        {
            CreateAttack(null, cUser);
        }

        public void CreateAttack(Card attack, CharacterBase cUser)
        {
            // Auto correct direction pour QoL
            if (cUser.Inputs.InputLeftStickX.InputValue != 0)
                cUser.CharacterMovement.SetDirection((int)Mathf.Sign(cUser.Inputs.InputLeftStickX.InputValue));

            user = cUser;
            this.tag = cUser.tag;

            // Direction
            this.transform.localScale = new Vector3(this.transform.localScale.x * Mathf.Abs(user.SpriteRenderer.transform.localScale.x) * user.CharacterMovement.Direction,
                                                    this.transform.localScale.y * user.SpriteRenderer.transform.localScale.y,
                                                    user.SpriteRenderer.transform.localScale.z);

            if (linkToCharacter)
            {
                if(fixToGround)
                    this.transform.SetParent(user.Animator.transform);
                else
                    this.transform.SetParent(user.SpriteRenderer.transform);

                this.transform.localPosition = Vector3.zero;
                this.transform.localScale = new Vector3(Mathf.Abs(this.transform.localScale.x),
                                                        this.transform.localScale.y,
                                                        this.transform.localScale.z);
            }


            for (int i = 0; i < atkControllers.Length; i++)
            {
                atkControllers[i].InitAttack(attack, cUser);
            }

            if (subActionCardBreakable)
                user.CharacterAction.AddSubActionBreakable(this);
        }


        public void ActionActive(int subAttack = 0)
        {
            atkControllers[subAttack].ActionActive();
        }

        public void ActionUnactive(int subAttack = 0)
        {
            atkControllers[subAttack].ActionUnactive();
        }



        public void AttackMotionSpeed(float newMotionSpeed)
        {
            // d'après l'ordre d'execution de unity, characterAnimationData se fait manger une frame à cause de la coroutine qui gère le motion speed
            // donc j'appelle updateAnimation une nouvelle fois ici mais ce truc est potentiellement dangereux et casse des trucs dans certains cas
            if (motionSpeed == 0 && newMotionSpeed > 0)
            {
                characterAnimationData.UpdateAnimation(user, user.MotionSpeed); //Debug.Log("Je ratrappe le retard");
                for (int i = 0; i < atkControllers.Length; i++)
                {
                    atkControllers[i].UpdateComponents();
                }
            }

            motionSpeed = newMotionSpeed;

        }



        // Termine l'action ainsi que l'utilisateur
        public void ActionEnd()
        {
            user.CharacterAction.EndAction();
            CancelAction();
        }



        // Termine l'action
        public void CancelAction()
        {
            StopAllCoroutines();
            for (int i = 0; i < atkControllers.Length; i++)
            {
                if(atkControllers[i] != null)
                    atkControllers[i].CancelAction();
            }
            if (subActionCardBreakable)
                user.CharacterAction.RemoveSubActionBreakable(this);
            Destroy(this.gameObject);

        }


        #endregion

    } 

} // #PROJECTNAME# namespace