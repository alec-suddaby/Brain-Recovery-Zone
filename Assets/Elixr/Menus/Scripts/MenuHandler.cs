using UnityEngine;
using UnityEngine.UI;

namespace Elixr.MenuSystem
{
    public class MenuHandler : MonoBehaviour
    {
        public Text titleText;

        public Text descriptionText;

        public Transform buttonParent;

        public MenuButton buttonPrefab;

        public GridLayoutGroup buttonLayout;

        public GameObject audioButton;

        private AudioClip descriptionAudio;

        public void SetupMenu(Menu menu, MenuManager menuManager)
        {
            descriptionAudio = menu.descriptionAudio;
            audioButton.SetActive(descriptionAudio != null);

            titleText.text = menu.title;
            descriptionText.text = menu.description;

            if (menu.children.Count == 4)
            {
                buttonLayout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
                buttonLayout.constraintCount = 2;
            }

            menu.children.ForEach(child =>
            {
                MenuButton button = Instantiate(buttonPrefab.gameObject, buttonParent).GetComponent<MenuButton>();
                button.SetupButton(child, menuManager);
            });
        }

        public void PlayAudioDescription()
        {
            AudioTrigger audioTrigger;
            if (audioTrigger = FindObjectOfType<AudioTrigger>().GetComponent<AudioTrigger>())
            {
                audioTrigger.PlayAudio(descriptionAudio);
            }
        }
    }
}