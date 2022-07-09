using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using Characters;

namespace Main
{
    public class MainSceneManager : MonoBehaviour
    {
        public static Action Refresh;
        public Text moneyText;
        public SpriteRenderer PlayerSprite;

        private void Awake()
        {
            Refresh += SetMoneyText;
        }

        private void OnDestroy()
        {
            Refresh -= SetMoneyText;
        }

        // Character operate controller

        private void Start()
        {
            SetMoneyText();
        }

        private void SetMoneyText()
        {
            moneyText.text = PlayerPrefs.GetInt(SavesData.Money, 0).ToString();

        }
    }
}