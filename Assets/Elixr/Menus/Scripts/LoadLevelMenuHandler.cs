using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Elixr.MenuSystem
{
    public class LoadLevelMenuHandler : MonoBehaviour
    {
        public Text titleText;

        public Text descriptionText;

        public LoadLevelNode Level;

        public GameObject audioButton;

        private AudioClip descriptionAudio;

        public Transform extraInfoParent;

        private const float sceneFadeTime = 2.5f;

        public virtual void SetupMenu(LoadLevelNode level)
        {
            Level = level;

            titleText.text = level.title;
            descriptionText.text = level.description;

            descriptionAudio = level.descriptionAudio;
            audioButton.SetActive(descriptionAudio != null);

            if (level.extraMenuInfo != null)
            {
                Instantiate(level.extraMenuInfo, extraInfoParent);
            }
        }

        public virtual void LoadLevel()
        {
            FindObjectOfType<MenuManager>().menus.blackboard.interactable = false;

            FindObjectOfType<LoadLevelSettings>().SetDifficultyLevel(Level.DifficultyLevel, Level.Stage);

            FindObjectOfType<SceneLoader>().LoadNewScene(Level.LevelName);
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