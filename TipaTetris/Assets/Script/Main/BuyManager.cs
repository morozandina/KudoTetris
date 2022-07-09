using System.Collections;
using System.Collections.Generic;
using Buy;
using Main;
using UnityEngine;
using UnityEngine.UI;

public class BuyManager : MonoBehaviour
{
    [Header("Button location to spawn:")]
    public Transform ColorParent;
    public Transform LocationParent;
    [Header("Confirmation buttons and panels:")]
    public GameObject ConfirmationPanel;
    public GameObject ConfirmationPanelButtons;
    public GameObject AlertPanel;
    public Button Buy;
    public Button Ad;
    [Header("Buy Database:")]
    public BuyItems BuyItems;

    private RectTransform _colorParentRect;
    private RectTransform _locationParentRect;

    private void Start()
    {
        _colorParentRect = ColorParent.GetComponent<RectTransform>();
        _locationParentRect = LocationParent.GetComponent<RectTransform>();

        SetButtonsStatus();
    }

    private void SetButtonsStatus()
    {
        // Colors partition
        foreach (var color in BuyItems.Colors)
        {
            _colorParentRect.sizeDelta += new Vector2(100, 0);
            var tempGO = Instantiate(BuyItems.Items[0], ColorParent);

            tempGO.transform.GetChild(0).GetComponent<Image>().color = color;
        }
        _colorParentRect.sizeDelta += new Vector2(100, 0);
        Instantiate(BuyItems.LastItems[0], ColorParent);
        SetChildColorParam();

        // Location partition
        foreach (var sprite in BuyItems.LocationSprite)
        {
            _locationParentRect.sizeDelta += new Vector2(250, 0);
            var tempGO = Instantiate(BuyItems.Items[1], LocationParent);

            tempGO.transform.GetChild(0).GetComponent<Image>().sprite = sprite;
        }
        Instantiate(BuyItems.LastItems[1], LocationParent);
        SetChildLocationParam();
    }

    private void SetChildColorParam()
    {
        for (var i = 0; i < ColorParent.childCount - 1; i++)
        {
            ColorParent.GetChild(i).GetComponent<Button>().onClick.RemoveAllListeners();
            ColorParent.GetChild(i).GetChild(1).gameObject.SetActive(false);
            ColorParent.GetChild(i).GetChild(2).gameObject.SetActive(false);
            ColorParent.GetChild(i).GetChild(3).gameObject.SetActive(false);

            if (PlayerPrefs.GetInt(SavesData.TetrisColor + i, 0) == 0) // It is not bought
            {
                var _priceParent = ColorParent.GetChild(i).GetChild(1);
                _priceParent.gameObject.SetActive(true);
                _priceParent.GetChild(0).GetComponent<Text>().text = BuyItems.ColorPrice + " $";
                _priceParent.GetChild(1).GetComponent<Text>().text = $"{PlayerPrefs.GetInt(SavesData.TetrisColorAd + i, 0)} OF {BuyItems.ColorADCount} ADS";

                var index = i;
                ColorParent.GetChild(i).GetComponent<Button>().onClick.AddListener(() => { BuyButton(index, 0); });
            }
            if (PlayerPrefs.GetInt(SavesData.TetrisColor + i, 0) == 1) // It is bought
                if (PlayerPrefs.GetString(SavesData.CurrentTetrisColor, "FFFFFFFF") == ColorUtility.ToHtmlStringRGBA(BuyItems.Colors[i])) // This object is used
                    ColorParent.GetChild(i).GetChild(3).gameObject.SetActive(true);
                else
                {
                    ColorParent.GetChild(i).GetChild(2).gameObject.SetActive(true);
                    ColorParent.GetChild(i).GetComponent<Button>().onClick.RemoveAllListeners();
                    var index = i;
                    ColorParent.GetChild(i).GetComponent<Button>().onClick.AddListener(() => { UseButton(index, 0); });
                }
        }
    }

    private void SetChildLocationParam()
    {
        for (var i = 0; i < LocationParent.childCount - 1; i++)
        {
            LocationParent.GetChild(i).GetComponent<Button>().onClick.RemoveAllListeners();
            LocationParent.GetChild(i).GetChild(1).gameObject.SetActive(false);
            LocationParent.GetChild(i).GetChild(2).gameObject.SetActive(false);
            LocationParent.GetChild(i).GetChild(3).gameObject.SetActive(false);

            if (PlayerPrefs.GetInt(SavesData.TetrisBackground + i, 0) == 0) // It is not bought
            {
                var _priceParent = LocationParent.GetChild(i).GetChild(1);
                _priceParent.gameObject.SetActive(true);
                _priceParent.GetChild(0).GetComponent<Text>().text = BuyItems.LocationPrice + " $";
                _priceParent.GetChild(1).GetComponent<Text>().text = $"{PlayerPrefs.GetInt(SavesData.TetrisBackgroundAd + i, 0)} OF {BuyItems.LocationADCount} ADS";

                var index = i;
                LocationParent.GetChild(i).GetComponent<Button>().onClick.AddListener(() => { BuyButton(index, 1); });
            }
            if (PlayerPrefs.GetInt(SavesData.TetrisBackground + i, 0) == 1) // It is bought
                if (PlayerPrefs.GetInt(SavesData.CurrentTetrisBackground, 0) == i) // This object is used
                    LocationParent.GetChild(i).GetChild(3).gameObject.SetActive(true);
                else
                {
                    LocationParent.GetChild(i).GetChild(2).gameObject.SetActive(true);
                    LocationParent.GetChild(i).GetComponent<Button>().onClick.RemoveAllListeners();
                    var index = i;
                    LocationParent.GetChild(i).GetComponent<Button>().onClick.AddListener(() => { UseButton(index, 1); });
                }
        }
    }

    // ButtonClick

    private void UseButton(int index, int type) // 0 - color & 1 - location
    {
        switch (type)
        {
            case 0:
                ColorParent.GetChild(index).GetChild(2).gameObject.SetActive(false);
                ColorParent.GetChild(index).GetChild(3).gameObject.SetActive(true);

                PlayerPrefs.SetString(SavesData.CurrentTetrisColor, ColorUtility.ToHtmlStringRGBA(BuyItems.Colors[index]));
                SetChildColorParam();
                break;
            case 1:
                LocationParent.GetChild(index).GetChild(2).gameObject.SetActive(false);
                LocationParent.GetChild(index).GetChild(3).gameObject.SetActive(true);

                PlayerPrefs.SetInt(SavesData.CurrentTetrisBackground, index);
                SetChildLocationParam();
                break;
        }
    }

    private void BuyButton(int index, int type)
    {
        ConfirmationPanel.SetActive(true);
        ConfirmationPanelButtons.SetActive(true);
        AlertPanel.SetActive(false);

        Buy.onClick.RemoveAllListeners();

        switch (type)
        {
            case 0:
                Buy.onClick.AddListener(() =>
                {
                    if (PlayerPrefs.GetInt(SavesData.Money, 0) >= BuyItems.ColorPrice)
                    {
                        PlayerPrefs.SetInt(SavesData.TetrisColor + index, 1);
                        PlayerPrefs.SetInt(SavesData.Money, PlayerPrefs.GetInt(SavesData.Money, 0) - BuyItems.ColorPrice);
                        ConfirmationPanel.SetActive(false);
                        SetChildColorParam();

                        MainSceneManager.Refresh?.Invoke();
                    }
                    else
                    {
                        ConfirmationPanelButtons.SetActive(false);
                        AlertPanel.SetActive(true);
                    }
                });
                break;
            case 1:
                Buy.onClick.AddListener(() =>
                {
                    if (PlayerPrefs.GetInt(SavesData.Money, 0) >= BuyItems.LocationPrice)
                    {
                        PlayerPrefs.SetInt(SavesData.TetrisBackground + index, 1);
                        PlayerPrefs.SetInt(SavesData.Money, PlayerPrefs.GetInt(SavesData.Money, 0) - BuyItems.LocationPrice);
                        ConfirmationPanel.SetActive(false);
                        SetChildLocationParam();

                        MainSceneManager.Refresh?.Invoke();
                    }
                    else
                    {
                        ConfirmationPanelButtons.SetActive(false);
                        AlertPanel.SetActive(true);
                    }
                });
                break;
        }
    }

    private void AdButton(int index, int type)
    {

    }
}
