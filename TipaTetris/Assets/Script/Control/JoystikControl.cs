using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Control
{
    public class JoystikControl : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        private FloatingJoystick floatingJoystick;
        private bool attackSettings;

        private bool pointerDown;
        private float pointerDownTimer;
        
        public float requiredHoldTime;
        public Button attackButton;

        private void Start()
        {
            floatingJoystick = gameObject.GetComponent<FloatingJoystick>();
            attackSettings = PlayerPrefs.GetInt(SavesData.AttackSettings, 0) == 0;

            if (attackSettings) return;
            attackButton.gameObject.SetActive(true);
            attackButton.onClick.AddListener(() => CharacterMovement.Attack?.Invoke());
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (attackSettings)
                pointerDown = true;
        }
        
        public void OnPointerUp(PointerEventData eventData)
        {
            if (attackSettings)
                Reset();
        }

        private void Update()
        {
            if (attackSettings)
            {
                if (!pointerDown) return;
                
                pointerDownTimer += Time.deltaTime;
                if (pointerDownTimer > requiredHoldTime)
                {
                    CharacterMovement.JoystickMove?.Invoke(floatingJoystick.Horizontal, floatingJoystick.Vertical);
                }
            }
            else 
                CharacterMovement.JoystickMove?.Invoke(floatingJoystick.Horizontal, floatingJoystick.Vertical);
        }

        private void Reset()
        {
            if (!attackSettings)
                return;
            
            if (pointerDownTimer <= requiredHoldTime)
                CharacterMovement.Attack?.Invoke();
        
            pointerDown = false;
            pointerDownTimer = 0;
            CharacterMovement.JoystickMove?.Invoke(0, 0);
        }
    }
}