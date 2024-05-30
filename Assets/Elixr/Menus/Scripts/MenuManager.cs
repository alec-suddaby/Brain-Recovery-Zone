using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Elixr.MenuSystem
{
    public class MenuManager : MonoBehaviour
    {

        // The main behaviour tree asset
        public BehaviourTree menus;

        public CanvasGroup menuCanvas;

        public MenuHandler menuPrefab;
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
        public Text breadcrumbsText;
        public string breadcrumbsSeperator = " | ";

        // Start is called before the first frame update
        private void Start()
        {
            PlayerPrefs.SetInt("MemoryMazeLevel", 1);
            PlayerPrefs.SetInt("MemoryMazeStage", 1);
            PlayerPrefs.Save();

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

        private IEnumerator FadeTransition(Node node)
        {
            bool showBackButton = menus.blackboard.BreadcrumbsCount > 1;

            float oneWayTransition = transitionTime / 2f;
            float timeRemaining = oneWayTransition;
            if (!showBackButton && backButton.alpha > 0)
            {
                StartCoroutine(FadeBackButton(showBackButton));
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

            if (showBackButton && backButton.alpha < 1)
            {
                StartCoroutine(FadeBackButton(showBackButton));
            }
            //backButton.gameObject.SetActive(menus.blackboard.BreadcrumbsCount > 1);

            switch (node)
            {
                case Menu menuNode:
                    MenuHandler newMenu = Instantiate(menuPrefab.gameObject, menuCanvas.transform).GetComponent<MenuHandler>();
                    newMenu.SetupMenu(menuNode, this);
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
            float oneWayTransition = transitionTime / 2f;
            float timeRemaining = oneWayTransition;

            while (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                float transitionValue = timeRemaining / oneWayTransition;

                if (visable)
                {
                    transitionValue = 1 - transitionValue;
                }

                transitionValue = Mathf.Clamp01(transitionValue);

                backButton.alpha = transitionValue;

                yield return new WaitForEndOfFrame();
            }
        }

        public void Back()
        {
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
    }
}