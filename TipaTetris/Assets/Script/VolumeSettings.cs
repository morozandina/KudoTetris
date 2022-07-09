using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class VolumeSettings : MonoBehaviour
{
    public static Action<int> RefreshVolume;
    private Volume _volume;

    private void Awake()
    {
        RefreshVolume += RefreshVolumeLive;
    }

    private void OnDestroy()
    {
        RefreshVolume -= RefreshVolumeLive;
    }

    void Start()
    {
        _volume = GetComponent<Volume>();
        Scene currentScene = SceneManager.GetActiveScene();
        int buildIndex = currentScene.buildIndex;

        if (PlayerPrefs.GetInt(SaveKeys.PostProcessing, 1) == 0)
        {
            if (buildIndex == 3)
            {
                _volume.weight = 0;
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

    private void RefreshVolumeLive(int level)
    {
        _volume.weight = level;
    }
}
