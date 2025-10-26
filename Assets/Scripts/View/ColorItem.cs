using ColorSorter.Game;
using UnityEngine;
using UnityEngine.UI;

namespace ColorSorter.View
{
    public class ColorItem : MonoBehaviour
    {
        [SerializeField] private Image icon;

        public void SetColor(ColorType color)
        {
            switch (color)
            {
                case ColorType.Red:
                    icon.color = Color.red;
                    break;
                case ColorType.Blue:
                    icon.color = Color.blue;
                    break;
                default:
                    icon.color = Color.white;
                    break;
            }
        }

        public void SetActive(bool isActive)
        {
            gameObject.SetActive(isActive);
        }

        public void SetHighlighted(bool isHighlighted)
        {
            // 맨 앞 강조
            Color currentColor = icon.color;
            currentColor.a = isHighlighted ? 1f : 0.5f;
            icon.color = currentColor;
        }
    }
}
