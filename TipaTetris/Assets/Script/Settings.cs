using System;
using UnityEngine;

public static class SaveKeys
{
    public static string Resolution = "Resolution";
    public static string Graphics = "Graphics";
    public static string PostProcessing = "PostProcessing";
}
public enum QualityType
{
    Low = 0,
    High = 1
}

public static class GameSettingsManager
{
    private static QualityType GraphicsSettings;
    private static QualityType ResolutionQuality;

    private static int DefaultWidthScreen;
    private static int DefaultHeightScreen;

    [RuntimeInitializeOnLoadMethod]
    private static void InitGraphicsSettings()
    {
        SetResolution();
        SetGraphicsSettings();
    }

    public static void ResolutionAndQuality(QualityType qualityType)
    {
        SaveResolution(qualityType);
        SaveGraphics(qualityType);
    }

    public static void SaveResolution(QualityType qualityType)
    {
        PlayerPrefs.SetString(SaveKeys.Resolution, qualityType.ToString());

        ChangeResolution(qualityType);
        ResolutionQuality = qualityType;
    }

    public static void SaveGraphics(QualityType qualityType)
    {
        PlayerPrefs.SetString(SaveKeys.Graphics, qualityType.ToString());

        switch (qualityType)
        {
            case QualityType.Low:
                QualitySettings.SetQualityLevel(1, true);
                break;
            case QualityType.High:
                QualitySettings.SetQualityLevel(2, true);
                break;
        }

        GraphicsSettings = qualityType;
    }

    private static void SetGraphicsSettings()
    {
        Enum.TryParse(PlayerPrefs.GetString(SaveKeys.Graphics, QualityType.High.ToString()), out GraphicsSettings);

        switch (GraphicsSettings)
        {
            case QualityType.Low:
                QualitySettings.SetQualityLevel(1, true);
                break;
            case QualityType.High:
                QualitySettings.SetQualityLevel(2, true);
                break;
        }
    }

    private static void SetResolution()
    {
        DefaultWidthScreen = Screen.width;
        DefaultHeightScreen = Screen.height;
        Enum.TryParse(PlayerPrefs.GetString(SaveKeys.Resolution, QualityType.High.ToString()),
        out QualityType resolution);
        ChangeResolution(resolution);
        ResolutionQuality = resolution;
    }

    private static void ChangeResolution(QualityType qualityType)
    {
        var newWidth = 0;
        var newHeight = 0;

        switch (qualityType)
        {
            case QualityType.Low:
                newWidth = (int)(DefaultWidthScreen * 0.7f);
                newHeight = (int)(DefaultHeightScreen * 0.7f);
                
                Screen.SetResolution(newWidth, newHeight, true);
                
                break;
            case QualityType.High:
                Screen.SetResolution(DefaultWidthScreen, DefaultHeightScreen, true);
                break;
        }
    }
}