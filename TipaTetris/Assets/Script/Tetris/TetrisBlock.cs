using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Tetris
{
    public class TetrisBlock : MonoBehaviour
    {
        // Fall
        public static float fallTime { get; set; }

        [NonSerialized] public int fallMultiplier = 1;
        // Other
        private float previousTime;
        private List<Vector2> lastPosition = new List<Vector2>();
        private TetrisMoveDown tetrisMoveDown;
        

        // Just for verification
        private bool isSpawnedOther;

        private void Start()
        {
            tetrisMoveDown = GetComponent<TetrisMoveDown>();
            isSpawnedOther = false;
        }

        private void Update()
        {
            MoveObjectDown();
        }

        // Object move system
        private void MoveObjectDown()
        {
            if (!(Time.time - previousTime > fallTime / fallMultiplier))
                return;
            
            transform.position += new Vector3(0, -1, 0);
            
            previousTime = Time.time;

            if (ValidMove())
                return;
            
            transform.position += new Vector3(0, 1, 0);
            AddToGrid();
            CheckForLines();

            if (!isSpawnedOther)
            {
                Spawner.StartNewFig?.Invoke();
                Destroy(tetrisMoveDown);
                isSpawnedOther = true;
            }

            if (!ValidLastBlock())
                GameManager.Lose?.Invoke();
        }

        // Grid System { Adding }
        private void AddToGrid()
        {
            foreach (Transform children in transform)
            {
                var roundedX = Mathf.RoundToInt(children.position.x);
                var roundedY = Mathf.RoundToInt(children.position.y);
        
                lastPosition.Add(new Vector2(roundedX, roundedY));
                Tetroid.grid[roundedX, roundedY] = children;
            }
        }

        // Grid System { Remove cells }
        private void RemoveCell()
        {
            for (var i = 0; i < lastPosition.Count; i++)
                if (Tetroid.grid[(int)lastPosition[i].x, (int)lastPosition[i].y] != null)
                {
                    Tetroid.grid[(int)lastPosition[i].x, (int)lastPosition[i].y] = null;
                    lastPosition.RemoveAt(i);
                }
        }

        // Grid System { Check if line is complete and remove it }
        private void CheckForLines()
        {
            for (var i = Tetroid.height - 1; i >= 0; i--)
                if (HasLine(i))
                {
                    DeleteLine(i);
                }
        }

        // Grid System { Verify line }
        private bool HasLine(int i)
        {
            for (var j = 0; j < Tetroid.width; j++)
                if (Tetroid.grid[j, i] == null)
                    return false;
        
            return true;
        }

        // Grid System { Remove line }
        private void DeleteLine(int i)
        {
            for (var j = 0; j < Tetroid.width; j++)
            {
                Destroy(Tetroid.grid[j, i].gameObject);
                Tetroid.grid[j, i] = null;
            }
            TetrisImageBlock.ChangeForAll?.Invoke();
            GameManager.ChangeScore?.Invoke();
        }

        // If object can move down
        private bool ValidMove()
        {
            foreach (Transform children in transform)
            {
                var position = children.position;
                var roundedX = Mathf.RoundToInt(position.x);
                var roundedY = Mathf.RoundToInt(position.y);
        
                if (roundedY < 0) // Verify canvas size
                    return false; 
                
                if (Tetroid.grid[roundedX, roundedY] != null) // Verify grid size
                    return false;
            }
        
            return true;
        }

        private bool ValidLastBlock()
        {
            foreach (Transform children in transform)
            {
                var roundedY = Mathf.RoundToInt(children.position.y);
        
                if (roundedY >= Tetroid.height - 1)
                    return false;
            }
        
            return true;
        }
    }
}