using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Tetris;


namespace Control
{
    public class TetrisDownButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        public void OnPointerDown(PointerEventData eventData)
        {
            TetrisMoveDown.ChangeCurrentBlockSpeed?.Invoke(true);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            TetrisMoveDown.ChangeCurrentBlockSpeed?.Invoke(false);
        }
    }
}