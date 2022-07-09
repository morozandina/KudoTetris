using System;
using UnityEngine;

namespace Script.Auxiliary
{
    public class CameraShakeCenter : MonoBehaviour
    {
        public static Action Shake;
        private Animator animator;

        private void Awake()
        {
            Shake += ShakeScreen;
            animator = GetComponent<Animator>();
        }

        private void OnDestroy()
        {
            Shake -= ShakeScreen;
        }

        private void ShakeScreen()
        {
            animator.SetTrigger("Shake");
        }
    }
}
