using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartSettings : MonoBehaviour
{
    void Start()
    {
        Application.targetFrameRate = -1;

        PlayerPrefs.SetInt(SavesData.TetrisColor + 0, 1);

        PlayerPrefs.SetInt(SavesData.TetrisBackground + 0, 1);
    }
}
