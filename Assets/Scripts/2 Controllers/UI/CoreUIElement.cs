using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace GnomeGardeners
{
    public abstract class CoreUIElement<T> : MonoBehaviour
    {
        public abstract void UpdateUI(T primaryData);
        protected abstract bool ClearedIfEmpty(T newData);

        protected void UpdateText(Text target, string text)
        {
            target.text = text;
        }

        protected void UpdateText(TMP_Text target, string text)
        {
            target.text = text;
        }

        protected void UpdateSprite(Image image, Sprite sprite)
        {
            image.sprite = sprite;
        }

        protected void UpdateNumericText(Text target, string textformatting, float value)
        {
            UpdateText(target, string.Format(textformatting, value));
        }

        protected void UpdateNumericText(TMP_Text target, string textformatting, float value)
        {
            UpdateText(target, string.Format(textformatting, value));
        }

        protected void SetPercentage(Image target, float percent)
        {
            target.fillAmount = percent;
        }

        protected void UpdateTimeAsString(Text target, float timeRemaining)
        {
            int minutes = (int)Mathf.Floor(timeRemaining / 60f);
            int seconds = (int)Mathf.Floor(timeRemaining % 60f);
            UpdateText(target, minutes.ToString() + ":" + seconds.ToString());
        }

        protected void UpdateTimeAsString(TMP_Text target, float timeRemaining)
        {
            int minutes = (int)Mathf.Floor(timeRemaining / 60f);
            int seconds = (int)Mathf.Floor(timeRemaining % 60f);
            UpdateText(target, minutes.ToString() + ":" + seconds.ToString("D2"));
        }

        protected void UpdateSliderValue(Slider target, int value)
        {
            target.value = value;
        }

        protected void UpdateSliderValue(Slider target, float value)
        {
            target.value = value;
        }

        protected void UpdateSliderMinMax(Slider target, int min, int max)
        {
            target.minValue = min;
            target.maxValue = max;
        }
    }

}
