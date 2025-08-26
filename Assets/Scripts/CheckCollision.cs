//using UnityEngine;
//using UnityEngine.Events;

//[System.Serializable]
//public class ColliderTargetTagName
//{
//    public string tagName;
//    public UnityEvent onCollisionEnter;
//    public UnityEvent onCollisionExit;
//}

//public class CheckCollision : MonoBehaviour
//{
//    public ColliderTargetTagName[] targets;
//    public enum DIRECTION
//    {
//        NONE,
//        UP,
//        DOWN,
//        LEFT,
//        RIGHT
//    }
//    public DIRECTION direction;

//    private BoarMovement _boarMovement;

//    void Start()
//    {
//        _boarMovement = GetComponent<BoarMovement>();
//    }

//    public void OnCollisionEnter2D(Collision2D collision)
//    {
//        if (_boarMovement == null) return;

//        foreach (ColliderTargetTagName target in targets)
//        {
//            if (collision.gameObject.CompareTag(target.tagName))
//            {
//                switch (direction)
//                {
//                    case DIRECTION.UP:
//                        _boarMovement.HandleTopCollision();
//                        break;
//                    case DIRECTION.DOWN:
//                        _boarMovement.HandleBottomCollision();
//                        break;
//                    case DIRECTION.LEFT:
//                        _boarMovement.HandleLeftCollision();
//                        break;
//                    case DIRECTION.RIGHT:
//                        _boarMovement.HandleRightCollision();
//                        break;
//                }

//                Debug.Log("Collided with tag: " + target.tagName);
//                target.onCollisionEnter.Invoke();
//                return;
//            }
//        }
//    }

//    public void OnCollisionExit2D(Collision2D collision)
//    {
//        foreach (ColliderTargetTagName target in targets)
//        {
//            if (collision.gameObject.CompareTag(target.tagName))
//            {
//                target.onCollisionExit.Invoke();
//                return;
//            }
//        }
//    }
//}
