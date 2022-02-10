using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[RequireComponent(typeof(BoxCollider2D))]
public class CharacterRigidbody : MonoBehaviour
{
    public delegate void ActionCollision(Transform transform);
    public event ActionCollision OnCollision;

    [Title("CharacterController")]
    [SerializeField]
    protected BoxCollider2D characterCollider;
    public BoxCollider2D CharacterCollider
    {
        get { return characterCollider; }
    }

    [Title("Collision")]
    [SerializeField]
    LayerMask collisionLayerMask;
    [SerializeField]
    LayerMask collisionAerialLayerMask;
    [SerializeField]
    protected bool collision = true;

    [HorizontalGroup("RaycastOffset")]
    [SerializeField]
    protected float offsetRaycastX = 0.0001f;
    [HorizontalGroup("RaycastOffset")]
    [SerializeField]
    protected float offsetRaycastY = 0.0001f;

    [HorizontalGroup("RaycastNumber")]
    [SerializeField]
    protected int numberRaycastVertical = 2;
    [HorizontalGroup("RaycastNumber")]
    [SerializeField]
    protected int numberRaycastHorizontal = 2;




    protected float actualSpeedX = 0;
    protected float actualSpeedY = 0;

    bool wallHorizontalCollision = false;
    public bool WallHorizontalCollision
    {
        get { return wallHorizontalCollision; }
    }

    bool wallVerticalCollision = false;
    public bool WallVerticalCollision
    {
        get { return wallVerticalCollision; }
    }

    int layerMask;
    bool inAir = false;
    Vector2 bottomLeft;
    Vector2 upperLeft;
    Vector2 bottomRight;
    Vector2 upperRight;

    Transform collisionInfo;




    private void Start()
    {
        ResetLayerMask();
    }


    public void SetNewLayerMask(LayerMask newLayerMask)
    {
        layerMask = newLayerMask;
    }
    public void ResetLayerMask()
    {
        if(inAir)
            layerMask = collisionAerialLayerMask;
        else
            layerMask = collisionLayerMask;
    }


    // Empêche les autre rigidbody de détecter cet objet, mais n'empêche pas ce rigidbody de détecter les autres
    public void CanObstacle(bool b)
    {
        //characterCollider.enabled = b;

        // à refactor c'est pas ouf ouf
        if(b)
            this.gameObject.layer = 13;
        else
            this.gameObject.layer = 17; // Le layer 17 est utilisé pour que le lock on voit quand même le personnage
    }


    public void UpdateCollision(float speedX, float speedY, bool inAir = false)
    {
        actualSpeedX = speedX;
        actualSpeedY = speedY;
        wallHorizontalCollision = false;
        wallVerticalCollision = false;

        if(this.inAir != inAir)
        {
            this.inAir = inAir;
            ResetLayerMask();
        }


        if (collision == true)
        {
            //layerMask = 1 << 8 | 1 << 13;

            bottomLeft = new Vector2(characterCollider.bounds.min.x, characterCollider.bounds.min.y);
            upperLeft = new Vector2(characterCollider.bounds.min.x, characterCollider.bounds.max.y);
            bottomRight = new Vector2(characterCollider.bounds.max.x, characterCollider.bounds.min.y);
            upperRight = new Vector2(characterCollider.bounds.max.x, characterCollider.bounds.max.y);

            UpdatePositionX();
            transform.position = new Vector3(transform.position.x + (actualSpeedX * Time.deltaTime), transform.position.y, transform.position.y);

            bottomLeft = new Vector2(characterCollider.bounds.min.x, characterCollider.bounds.min.y);
            upperLeft = new Vector2(characterCollider.bounds.min.x, characterCollider.bounds.max.y);
            bottomRight = new Vector2(characterCollider.bounds.max.x, characterCollider.bounds.min.y);
            upperRight = new Vector2(characterCollider.bounds.max.x, characterCollider.bounds.max.y);

            UpdatePositionY();
            transform.position = new Vector3(transform.position.x, transform.position.y + (actualSpeedY * Time.deltaTime), transform.position.y);
        }

        //transform.position = new Vector3(transform.position.x + (actualSpeedX * Time.deltaTime), transform.position.y + (actualSpeedY * Time.deltaTime), 0);
    }

    private void UpdatePositionX()
    {

        RaycastHit2D raycastX;
        Vector2 originRaycast;

        if (actualSpeedX < 0)
        {
            // ======================================================================================================
            originRaycast = bottomLeft - new Vector2(offsetRaycastX, 0);
            for (int i = 0; i < numberRaycastHorizontal; i++)
            {
                raycastX = Physics2D.Raycast(originRaycast, new Vector2(actualSpeedX * Time.deltaTime, 0), Mathf.Abs(actualSpeedX * Time.deltaTime) + offsetRaycastX, layerMask);
                Debug.DrawRay(originRaycast, new Vector2(actualSpeedX * Time.deltaTime, 0), Color.red);
                if (raycastX.collider != null)
                {
                    collisionInfo = raycastX.collider.transform;
                    float distance = raycastX.point.x - bottomLeft.x;
                    distance += offsetRaycastX;
                    actualSpeedX = distance / Time.deltaTime;
                    wallHorizontalCollision = true;
                    OnCollision?.Invoke(collisionInfo);
                    return;
                }
                originRaycast += new Vector2(0, Mathf.Abs(upperLeft.y - bottomLeft.y) / (numberRaycastHorizontal - 1));
            }
            // ======================================================================================================

        }
        else if (actualSpeedX > 0)
        {
            // ======================================================================================================
            originRaycast = bottomRight + new Vector2(offsetRaycastX, 0);
            for (int i = 0; i < numberRaycastHorizontal; i++)
            {
                raycastX = Physics2D.Raycast(originRaycast, new Vector2(actualSpeedX * Time.deltaTime, 0), Mathf.Abs(actualSpeedX * Time.deltaTime) + offsetRaycastX, layerMask);
                Debug.DrawRay(originRaycast, new Vector2(actualSpeedX * Time.deltaTime, 0), Color.red);
                if (raycastX.collider != null)
                {
                    collisionInfo = raycastX.collider.transform;
                    float distance = raycastX.point.x - bottomRight.x;
                    distance -= offsetRaycastX;
                    actualSpeedX = distance / Time.deltaTime;
                    wallHorizontalCollision = true;
                    OnCollision?.Invoke(collisionInfo);
                    return;
                }
                originRaycast += new Vector2(0, Mathf.Abs(upperRight.y - bottomRight.y) / (numberRaycastHorizontal - 1));
            }
            // ======================================================================================================

        }
    }


    private void UpdatePositionY()
    {
        RaycastHit2D raycastY;
        Vector2 originRaycast;

        if (actualSpeedY < 0)
        {
            // ======================================================================================================
            originRaycast = bottomLeft - new Vector2(0, offsetRaycastY);
            for (int i = 0; i < numberRaycastVertical; i++)
            {
                raycastY = Physics2D.Raycast(originRaycast, new Vector2(0, actualSpeedY * Time.deltaTime), Mathf.Abs(actualSpeedY * Time.deltaTime) + offsetRaycastY, layerMask);
                Debug.DrawRay(originRaycast, new Vector2(0, actualSpeedY * Time.deltaTime + offsetRaycastY), Color.red);
                if (raycastY.collider != null)
                {
                    collisionInfo = raycastY.collider.transform;
                    float distance = raycastY.point.y + offsetRaycastY - bottomLeft.y;
                    distance += offsetRaycastY;
                    actualSpeedY = distance / Time.deltaTime;
                    wallVerticalCollision = true;
                    OnCollision?.Invoke(collisionInfo);
                    return;
                }
                originRaycast += new Vector2(Mathf.Abs(bottomRight.x - bottomLeft.x) / (numberRaycastVertical - 1), 0);
            }
            // ======================================================================================================

        }
        else if (actualSpeedY > 0)
        {
            // ======================================================================================================
            originRaycast = upperLeft + new Vector2(0, offsetRaycastY);
            for (int i = 0; i < numberRaycastVertical; i++)
            {
                raycastY = Physics2D.Raycast(originRaycast, new Vector2(0, actualSpeedY * Time.deltaTime), Mathf.Abs(actualSpeedY * Time.deltaTime) + offsetRaycastY, layerMask);
                Debug.DrawRay(originRaycast, new Vector2(0, actualSpeedY * Time.deltaTime), Color.yellow);
                if (raycastY.collider != null)
                {
                    collisionInfo = raycastY.collider.transform;
                    float distance = raycastY.point.y - upperLeft.y;
                    distance -= offsetRaycastY;
                    actualSpeedY = distance / Time.deltaTime;
                    wallVerticalCollision = true;
                    OnCollision?.Invoke(collisionInfo);
                    return;
                }
                originRaycast += new Vector2(Mathf.Abs(upperRight.x - upperLeft.x) / (numberRaycastVertical - 1), 0);
            }
            // ======================================================================================================
        }
    }


}
