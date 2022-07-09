using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Control;

namespace Tetris
{
    public class TetrisMoveDown : MonoBehaviour
    {
        public static Action<bool> ChangeCurrentBlockSpeed;
        private TetrisBlock tetrisBlock;

        private void Awake()
        {
            ChangeCurrentBlockSpeed += ChangeCurrentSpeed;
        }

        private void OnDestroy()
        {
            ChangeCurrentBlockSpeed -= ChangeCurrentSpeed;
        }

        private void Start()
        {
            tetrisBlock = GetComponent<TetrisBlock>();
        }

        private void ChangeCurrentSpeed(bool _state)
        {
            tetrisBlock.fallMultiplier = _state ? 10 : 1;
        }
    }
}