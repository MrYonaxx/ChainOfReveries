using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;

namespace VoiceActing
{
    public class EntityMovement : MonoBehaviour
    {

        [Title("Stats")]
        [SerializeField]
        private float speedMax = 1;
        public float SpeedMax
        {
            get { return speedMax; }
        }

        /*[SerializeField]
        protected float speedAcceleration = 1;
        public float SpeedAcceleration
        {
            get { return speedAcceleration; }
        }

        [SerializeField]
        protected float speedDecceleration = 1;
        public float SpeedDecceleration
        {
            get { return speedDecceleration; }
        }*/


        [SerializeField]
        private float gravity = 1;
        public float Gravity
        {
            get { return gravity; }
        }
        [SerializeField]
        private float gravityMax = 1;
        public float GravityMax
        {
            get { return gravityMax; }
        }


        //protected CharacterBase character;

        protected int direction = 1;
        public int Direction
        {
            get { return direction; }
        }

        protected bool inAir = false;
        public bool InAir
        {
            get { return inAir; }
        }

        // Pour distinguer quand le perso se déplace ou quand le perso glisse
        protected bool inMovement = false;
        public bool InMovement
        {
            get { return inMovement; }
            set { inMovement = value; }
        }



        [SerializeField]
        [ReadOnly]
        protected float speedX = 0;
        public float SpeedX
        {
            get { return speedX; }
        }

        [SerializeField]
        [ReadOnly]
        protected float speedY = 0;
        public float SpeedY
        {
            get { return speedY; }
        }

        // vitesse dans les airs
        protected float speedZ = 0;
        public float SpeedZ
        {
            get { return speedZ; }
        }


        /*public void ApplyGravity(float gravity, float gravityMax)
        {
            if (inAir == true)
            {
                speedZ -= ((gravity * Time.deltaTime) * character.MotionSpeed);
                speedZ = Mathf.Max(speedZ, gravityMax);
                spriteRenderer.transform.localPosition += new Vector3(0, (speedZ * Time.deltaTime) * character.MotionSpeed, 0);
                if (spriteRenderer.transform.localPosition.y <= 0 && character.MotionSpeed != 0)
                {
                    inAir = false;
                    speedZ = 0;
                    //spriteRenderer.transform.localPosition = new Vector3(spriteRenderer.transform.localPosition.x, 0, spriteRenderer.transform.localPosition.z);
                    //OnGroundCollision();
                }
            }
        }
        */

        public void Jump(float impulsion)
        {
            speedZ = impulsion;
            if(inAir == false && impulsion > 0)
                inAir = true;
        }



        /// <summary>
        /// Set a speed multiplied by characterSpeed
        /// </summary>
        /// <param name="newSpeedX"></param>
        /// <param name="newSpeedY"></param>
        public void Move(float newSpeedX, float newSpeedY)
        {
            speedX = newSpeedX * speedMax;
            speedY = newSpeedY * speedMax;
        }

        public void MoveForward(float multiplier)
        {
            SetSpeed(speedMax * multiplier * direction, 0);
        }

        public void MoveForwardFixed(float speed)
        {
            SetSpeed(speed * direction, 0);
        }

        /// <summary>
        /// Set a arbitrary speed
        /// </summary>
        /// <param name="newSpeedX"></param>
        /// <param name="newSpeedY"></param>
        public void SetSpeed(float newSpeedX, float newSpeedY)
        {
            speedX = newSpeedX;
            speedY = newSpeedY;
        }

        public void SetDirection(int newDirection)
        {
            direction = newDirection;
        }

        public void TurnBack()
        {
            SetDirection(-direction);
        }

        public void MoveToPointInstant(Vector3 point)
        {
            Vector2 direction = point - this.transform.position;
            SetSpeed(direction.x / Time.deltaTime, direction.y / Time.deltaTime);
        }


        public bool MoveToPoint(Vector3 point, float speed)
        {
            Vector2 direction = point - this.transform.position;
            if (Mathf.Abs(direction.magnitude) < 0.1f)
            {
                SetSpeed(0, 0);
                return true;
            }
            else
            {
                direction.Normalize();
                SetSpeed(direction.x * speed, direction.y * speed);
                return false;
            }
        }


        public void SetMotionSpeed(float newSpeed, float time = 0)
        {
        }



    }
}
