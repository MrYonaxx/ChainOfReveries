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
    public class CharacterBase: MonoBehaviour, IMotionSpeed, IControllable
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */
        [SerializeField]
        protected Transform particlePoint;
        public Transform ParticlePoint
        {
            get { return particlePoint; }
        }

        [SerializeField]
        protected SpriteRenderer spriteRenderer;
        public SpriteRenderer SpriteRenderer
        {
            get { return spriteRenderer; }
        }

        [SerializeField]
        Animator animator;
        public Animator Animator
        {
            get { return animator; }
        }

        // Objet avec tout les scripts de feedbacks
        [SerializeField]
        private Transform feedbacksComponents;
        public Transform FeedbacksComponents
        {
            get { return feedbacksComponents; }
        }

        [Title("States Machines")]
        [SerializeField]
        protected CharacterState currentState;
        public CharacterState State
        {
            get { return currentState; }
        }

        [SerializeField]
        protected CharacterState stateIdle;
        public CharacterState StateIdle
        {
            get { return stateIdle; }
        }

        [Title("Stats")]
        [SerializeField]
        protected CharacterData characterData;
        public CharacterData CharacterData
        {
            get { return characterData; }
        }

        [SerializeField]
        protected CharacterStat characterStat;
        public CharacterStat CharacterStat
        {
            get { return characterStat;}
        }

        /*[SerializeField]
        protected CharacterStatController characterStat;
        public CharacterStatController CharacterStat
        {
            get { return characterStat; }
        }*/


        // Les composants basico basiques
        [Title("Components")]
        [SerializeField]
        protected CharacterRigidbody characterRigidbody;
        public CharacterRigidbody CharacterRigidbody
        {
            get { return characterRigidbody; }
        }

        [Title("Components Coupled")]
        [SerializeField]
        protected CharacterMovement characterMovement;
        public CharacterMovement CharacterMovement
        {
            get { return characterMovement; }
        }

        [SerializeField]
        protected CharacterAction characterAction;
        public CharacterAction CharacterAction
        {
            get { return characterAction; }
        }


        [SerializeField]
        protected CharacterKnockback characterKnockback;
        public CharacterKnockback CharacterKnockback
        {
            get { return characterKnockback; }
        }

        [SerializeField]
        protected CharacterEquipment characterEquipment;
        public CharacterEquipment CharacterEquipment
        {
            get { return characterEquipment; }
        }



        // Les composants lié au gameplay carte
        [Title("Components Cards")]
        [SerializeField]
        protected DeckController deckController;
        public DeckController DeckController
        {
            get { return deckController; }
        }

        [SerializeField]
        protected SleightController sleightController;
        public SleightController SleightController
        {
            get { return sleightController; }
        }




        [SerializeField]
        protected TargetController lockController;
        public TargetController LockController
        {
            get { return lockController; }
        }

        // pour y acceder plus simplement
        protected InputController inputController;
        public InputController Inputs
        {
            get { return inputController; }
        }



        [Title("Optionals")]
        [SerializeField]
        protected CharacterStatusController characterStatusController;
        public CharacterStatusController CharacterStatusController
        {
            get { return characterStatusController; }
        }



        [Title("Motion Speed")]
        [SerializeField]
        private float motionSpeed = 1;
        public float MotionSpeed
        {
            get { return Mathf.Max(motionSpeed * characterStat.MotionSpeed.Value, 0); }
        }



        CharacterState oldState;
        public CharacterState OldState
        {
            get { return oldState; }
        }



        bool init = false;
        bool canPlay = true;
        IEnumerator motionSpeedCoroutine;

        public delegate void Action();
        public delegate void ActionSetState(CharacterState oldState, CharacterState newState);


        public event ActionSetState OnStateChanged;
        public event Action OnBattleStart;
        public event Action OnBattleEnd;

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
        public void SetCharacter()
        {
            if (characterData != null)
                SetCharacter(characterData);
        }

        public void SetCharacter(CharacterData data)
        {
            if (init == true)
                return;
            init = true;
            canPlay = true;
            characterData = data;
            characterStat = new CharacterStat(characterData.CharacterStat);

            // On initialise le deck
            sleightController.SetSleightDatabase(data.SleightDatabase);
            deckController.SetDeck(data.InitialDeck);

            characterMovement.InitializeComponent(characterStat);
            characterAction.InitializeComponent(this);
            characterKnockback.InitializeComponent(this);
            characterStatusController.InitializeComponent(this);
            characterEquipment.InitializeComponent(this);



        }


        protected virtual void Start()
        {
            BattleFeedbackManager.Instance.AddIMotionSpeed(this);
            SetCharacter();
        }

        protected virtual void OnDestroy()
        {
            BattleFeedbackManager.Instance.RemoveIMotionSpeed(this);
            characterStatusController?.RemoveAllStatus();
        }

        public void SetState(CharacterState characterState)
        {
            if (currentState != null)
                currentState.EndState(this, characterState);

            oldState = currentState;
            currentState = characterState;

            currentState.StartState(this, oldState);

            OnStateChanged?.Invoke(oldState, currentState);
        }

        public void ResetToIdle()
        {
            SetState(stateIdle);
        }

        public void CanPlay(bool b)
        {
            canPlay = b;
        }

        public void UpdateControl(InputController inputs)
        {
            if (canPlay)
                inputController = inputs;
            else
            {
                inputController.ResetAllBuffer();
                inputController.ResetAllValue();

            }

            characterStatusController.UpdateController(this);

            currentState.UpdateState(this);
            characterRigidbody.UpdateCollision(characterMovement.SpeedX * MotionSpeed, characterMovement.SpeedY * MotionSpeed, characterMovement.InAir);
            currentState.LateUpdateState(this);

        }




        // Active le combat, autrise d'attaquer, les tick de status / reset les cartes
        public void StartBattle()
        {
            OnBattleStart?.Invoke();
        }

        // Event pour stopper le combat (Genre pour retirer certains status, ex : si le poison on veut qu'il continue entre
        // les combats mais on veut pas qu'il s'active entre les salles)
        // (ou encore si on a des buffs qui dure 3 combats par exemple)
        public void EndBattle()
        {
            OnBattleEnd?.Invoke();
        }




        public GameObject CreateAnimation(GameObject animation)
        {
            return Instantiate(animation, particlePoint.transform.position, Quaternion.identity);
        }

        public AnimationParticle CreateAnimation(AnimationParticle animation)
        {
            return Instantiate(animation, particlePoint.transform.position, Quaternion.identity);
        }



        public void SetCharacterMotionSpeed(float newSpeed, float time = 0)
        {
            motionSpeed = newSpeed;
            animator.speed = MotionSpeed;
            characterAction.SetAttackMotionSpeed(MotionSpeed); 

            if (motionSpeedCoroutine != null)
                StopCoroutine(motionSpeedCoroutine);

            if (time > 0 && this.gameObject.activeInHierarchy == true)
            {
                /*if (motionSpeedCoroutine != null)
                    StopCoroutine(motionSpeedCoroutine);*/
                motionSpeedCoroutine = MotionSpeedCoroutine(time);
                StartCoroutine(motionSpeedCoroutine);
            }
        }


        private IEnumerator MotionSpeedCoroutine(float time)
        {
            while (time > 0)
            {
                time -= Time.deltaTime;
                yield return null;
            }

            motionSpeed = 1;
            animator.speed = MotionSpeed;
            characterAction.SetAttackMotionSpeed(MotionSpeed);
        }




        #endregion

    } 

} // #PROJECTNAME# namespace