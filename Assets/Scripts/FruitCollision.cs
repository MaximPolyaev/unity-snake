using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SnakeGame
{
    public class FruitCollision : MonoBehaviour
    {
        public SnakeController snakeController;

        public Text textHighscore;
        public Text textScore;
        
        private int highscore = 0;
        private int score = 0;
        private void OnCollisionEnter(Collision other)
        {
            switch (other.transform.gameObject.name)
            {
                case "Bomb(Clone)":
                    GameOver();
                    break;
                case "FruitApple(Clone)":
                    FruitEat(other.transform.gameObject);
                    break;
                case "FruitMelon(Clone)":
                    FruitEat(other.transform.gameObject);
                    break;
                case "FruitStrawberry(Clone)":
                    FruitEat(other.transform.gameObject);
                    break;
            }
        }

        private void FruitEat(GameObject fruit)
        {
            snakeController.AddBodyItem();
            Destroy(fruit);
            score += 1;
            textScore.text = score.ToString();
            if (score > highscore)
            {
                highscore = score;
                textHighscore.text = highscore.ToString();
            }

            if (score > 50)
            {
                GameWin();
            }
        }

        private void GameWin()
        {
            
        }

        private void GameOver()
        {
            score = 0;
            textScore.text = "0";
            snakeController.GameOver();
        }
    }
}

