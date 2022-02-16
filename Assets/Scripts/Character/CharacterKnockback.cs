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
    public class CharacterKnockback: MonoBehaviour
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */

        [SerializeField]
        CharacterState stateKnockback;
        [SerializeField]
        CharacterState stateReversal;

        [SerializeField]
        float counterHitRevengeValue = -5;

        // Référence circulaire mais ça permet de tout simplifier
        CharacterBase character;

        List<KnockbackCondition> knockbackConditions = new List<KnockbackCondition>(); 

        protected bool noKnockback = false;
        public bool NoKnockback
        {
            get { return noKnockback; }
            set { noKnockback = value; }
        }

        private float knockbackTime = 0f;
        public float KnockbackTime
        {
            get { return knockbackTime; }
            set { knockbackTime = value; }
        }

        protected bool knockdown = false;
        public bool Knockdown
        {
            get { return knockdown; }
            set { knockdown = value; }
        }

        protected bool isInvulnerable = false;
        public bool IsInvulnerable
        {
            get { return isInvulnerable; }
            set { isInvulnerable = value; }
        }

        protected bool isDead = false;
        public bool IsDead
        {
            get { return isDead; }
            set { isDead = value; }
        }

        protected float revengeValue = 0f;
        public float RevengeValue
        {
            get { return revengeValue; }
        }

        protected bool canRevenge = false;
        public bool CanRevenge
        {
            get { return canRevenge; }
            set { canRevenge = value; }
        }


        protected List<AttackController> knockbackAttackController = new List<AttackController>();
        protected List<float> knockbackInvulnerability = new List<float>();

        public delegate void ActionDmgMsg(DamageMessage msg);
        public delegate void ActionCharacterDmgMsg(CharacterBase c, DamageMessage msg);
        public delegate void ActionDoubleFloat(float a, float b);

        public event ActionDmgMsg OnHit;
        public event ActionCharacterDmgMsg OnDeath;
        public event ActionDoubleFloat OnRVChanged;

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
        }





        public void AddKnockbackCondition(KnockbackCondition knockbackCondition)
        {
            knockbackConditions.Add(knockbackCondition);
        }
        public void RemoveKnockbackCondition(KnockbackCondition knockbackCondition)
        {
            knockbackConditions.Remove(knockbackCondition);
        }





        // Ajoute une attaque à la liste d'invulnérabilité, utilisé pour les multiHit et les AoE (sinon quand une attaque doit touché 2 personne, elle touche une personne puis se désactive et ne touche pas l'autre)
        public void KnockbackInvulnerability(AttackController attack, float attackInvulnerability)
        {
            knockbackAttackController.Add(attack);
            knockbackInvulnerability.Add(attackInvulnerability);
        }

        private bool CheckAttackInvulnerability(AttackController attack)
        {
            for (int i = 0; i < knockbackAttackController.Count; i++)
            {
                if (knockbackInvulnerability[i] > 0)
                {
                    if (attack == knockbackAttackController[i])
                        return true;
                }
            }
            return false;
        }


        private void UpdateAttackInvulnerability(float motionSpeed = 1)
        {
            for (int i = 0; i < knockbackAttackController.Count; i++)
            {
                knockbackInvulnerability[i] -= Time.deltaTime * motionSpeed;
                if (knockbackInvulnerability[i] <= 0)
                {
                    knockbackAttackController.RemoveAt(i);
                    knockbackInvulnerability.RemoveAt(i);
                }
            }
        }

        private void Update()
        {
            UpdateAttackInvulnerability(character.MotionSpeed);
            UpdateRevengeValue();
        }








        void OnTriggerEnter2D(Collider2D col)
        {
            Hit(col);
        }

        // A Optimiser peut être
        void OnTriggerStay2D(Collider2D col)
        {
            Hit(col);
        }

        public bool CheckIfHit(AttackController attack)
        {
            if (isInvulnerable)
                return false;
            if (CheckAttackInvulnerability(attack) == true)
                return false;
            if (attack.CheckCollisionY(this.transform.position.y) == false)
                return false;

            if (CheckRevenge(attack))
                return false;

            if (attack.IsUserDead()) // Pour pas créer de bug de double ko
                return false;

            return true;
        }

        public void Hit(Collider2D col)
        {
            if (!col.IsTouchingLayers(LayerMask.GetMask("CharacterHitbox")))
                return;
            if ((col.tag == "Enemy" || col.tag == "Player") && this.tag != col.tag)
            {
                AttackController attackIncoming = col.GetComponent<IAttack>().GetAttack();
                Hit(attackIncoming);
            }
        }

        public void Hit(AttackController attackIncoming)
        {
            if (CheckIfHit(attackIncoming) == true)
            {
                // Process Attack
                // On créer un damageMessage avec les AttackProcessor de l'attaque
                DamageMessage damageMessage = attackIncoming.GetDamageMessage(character);


                // Une fois terminé on a un damageMessage avec toutes les infos, on check ce damageMessage avec [KnockbackCondition/DefenseProcessor]
                //      Effet possible de défense : Bloque la première attaque
                //      Bloque une attaque de type feu
                //      Si on inflige plus de 1500 points de dégats, dégats = 0
                //      Status Paradoxe de FF12 (damage = -damage)
                for (int i = 0; i < knockbackConditions.Count; i++)
                {
                    if (knockbackConditions[i].CheckCondition(character, attackIncoming, damageMessage) == true) // Ajouter en argument DmgMsg (et ptet renommé la classe)
                        return;
                }

                // On applique le DamageMessage final
                attackIncoming.ApplyDamageMessage(character, damageMessage);

                // On enlève le pv et on décide si on knockback
                Hit(damageMessage);

                // Enfin on appelle les effets de l'attaque
                attackIncoming.HasHit(character);
            }
        }

        public void Hit(DamageMessage damageMessage)
        {
            character.CharacterStat.HP -= (int)damageMessage.damage;
            if (character.CharacterStat.HP <= 0)
            {
                Knockback(1, false);
                if (isDead == false)
                {
                    isDead = true;
                    ResetRevengeValue();
                    OnDeath?.Invoke(character, damageMessage);
                }
            }
            else if(damageMessage.knockback > 0)
            {
                // On ne mets pas le state knockback, c'est l'attaque qui doit le faire (certaines attaques font des dommages mais ne knockback pas)
                // Mon moi du futur, C'est le spaghetti mais c'est le prix à payer pour finir le jeu au lieu de refactor
                Knockback(1, false);
            }

            OnHit?.Invoke(damageMessage);
        }


        public void Death()
        {

        }


        public void Knockback(float multiplier = 1, bool setState = false)
        {
            knockbackTime = character.CharacterStat.KnockbackTime.Value * multiplier;

            if(character.State.ID == CharacterStateID.Acting) // Counter Hit
            {
                AddRevengeValue(counterHitRevengeValue);
            }

            if(setState)
                character.SetState(stateKnockback);
        }



        // ============================================================
        // Revenge Value

        public void AddRevengeValue(float value)
        {
            if (isDead)
                return;
            revengeValue += value;
            revengeValue = Mathf.Clamp(revengeValue, 0, character.CharacterStat.RevengeValue.Value);
            OnRVChanged?.Invoke(revengeValue, character.CharacterStat.RevengeValue.Value);
        }

        public void ResetRevengeValue()
        {
            revengeValue = 0;
            OnRVChanged?.Invoke(revengeValue, character.CharacterStat.RevengeValue.Value);
        }

        private void UpdateRevengeValue()
        {
            if(knockbackTime <= 0 && revengeValue > 0)
            {
                AddRevengeValue(-0.5f * Time.deltaTime);
            }
        }

        public bool CheckRevenge(AttackController attackManager)
        {
            if (character.CharacterStat.RevengeValue.Value <= 0)
                return false;

            if (canRevenge && revengeValue >= character.CharacterStat.RevengeValue.Value)
            {
                //character.DeckController.SelectCard();
                //reversalAttack = attackManager;
                ResetRevengeValue();
                character.SetState(stateReversal);
                return true;
            }
            return false;
        }



        // ============================================================
        // Test

        private float reversalTime = 0;
        public float ReversalTime
        {
            get { return reversalTime; }
            set { reversalTime = value; }
        }

       /* private AttackController reversalAttack;
        public AttackController ReversalAttack
        {
            get { return reversalAttack; }
        }


        public bool CheckReversal(AttackController attackManager)
        {
            if(reversalTime > 0)
            {
                if (attackManager.Card.CardData.CardBreakComponent.CheckCardBreak(attackManager.Card, character.DeckController.GetCurrentCard()) == 1)
                {
                    character.DeckController.SelectCard();
                    reversalAttack = attackManager;
                    character.SetState(stateReversal);
                    return true;
                }
            }
            return false;
        }*/

        // ============================================================

        #endregion

    } 

} // #PROJECTNAME# namespace