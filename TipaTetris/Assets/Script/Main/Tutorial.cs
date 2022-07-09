using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    public GameObject[] Parts;
    public GameObject AuxiliarParts;
    public Text[] Indicators;
    public Color ActiveIndicatorColor;
    public Image BlockColor;
    private static int _currentIndex;

    private int attackSettings;

    public void Start()
    {
        _currentIndex = 0;
        attackSettings = PlayerPrefs.GetInt(SavesData.AttackSettings, 0);
        BlockColor.color = UsedColor();
    }

    public void ChangeIndex(int index)
    {
        _currentIndex += index;

        if (_currentIndex > Parts.Length - 1)
            _currentIndex = 0;
        else if (_currentIndex < 0)
            _currentIndex = Parts.Length - 1;

        ChangeInformation();
    }

    private void ChangeInformation()
    {
        AuxiliarParts.SetActive(false);
        foreach (var item in Parts)
            item.SetActive(false);

        Parts[_currentIndex].SetActive(true);

        if (_currentIndex == 1 && attackSettings == 1)
        {
            Parts[_currentIndex].SetActive(false);
            AuxiliarParts.SetActive(true);
        }

        foreach (var item in Indicators)
            item.color = Color.white;

        Indicators[_currentIndex].color = ActiveIndicatorColor;
    }

    public void CloseTutorial()
    {
        _currentIndex = 0;

        ChangeInformation();

        gameObject.SetActive(false);
    }

    private Color UsedColor()
    {
        ColorUtility.TryParseHtmlString("#" + PlayerPrefs.GetString(SavesData.CurrentTetrisColor, "FFFFFFFF"), out var tempColor);
        return tempColor;
    }
}
