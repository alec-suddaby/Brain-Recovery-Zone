using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Elixr.MenuSystem
{
    public class MenuManager : MonoBehaviour
    {

        // The main behaviour tree asset
        public BehaviourTree menus;

        public CanvasGroup menuCanvas;

        public MenuHandler menuPrefab;
        public VideoLoaderHandler videoHandler;
        public LoadLevelMenuHandler loadLevelPrefab;
        public LoadLevelOptionsHandler loadLevelOptionsPrefab;


        public CanvasGroup backButton;

        // Storage container object to hold game object subsystems
        private Context context;

        public float transitionTime = 1f;

        public MenuPositionTracker positionTracker;

        public float backButtonCooldown = 1f;
        private float lastBackButtonClicktime = float.MinValue;

        public CanvasGroup breadcrumbsCanvas;
        public TextMeshProUGUI breadcrumbsText;
        public string breadcrumbsSeperator = " | ";

        [SerializeField] private LikertScaleManager likertScaleManager;
        public LikertScaleManager LikertManager => likertScaleManager;

        [SerializeField] private CanvasGroup showButton;

        [SerializeField] private CanvasGroup optionsPanel;

        [SerializeField] private CanvasGroup settingsPanel;

        private bool ShowBackButton => menus.blackboard.BreadcrumbsCount > 1;

        private bool SettingsPanelActive = false;
        
        // Start is called before the first frame update
        private void Start()
        {
            SetSettingsPanelVisibility(false);
            ToggleShowHideButtons(false);

            backButton.alpha = 0f;
            context = CreateBehaviourTreeContext();
            menus = menus.Clone();
            menus.Bind(context);
            if (menus)
            {
                if (positionTracker == null)
                {
                    Debug.Log($"Position Tracker is null");
                }
                menus.blackboard.ActiveNodeChanged.AddListener(MenuChanged);

                if (positionTracker.lastMenuPositionGuid == null || positionTracker.lastMenuPositionGuid == string.Empty || positionTracker.lastMenuPositionGuid == "")
                {
                    Debug.Log("No menu recorded");
                    menus.Update();
                }
                else
                {
                    Debug.Log("Loading menu");
                    menus.blackboard.Load(this);
                }
            }
        }

        private Context CreateBehaviourTreeContext()
        {
            return Context.CreateFromGameObject(gameObject);
        }

        private void MenuChanged(Node node)
        {
            positionTracker.lastMenuPositionGuid = node.guid;
            StartCoroutine(FadeTransition(node));
        }

        public IEnumerator Fade(float alpha, float duration, bool interactable = true, CanvasGroup canvasGroup = null, float startDelay = 0f)
        {
            yield return new WaitForSeconds(startDelay);

            if (canvasGroup == null)
            {
                canvasGroup = menuCanvas;
            }

            if (interactable)
            {
                canvasGroup.blocksRaycasts = interactable;
                canvasGroup.interactable = interactable;
            }

            float initialAlpha = canvasGroup.alpha;
            float alphaChange = alpha - initialAlpha;

            float timeElapsed = 0;

            while (timeElapsed < duration)
            {
                timeElapsed += Time.deltaTime;

                float elapsedAmount = timeElapsed / duration;

                canvasGroup.alpha = Mathf.Clamp01(initialAlpha + (elapsedAmount * alphaChange));

                yield return new WaitForEndOfFrame();
            }

            if (!interactable)
            {
                canvasGroup.blocksRaycasts = interactable;
                canvasGroup.interactable = interactable;
            }

            canvasGroup.alpha = alpha;
        }

        private IEnumerator FadeTransition(Node node)
        {
            float oneWayTransition = transitionTime / 2f;
            float timeRemaining = oneWayTransition;
            if (!ShowBackButton && backButton.alpha > 0)
            {
                StartCoroutine(FadeBackButton(ShowBackButton));
            }

            while (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                float alpha = Mathf.Clamp01(timeRemaining / oneWayTransition);
                menuCanvas.alpha = alpha;
                breadcrumbsCanvas.alpha = alpha;
                yield return new WaitForEndOfFrame();
            }

            foreach (Transform menuItem in menuCanvas.transform)
            {
                DestroyImmediate(menuItem.gameObject);
            }

            breadcrumbsText.text = GetBreadcrumbs();

            if (ShowBackButton && backButton.alpha < 1)
            {
                StartCoroutine(FadeBackButton(ShowBackButton));
            }
            //backButton.gameObject.SetActive(menus.blackboard.BreadcrumbsCount > 1);

            switch (node)
            {
                case Menu menuNode:
                    MenuHandler newMenu = Instantiate(menuPrefab.gameObject, menuCanvas.transform).GetComponent<MenuHandler>();
                    newMenu.SetupMenu(menuNode, this);
                    break;
                case VideoLoader videoLoader:
                    VideoLoaderHandler newVideoLoader = Instantiate(videoHandler.gameObject, menuCanvas.transform).GetComponent<VideoLoaderHandler>();
                    newVideoLoader.SetupMenu(videoLoader);
                    break;
                case LoadLevelOptionsNode levelOptionsMenu:
                    LoadLevelMenuHandler newLevelOptionsMenu = Instantiate(loadLevelOptionsPrefab.gameObject, menuCanvas.transform).GetComponent<LoadLevelOptionsHandler>();
                    newLevelOptionsMenu.SetupMenu(levelOptionsMenu);
                    break;
                case LoadLevelNode levelMenuNode:
                    LoadLevelMenuHandler newLevelMenu = Instantiate(loadLevelPrefab.gameObject, menuCanvas.transform).GetComponent<LoadLevelMenuHandler>();
                    newLevelMenu.SetupMenu(levelMenuNode);
                    break;
                default:
                    break;
            }

            timeRemaining = 0;
            while (timeRemaining < oneWayTransition)
            {
                timeRemaining += Time.deltaTime;
                float alpha = Mathf.Clamp01(timeRemaining / oneWayTransition);
                menuCanvas.alpha = alpha;
                breadcrumbsCanvas.alpha = alpha;
                yield return new WaitForEndOfFrame();
            }
        }

        private string GetBreadcrumbs()
        {
            List<Node> currentMenus = menus.blackboard.Breadcrumbs.ToList();

            if (currentMenus.Count == 0)
            {
                return "";
            }

            currentMenus.RemoveAt(currentMenus.Count - 1);

            if (currentMenus.Count == 0)
            {
                return "";
            }

            currentMenus.RemoveAt(0);

            List<string> menuNames = new List<string>();
            foreach (Node currentMenu in currentMenus)
            {
                string title = currentMenu.title.Trim();

                if (title != string.Empty && title != "")
                {
                    menuNames.Add(title);
                }

            }

            return string.Join(breadcrumbsSeperator, menuNames);
        }

        private IEnumerator FadeBackButton(bool visable)
        {
            float startAlpha = backButton.alpha;
            float alphaChange = (visable ? 1f : 0f) - startAlpha;

            float oneWayTransition = transitionTime / 2f;
            float fadeTime = 0;

            while (fadeTime < oneWayTransition)
            {
                fadeTime += Time.deltaTime;
                float transitionValue = alphaChange * (fadeTime/oneWayTransition);

                transitionValue = Mathf.Clamp01(transitionValue);

                backButton.alpha = transitionValue;

                yield return new WaitForEndOfFrame();
            }
        }

        public void Back()
        {
            if (SettingsPanelActive)
            {
                SetSettingsPanelVisibility(false);

                return;
            }

            if (likertScaleManager.IsActive)
            {
                likertScaleManager.Close();
                return;
            }

            if (Time.time - lastBackButtonClicktime < backButtonCooldown)
            {
                return;
            }

            lastBackButtonClicktime = Time.time;

            if (menus == null)
            {
                return;
            }

            menus.blackboard.Back();
        }

        private void OnDrawGizmosSelected()
        {
            if (!menus)
            {
                return;
            }

            BehaviourTree.Traverse(menus.rootNode, (n) =>
            {
                if (n.drawGizmos)
                {
                    n.OnDrawGizmos();
                }
            });
        }

        public void ToggleShowHideButtons(bool show)
        {
            StartCoroutine(Fade(show ? 1 : 0, transitionTime/2f, show, showButton));
            StartCoroutine(Fade(show ? 0 : 1, transitionTime/2f, !show, optionsPanel));
        }

        public void ToggleSettingsPanel()
        {
            SetSettingsPanelVisibility(!SettingsPanelActive);
        }

        private void SetSettingsPanelVisibility(bool visible)
        {
            CanvasGroup otherFadeCanvas = likertScaleManager.IsActive ? likertScaleManager.Panel : menuCanvas;
            StartCoroutine(Fade(!visible ? 1 : 0, transitionTime / 2f, !visible, otherFadeCanvas));

            StartCoroutine(Fade(visible ? 1 : 0, transitionTime/2f, visible, settingsPanel));

            SettingsPanelActive = visible;
        }

        public void ForceSettingsPanelVisibility(bool visible)
        {
            StartCoroutine(Fade(visible ? 1 : 0, transitionTime / 2f, visible, settingsPanel));

            SettingsPanelActive = visible;
        }
    }
}