using UnityEngine;

namespace Script.Auxiliary
{
    public class OpenLink : MonoBehaviour
    {
        public void OpenURL(string url)
        {
            Application.OpenURL(url);
        }
    }
}
