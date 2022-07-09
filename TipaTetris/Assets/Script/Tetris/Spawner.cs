using System;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Tetris
{
    public class Spawner : MonoBehaviour
    {
        // Action
        public static Action StartNewFig;
        // Spawn System
        public GameObject[] TetrisFigure;

        // Count Down
        [SerializeField] private Text countDownText;
        [SerializeField] private GameObject countDownObject;
        private float previousTime;
        private int countDown = 4;
        private bool isFinishedCountDown = false;

        private void Awake()
        {
            StartNewFig += NewFigure;
        }

        private void OnDestroy()
        {
            StartNewFig -= NewFigure;
        }

        public void NewFigure()
        {
            var newFig = Instantiate(TetrisFigure[Random.Range(0, TetrisFigure.Length)]);
            newFig.transform.position = new Vector3((int)Random.Range(0, Tetroid.width), transform.position.y, 0);

            foreach (Transform children in newFig.transform)
            {
                int roundedX = Mathf.RoundToInt(children.position.x);

                if (roundedX < 0)
                    newFig.transform.position += new Vector3(1, 0, 0);
                else if (roundedX >= Tetroid.width)
                    newFig.transform.position -= new Vector3(1, 0, 0);
            }
        }

        private void Update()
        {
            if (isFinishedCountDown)
                return;

            if (Time.time - previousTime > 1f)
            {
                if (countDown > 0)
                {
                    countDown -= 1;
                    countDownText.text = countDown.ToString();
                }
                else
                {
                    isFinishedCountDown = true;
                    Destroy(countDownObject);
                    NewFigure();
                }

                previousTime = Time.time;
            }
        }
    }
}