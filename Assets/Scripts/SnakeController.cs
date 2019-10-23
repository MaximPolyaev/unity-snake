using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngineInternal;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

namespace SnakeGame
{
    public class SnakeController : MonoBehaviour
    {
        public float speed;

        public GameManager gameManager;
        public GameObject snakeHead;
        public GameObject bodyBlockItem;
        public GameObject convasRestart;
        public float bodyBlockItemDiameter;

        private const float MAX_X = 9.64f;
        private const float MIN_X = -9.6f;
        private const float MAX_Y = 9.4f;
        private const float MIN_Y = -9.36f;

        private Vector2 direction;
        private Vector2 defaultPosition;
        
        private Quaternion defaultRotation;

        private bool up, down, right, left, key_k, key_l;
        private bool startGame;

        private Animation startAnimation;

        private List<GameObject> tail = new List<GameObject>();
        private List<StatusTail> statusesTail = new List<StatusTail>();

        private BoxCollider colliderSnakeHead;
        
        void Start()
        {
            startAnimation = snakeHead.GetComponent<Animation>();
            defaultPosition = transform.position;
            snakeHead.transform.Rotate(Vector3.back, -90f);
            colliderSnakeHead = snakeHead.GetComponent<BoxCollider>();
            startGame = false;
            direction = Vector2.right;
            
            StatusTail statusTail = new StatusTail();
            statusTail.pos = snakeHead.transform.position;
            statusTail.rotation = snakeHead.transform.rotation;
            statusesTail.Add(statusTail);
        }

        void Update()
        {
            if (startGame)
            {
                InputRender();
                transform.Translate(direction * speed * Time.deltaTime);
                TailRender();
                GameOverListener();
            }
        }

        public void AddBodyItem()
        {
            GameObject bodyItem = GameObject.Instantiate(bodyBlockItem, statusesTail[statusesTail.Count - 1].pos,
                Quaternion.identity, transform);

            StatusTail statusTail = new StatusTail();
            statusTail.pos = bodyItem.transform.position;
            statusTail.rotation = statusesTail[statusesTail.Count - 1].rotation;
            statusesTail.Add(statusTail);
            tail.Add(bodyItem);
        }

        void TailRender()
        {
            float distance = ((Vector2) snakeHead.transform.position - statusesTail[0].pos).magnitude;
            if (distance > bodyBlockItemDiameter)
            {
                Vector2 optimizeDirection = ((Vector2) snakeHead.transform.position - statusesTail[0].pos).normalized;
                
                StatusTail statusTail = new StatusTail();
                statusTail.pos = statusesTail[0].pos + optimizeDirection * bodyBlockItemDiameter;
                statusTail.rotation = statusesTail[0].rotation;
                statusesTail.Insert(0, statusTail);
                statusesTail.RemoveAt(statusesTail.Count - 1);
                distance -= bodyBlockItemDiameter;
            }

            for (int i = 0; i < tail.Count; i++)
            {
                tail[i].transform.position =
                    Vector2.Lerp(statusesTail[i + 1].pos, statusesTail[i].pos, distance / bodyBlockItemDiameter);
                tail[i].transform.rotation = statusesTail[i + 1].rotation;
            }
        }

        void InputRender()
        {
            up = Input.GetButtonDown("Up");
            down = Input.GetButtonDown("Down");
            right = Input.GetButtonDown("Right");
            left = Input.GetButtonDown("Left");
            key_k = Input.GetButtonDown("K");
            key_l = Input.GetButtonDown("L");
            if (up)
            {
                GoUp();
            }
            else if (down)
            {
                GoDown();
            }
            else if (left)
            {
                GoLeft();
            }
            else if (right)
            {
                GoRight();
            }
            else if (key_k)
            {
                AddBodyItem();
            } 
            else if (key_l)
            {
                Restart();
            }
        }

        void GoUp()
        {
            if (direction == Vector2.up || direction == Vector2.down) return;

            if (direction == Vector2.right)
            {
                snakeHead.transform.Rotate(Vector3.back, -90);
                statusesTail[0].rotation = snakeHead.transform.rotation;
            }

            if (direction == Vector2.left)
            {
                snakeHead.transform.Rotate(Vector3.back, 90);
                statusesTail[0].rotation = snakeHead.transform.rotation;
            }

            direction = Vector2.up;
        }

        void GoDown()
        {
            if (direction == Vector2.up || direction == Vector2.down) return;

            if (direction == Vector2.right)
            {
                snakeHead.transform.Rotate(Vector3.back, 90);
                statusesTail[0].rotation = snakeHead.transform.rotation;
            }
            if (direction == Vector2.left)
            {
                snakeHead.transform.Rotate(Vector3.back, -90);
                statusesTail[0].rotation = snakeHead.transform.rotation;
            }

            direction = Vector2.down;
        }

        void GoLeft()
        {
            if (direction == Vector2.left || direction == Vector2.right) return;

            if (direction == Vector2.up)
            {
                snakeHead.transform.Rotate(Vector3.back, -90);
                statusesTail[0].rotation = snakeHead.transform.rotation;
            }
            if (direction == Vector2.down)
            {
                snakeHead.transform.Rotate(Vector3.back, 90);
                statusesTail[0].rotation = snakeHead.transform.rotation;
            }

            direction = Vector2.left;
        }

        void GoRight()
        {
            if (direction == Vector2.left || direction == Vector2.right) return;

            if (direction == Vector2.up)
            {
                snakeHead.transform.Rotate(Vector3.back, 90);
                statusesTail[0].rotation = snakeHead.transform.rotation;
            }
            if (direction == Vector2.down)
            {
                snakeHead.transform.Rotate(Vector3.back, -90);
                statusesTail[0].rotation = snakeHead.transform.rotation;
            }

            direction = Vector2.right;
        }

        private void StartAnimation()
        {
            startAnimation.Play("StartMove");
        }

        public void Restart()
        {
            foreach (GameObject bodyItem in tail)
            {
                Destroy(bodyItem);
            }
            tail.Clear();
            statusesTail.Clear();
            
            StartMove();
            StatusTail statusTail = new StatusTail();
            statusTail.pos = snakeHead.transform.position;
            statusTail.rotation = snakeHead.transform.rotation;
            statusesTail.Add(statusTail);
        }
        
        public void StartMove()
        {
            startGame = true;
            transform.position = defaultPosition;
            snakeHead.transform.rotation = new Quaternion();
            snakeHead.transform.Rotate(Vector3.back, -90f);
            
            direction = Vector2.right;
            StartAnimation();
        }

        private void GameOverListener()
        {
            float curPosX = transform.position.x * (1 / transform.parent.localScale.x);
            float curPosY = transform.position.y * (1 / transform.parent.localScale.y);
            if (curPosX > MAX_X || curPosX < MIN_X ||
                curPosY > MAX_Y || curPosY < MIN_Y)
            {
                GameOver();
            }
        }

        public void GameOver()
        {
            Animation viewConvasRestart = convasRestart.GetComponent<Animation>();
            viewConvasRestart["ShowRestart"].time = 0f;
            viewConvasRestart["ShowRestart"].speed = 1.0f;
            viewConvasRestart.Play("ShowRestart");
            startGame = false;
            gameManager.SetActiveRestartBtn();
        }

        private void OnCollisionEnter(Collision other)
        {
            switch (other.transform.gameObject.name)
            {
                case "SnakeBodyItem(Clone)":
                    CollisionBodyItem();
                    break;
            }
        }

        private void CollisionBodyItem()
        {
            GameOver();
        }
    }
}