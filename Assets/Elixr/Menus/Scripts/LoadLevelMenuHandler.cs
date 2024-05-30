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

        private LoadLevelNode level;

        public GameObject audioButton;

        private AudioClip descriptionAudio;

        public Transform extraInfoParent;

        private const float sceneFadeTime = 2.5f;

        public virtual void SetupMenu(LoadLevelNode level)
        {
            this.level = level;
            titleText.text = level.title;
            descriptionText.text = level.description;

            descriptionAudio = level.descriptionAudio;
            audioButton.SetActive(descriptionAudio != null);

            if (level.extraMenuInfo != null)
            {
                Instantiate(level.extraMenuInfo, extraInfoParent);
            }
        }

        public void LoadLevel()
        {
            FindObjectOfType<MenuManager>().menus.blackboard.interactable = false;
            FindObjectOfType<SceneFade>().FadeOut();

            FindObjectOfType<LoadLevelSettings>().SetDifficultyLevel(level.DifficultyLevel, level.Stage);

            StartCoroutine("LoadScene");
        }

        private IEnumerator LoadScene()
        {
            yield return new WaitForSeconds(sceneFadeTime);

            SceneManager.LoadScene(level.LevelName);
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