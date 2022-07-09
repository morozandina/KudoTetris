using UnityEngine;
using UnityEngine.UI;

namespace Script
{
    public class OtherSettings : MonoBehaviour
    {
        [SerializeField] private Slider attackTypeSlider;
        [SerializeField] private Slider musicSlider;

        private void Start()
        {
            attackTypeSlider.value = PlayerPrefs.GetInt(SavesData.AttackSettings, 0);
            musicSlider.value = PlayerPrefs.GetInt(SavesData.Music, 0);
        }

        public void OnChangeMusic(Slider slider)
        {
            PlayerPrefs.SetInt(SavesData.Music, (int) slider.value);
        }
        
        public void OnChangeAttackType(Slider slider)
        {
            PlayerPrefs.SetInt(SavesData.AttackSettings, (int) slider.value);
        }
    }
}
