using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ImportLocation : MonoBehaviour
{
    public GameObject[] GameBackground;
    public GameObject[] MenuBackground;
    public GameObject[] OtherBackground;
    public Color[] CameraColor;

    [SerializeField]
    private Camera _camera;

    private void Start()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        int buildIndex = currentScene.buildIndex;
        int currentSkin = PlayerPrefs.GetInt(SavesData.CurrentTetrisBackground, 0);

        if (buildIndex == 0)
            Instantiate(MenuBackground[currentSkin], transform);
        if (buildIndex == 1)
            Instantiate(GameBackground[currentSkin], transform);
        if (buildIndex == 2 || buildIndex == 3 || buildIndex == 4)
            Instantiate(OtherBackground[currentSkin], transform);

        _camera.backgroundColor = CameraColor[currentSkin];
    }
}
