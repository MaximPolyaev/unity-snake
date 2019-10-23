using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;
using UnityEngine.Events;


namespace SnakeGame
{
    public class GameManager : MonoBehaviour
    {
        public SnakeController Snake;
        public GameObject convasCrown;
        public GameObject convasStartPlayBtn;
        public GameObject convasRestart;
        public GameObject detectClickRestartBtn;
        public GameObject gameField;

        private void Start()
        {
            detectClickRestartBtn.SetActive(false);
        }
        
        private void Update()
        {
            MouseDownListener(0);
        }

        private void MouseDownListener(int status)
        {
            if (Input.GetMouseButtonDown(status))
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit, 100.0f))
                {
                    if (hit.transform != null)
                    {
                        ValidateMouseDown(hit.transform.gameObject);
                    }
                }
            }
        }

        private void ValidateMouseDown(GameObject go)
        {
            switch (go.name)
            {
                case "DetectClickStartBtn":
                    DetectClickStartBtn(go);
                    break;
                case "DetectClickRestartBtn":
                    DetectClickRestartBtn();
                    break;
            }
        }

        void DetectClickStartBtn(GameObject thisGameObject)
        {
            thisGameObject.SetActive(false);
            convasCrown.SetActive(false);
            convasStartPlayBtn.SetActive(false);

            gameField.GetComponent<Animation>().Play("GameFieldInit");

            Snake.StartMove();
        }

        private void DetectClickRestartBtn()
        {
            SetActiveRestartBtn(false);
            Snake.Restart();
        }

        public void SetActiveRestartBtn(bool status = true)
        {
            if (!status)
            {
                detectClickRestartBtn.SetActive(false);
                Animation viewConvasRestart = convasRestart.GetComponent<Animation>();
                viewConvasRestart["ShowRestart"].time = viewConvasRestart["ShowRestart"].length;
                viewConvasRestart["ShowRestart"].speed = -1.0f;
                viewConvasRestart.Play("ShowRestart");
            }
            detectClickRestartBtn.SetActive(true);
        }
    }
}