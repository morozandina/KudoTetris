using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Characters
{
    [CreateAssetMenu(fileName = "New Character", menuName = "Characters/New Characters")]
    public class Character : ScriptableObject
    {
        [Header("Main Information :")]
        public new string name;
        public int index;
        public Sprite sprite;
        [Space]
        [Header("Movement :")]
        public int moveSpeed;
        public int jumpForce;
    }
}