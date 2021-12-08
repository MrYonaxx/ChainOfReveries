using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace VoiceActing
{
    public class InfiniteCorridor : MonoBehaviour
    {
        [Title("Parameter")]
        [SerializeField]
        float arenaInterval = 10;

        [Title("Camera Focus")]
        [SerializeField]
        Transform focusCorridor = null;
        [SerializeField]
        BoxCollider focusLevel = null;

        [Title("Collision")]
        [SerializeField]
        GameObject environmentCollision = null;
        [SerializeField]
        GameObject environmentCollisionLeft = null;
        [SerializeField]
        GameObject environmentCollisionRight = null;

        [Title("Visual")]
        [SerializeField]
        GameObject objectToCenter; // Objet a toujours centré sur l'arène


        private Vector3 currentArea;
        private Vector3 nextArea;
        public Vector3 NextArea
        {
            get { return nextArea; }
        }


        float collisionBounds = 10;
        float leftCollisionX = 0;

        float boundsFocusLeft = 0;
        float boundsFocusRight = 0;

        float previousCameraX = 0f;

        bool transition = false;


        // Start is called before the first frame update
        void Start()
        {
            collisionBounds = focusLevel.bounds.extents.x;
            leftCollisionX = environmentCollisionLeft.transform.localPosition.x;
        }

        // Update is called once per frame
        void Update()
        {
            if (transition)
            {
                if (focusCorridor.transform.position.x + 1 > nextArea.x) // Le +1 c'est pour que ça marche mieux
                {
                    CloseArea();
                }
                else if (focusCorridor.transform.position.x > previousCameraX)
                {
                    previousCameraX = focusCorridor.transform.position.x;
                    ResizeCameraBounds();
                    // On replace la collision de gauche pour que le joueur ne sorte pas du champs de la camera
                    environmentCollisionLeft.transform.position = new Vector3(boundsFocusLeft, environmentCollisionLeft.transform.position.y, environmentCollisionLeft.transform.position.z);
                }
            }
        }


        public Vector3 OpenArea(float multiplier)
        {
            transition = true;

            currentArea = nextArea;

            nextArea = currentArea;
            nextArea.x += (2 * arenaInterval) * multiplier;

            previousCameraX = focusCorridor.transform.position.x;
            ResizeCameraBounds();

            // On ouvre à droite;
            environmentCollisionRight.gameObject.SetActive(false);

            objectToCenter.transform.position = nextArea;

            return nextArea;
        }

        public void CloseArea()
        {
            transition = false;

            currentArea = nextArea;

            focusLevel.transform.position = new Vector3(nextArea.x, focusLevel.transform.position.y, focusLevel.transform.position.z);
            focusLevel.size = new Vector3(collisionBounds * 2, focusLevel.size.y, focusLevel.size.z);

            // On ferme à droite et on replace les collisions 
            environmentCollisionRight.gameObject.SetActive(true);
            environmentCollision.transform.position = currentArea;
            environmentCollisionLeft.transform.localPosition = new Vector3(leftCollisionX, environmentCollisionLeft.transform.localPosition.y, environmentCollisionLeft.transform.localPosition.z);
        }

        private void ResizeCameraBounds()
        {
            boundsFocusLeft = focusCorridor.position.x - collisionBounds;
            boundsFocusRight = nextArea.x + collisionBounds;
            focusLevel.transform.position = new Vector3(boundsFocusLeft + (boundsFocusRight - boundsFocusLeft) * 0.5f, focusLevel.transform.position.y, focusLevel.transform.position.z);
            focusLevel.size = new Vector3(boundsFocusRight - boundsFocusLeft, focusLevel.size.y, focusLevel.size.z);
        }
    }
}
