using UnityEngine;
using UnityEngine.UI;

namespace GnomeGardeners
{

    [RequireComponent(typeof(Button))]
    public class ButtonSFXController : MonoBehaviour
    {
        private Button button;

        // Start is called before the first frame update
        void Start()
        {
            button = GetComponent<Button>();
            button.onClick.AddListener(PlayPressSound);
        }

        private void PlayPressSound()
        {
            GameManager.Instance.AudioManager.PlaySound(SoundType.sfx_button_press);
        }
    }
}
