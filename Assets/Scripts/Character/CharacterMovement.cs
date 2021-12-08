using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;

namespace VoiceActing
{
    public class CharacterMovement : MonoBehaviour
    {

        CharacterStat characterStat;


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

        [SerializeField]
        [ReadOnly]
        // vitesse dans les airs
        protected float speedZ = 0;
        public float SpeedZ
        {
            get { return speedZ; }
        }

        protected float posZ = 0;
        public float PosZ
        {
            get { return posZ; }
        }


        public void InitializeComponent(CharacterStat stats)
        {
            characterStat = stats;
        }



        public void ApplyGravity(float multiplier)
        {
            if (inAir == true)
            {
                posZ += (speedZ * Time.deltaTime) * multiplier;
                speedZ -= (characterStat.Gravity.Value * Time.deltaTime) * multiplier;
                speedZ = Mathf.Max(speedZ, -characterStat.Gravity.Value);
                if (posZ <= 0 && multiplier > 0)
                {
                    inAir = false;
                    speedZ = 0;
                    posZ = 0;
                }
            }
        }

        public void Jump(float impulsion)
        {
            speedZ += impulsion * characterStat.Mass.Value;
            if(inAir == false && impulsion > 0 && speedZ > 0)
                inAir = true;
        }
        public void SetSpeedZ(float impulsion)
        {
            speedZ = impulsion;
            if (inAir == false && speedZ > 0)
                inAir = true;
        }



        public void Move(float newSpeedX, float newSpeedY)
        {
            speedX = newSpeedX;
            speedY = newSpeedY;
        }

        /// <summary>
        /// Set a speed multiplied by characterSpeed
        /// </summary>
        /// <param name="newSpeedX"></param>
        /// <param name="newSpeedY"></param>
        public void MoveDirection(float directionX, float directionY)
        {
            speedX = directionX * characterStat.Speed.Value;
            speedY = directionY * characterStat.Speed.Value;
        }

        public void MoveForward(float multiplier)
        {
            SetSpeed(characterStat.Speed.Value * multiplier * direction, 0);
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




    }
}
