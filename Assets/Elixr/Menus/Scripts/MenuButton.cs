using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Elixr.MenuSystem
{
    [RequireComponent(typeof(Button))]
    public class MenuButton : MonoBehaviour
    {
        public TextMeshProUGUI Title;
        public TextMeshProUGUI Description;

        private MenuManager menuManager;
        private Node node;

        public Button button;

        public GameObject lockedIcon;

        private void Awake()
        {
            lockedIcon.SetActive(false);
        }

        public void SetupButton(Node node, MenuManager menuManager)
        {
            this.menuManager = menuManager;
            this.node = node;

            if (Title != null)
            {
                Title.text = node.title;
            }

            if (Description != null)
            {
                Description.text = node.description;
            }

            button.interactable = node.isEnabled;

            if (node is UnlockableLoadLevel or UnlockableMenu)
            {
                lockedIcon.SetActive(!node.isEnabled);
            }
            else
            {
                lockedIcon.SetActive(false);
            }
        }

        public void ButtonClicked()
        {
            menuManager.menus.blackboard.ActiveNode = node;
        }
    }
}