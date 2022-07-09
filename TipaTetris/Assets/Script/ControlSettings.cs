using System;
using UnityEngine;
using UnityEngine.UI;

namespace Script
{
    public class ControlSettings : MonoBehaviour
    {
        private Slider slider;
        private QualityType qualityType;

        private void Start()
        {
            slider = GetComponent<Slider>();
        
            Enum.TryParse(PlayerPrefs.GetString(SaveKeys.Graphics, QualityType.High.ToString()), out qualityType);

            switch(qualityType)
            {
                case QualityType.Low:
                    slider.value = 0;
                    break;
                case QualityType.High:
                    slider.value = 1;
                    break;
            }
        }

        public void ChangeSettings()
        {
            GameSettingsManager.ResolutionAndQuality((QualityType) slider.value);
            PlayerPrefs.SetInt(SaveKeys.PostProcessing, (int) slider.value);
            VolumeSettings.RefreshVolume?.Invoke((int)slider.value);
        }
    }
}
