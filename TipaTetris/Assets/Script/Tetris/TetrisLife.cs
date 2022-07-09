using Script.Auxiliary;
using Tetris;
using UnityEngine;

namespace Tetris
{
    public class TetrisLife : MonoBehaviour
    {
        [SerializeField] private GameObject boom;

        private TetrisImageBlock tetrisImage;

        private void Start()
        {
            tetrisImage = GetComponent<TetrisImageBlock>();

            foreach (Transform child in transform)
            {
                child.GetComponent<SpriteRenderer>().color = UsedColor();
            }

            var main = boom.GetComponent<ParticleSystem>().main;
            main.startColor = UsedColor();
        }

        public void DeleteChildBox(Transform child)
        {
            var x = Mathf.RoundToInt(child.position.x);
            var y = Mathf.RoundToInt(child.position.y);
            
            // Call logical function
            tetrisImage.ChangeBlockImage();
            if (Tetroid.grid[x, y] != null)
            {
                Destroy(Tetroid.grid[x, y].gameObject);
                Tetroid.grid[x, y] = null;
            }
            Destroy(child.gameObject);
            // Call visual function
            Instantiate(boom, child.position, Quaternion.identity);
            CameraShakeCenter.Shake?.Invoke();
        }

        private static Color UsedColor()
        {
            ColorUtility.TryParseHtmlString("#" + PlayerPrefs.GetString(SavesData.CurrentTetrisColor, "FFFFFFFF"), out var tempColor);
            return tempColor;
        }
    }
}