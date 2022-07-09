using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Tetris
{
    public class TetrisImageBlock : MonoBehaviour
    {
        public static Action ChangeForAll;
        [Header("Block Sprites :")]
        [SerializeField] private Sprite allSprite;
        [SerializeField] private Sprite downSprite;
        [SerializeField] private Sprite upSprite;
        [SerializeField] private Sprite leftSprite;
        [SerializeField] private Sprite rightSprite;
        [SerializeField] private Sprite rightUpPlusSprite;
        [SerializeField] private Sprite rightDownPlusSprite;
        [SerializeField] private Sprite leftUpPlusSprite;
        [SerializeField] private Sprite leftDownPlusSprite;

        private void Awake()
        {
            ChangeForAll += ChangeBlockImage;
        }

        private void OnDestroy()
        {
            ChangeForAll -= ChangeBlockImage;
        }

        public void ChangeBlockImage()
        {
            StartCoroutine(WaitForIt());
        }

        private void VerifyBlocks()
        {
            switch (transform.childCount)
            {
                case 0:
                    Destroy(gameObject);
                    break;
                case 1:
                    SetDefaultBlock();
                    break;
                case 2:
                    ChangeDualBlock();
                    break;
                case 3:
                    ChangeThirdBlock();
                    break;
            }
        }

        private void SetDefaultBlock()
        {
            transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = allSprite;
        }

        private void ChangeDualBlock()
        {
            if (childPosition(0).x == childPosition(1).x)
                if (childPosition(0).y != childPosition(1).y)
                {
                    transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = downSprite;
                    transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = upSprite;
                }
            if (childPosition(0).y == childPosition(1).y)
                if (childPosition(0).x != childPosition(1).x)
                {
                    transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = leftSprite;
                    transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = rightSprite;
                }
            if (childPosition(0).y != childPosition(1).y && childPosition(0).x != childPosition(1).x)
            {
                transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = allSprite;
                transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = allSprite;
            }
        }

        private void ChangeThirdBlock()
        {
            if (childPosition(1).y == 1 && childPosition(2).y == 1)
            {
                transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = downSprite;
                if (childPosition(0).x == childPosition(2).x)
                {
                    transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = leftSprite;
                    transform.GetChild(2).GetComponent<SpriteRenderer>().sprite = rightUpPlusSprite;
                }
                else
                {
                    transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = leftUpPlusSprite;
                    transform.GetChild(2).GetComponent<SpriteRenderer>().sprite = rightSprite;
                }
            }
            else
            {
                transform.GetChild(2).GetComponent<SpriteRenderer>().sprite = upSprite;
                if (childPosition(2).x == childPosition(0).x)
                {
                    transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = leftDownPlusSprite;
                    transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = rightSprite;
                }
                else
                {
                    transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = leftSprite;
                    transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = rightDownPlusSprite;
                }
            }
        }

        private IEnumerator WaitForIt()
        {
            yield return new WaitForSeconds(.01f);

            VerifyBlocks();
        }

        private Vector2 childPosition(int index)
        {
            return transform.GetChild(index).localPosition;
        }
    }
}