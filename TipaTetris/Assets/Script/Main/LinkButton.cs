using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinkButton : MonoBehaviour
{
    public void OpenYouTube() {
        Application.OpenURL("https://www.youtube.com/channel/UCVtzyzGo1JrK_IVNV0DvFLQ");
    }

    public void OpenInstagram() {
        Application.OpenURL("https://www.instagram.com/en_i_st/");
    }
}
