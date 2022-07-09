using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Buy
{
    public class BuyItems : MonoBehaviour
    {
        public List<Color> Colors = new List<Color>();
        public List<Sprite> LocationSprite = new List<Sprite>();
        public List<GameObject> Items = new List<GameObject>();
        public List<GameObject> LastItems = new List<GameObject>();
        public int ColorPrice;
        public int LocationPrice;
        public int ColorADCount;
        public int LocationADCount;
    }
}