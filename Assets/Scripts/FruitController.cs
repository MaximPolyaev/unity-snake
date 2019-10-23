using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace SnakeGame
{
    public class FruitController : MonoBehaviour
    {
        public GameObject fruitApple;
        public GameObject fruitStrawberry;
        public GameObject fruitMelon;
        public GameObject bomb;

        private float timer = 0;
        private float seconds = 0;
        
        private const float MAX_X = 9.64f;
        private const float MIN_X = -9.6f;
        private const float MAX_Y = 9.4f;
        private const float MIN_Y = -9.36f;
        
        private void Update()
        {
            timer += Time.deltaTime;
            if (timer > 1)
            {
                timer = 0;
                seconds += 1;
            }

            if (seconds > 2)
            {
                seconds = 0;
                AddFruit();
            }
        }

        private void AddFruit()
        {
            GameObject fruit = new GameObject();
            Destroy(fruit);
            Vector3 worldPosition = Vector3.zero;
            worldPosition.x = Random.Range(MIN_X * transform.localScale.x, MAX_X * transform.localScale.x);
            worldPosition.y = Random.Range(MIN_Y * transform.localScale.y, MAX_Y * transform.localScale.y);
            int randomFruit = Random.Range(0, 4);
            switch (randomFruit)
            {
                case 0:
                    fruit = GameObject.Instantiate(fruitApple, worldPosition,
                        Quaternion.identity, transform);
                    break;
                case 1:
                    fruit = GameObject.Instantiate(fruitStrawberry, worldPosition,
                        Quaternion.identity, transform);
                    break;
                case 2:
                    fruit = GameObject.Instantiate(fruitMelon, worldPosition,
                        Quaternion.identity, transform);
                    break;
                case 3:
                    fruit = GameObject.Instantiate(bomb, worldPosition,
                        Quaternion.identity, transform);
                    break;
            }
        }
    }
}