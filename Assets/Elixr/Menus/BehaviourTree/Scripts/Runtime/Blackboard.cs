using System.Collections.Generic;
using UnityEngine.Events;

namespace Elixr.MenuSystem
{

    // This is the blackboard container shared between all nodes.
    // Use this to store temporary data that multiple nodes need read and write access to.
    // Add other properties here that make sense for your specific use case.
    [System.Serializable]
    public class Blackboard
    {
        public int BreadcrumbsCount => breadcrumbs.Count;
        private List<Node> breadcrumbs = new List<Node>();
        public Node[] Breadcrumbs => breadcrumbs.ToArray();

        private Node activeNode;
        public Node ActiveNode
        {
            get => activeNode;

            set
            {
                if (!interactable)
                {
                    return;
                }

                activeNode = value;
                if (!breadcrumbs.Contains(value))
                {
                    breadcrumbs.Add(value);
                }

                ActiveNodeChanged?.Invoke(activeNode);
            }
        }

        public UnityEvent<Node> ActiveNodeChanged;

        public bool interactable = true;

        public void Back()
        {
            if (breadcrumbs.Count > 1)
            {
                breadcrumbs.RemoveAt(breadcrumbs.Count - 1);
                ActiveNode = breadcrumbs[breadcrumbs.Count - 1];
            }
        }

        public void Load(MenuManager menuManager)
        {
            string targetGuid = menuManager.positionTracker.lastMenuPositionGuid;
            if (targetGuid == null || targetGuid == string.Empty)
            {
                return;
            }

            breadcrumbs.Clear();
            Node currentNode = FindMenuItem(menuManager, targetGuid);
            Node originalNode = currentNode;

            bool rootNodeFound = false;
            while (!rootNodeFound)
            {
                breadcrumbs.Insert(0, currentNode);
                currentNode = FindMenuItemParent(menuManager, currentNode.guid);

                rootNodeFound = currentNode is RootNode or null;
            }

            if(originalNode != null && originalNode.BackOnSceneReload)
                Back();
        }

        public void ReloadMenus(MenuManager menuManager)
        {
            string targetGuid = menuManager.positionTracker.lastMenuPositionGuid;
            if (targetGuid == null || targetGuid == string.Empty)
            {
                return;
            }

            breadcrumbs.Clear();
            Node currentNode = FindMenuItem(menuManager, targetGuid);
            Node originalNode = currentNode;

            bool rootNodeFound = false;
            while (!rootNodeFound)
            {
                breadcrumbs.Insert(0, currentNode);
                currentNode = FindMenuItemParent(menuManager, currentNode.guid);

                rootNodeFound = currentNode is RootNode or null;
            }

            menuManager.menuCanvas.interactable = true;
            menuManager.menuCanvas.blocksRaycasts = true;
            ActiveNode = originalNode;
        }

        private Node FindMenuItem(MenuManager menuManager, string guid)
        {
            foreach (Node node in menuManager.menus.nodes)
            {
                if (node.guid == guid)
                {
                    return node;
                }
            }

            throw new KeyNotFoundException($"Node with the GUID: {guid} could not be found.");
        }

        private Node FindMenuItemParent(MenuManager menuManager, string guid)
        {
            foreach (Node node in menuManager.menus.nodes)
            {
                switch (node)
                {
                    case CompositeNode compositeNode:

                        foreach (Node child in compositeNode.children)
                        {
                            if (child.guid == guid)
                            {
                                return compositeNode;
                            }
                        }
                        break;
                    case DecoratorNode decoratorNode:
                        if (decoratorNode.child.guid == guid)
                        {
                            return decoratorNode;
                        }
                        break;
                    default: continue;
                }
            }

            return null;
            //throw new KeyNotFoundException($"Parent of the node with the GUID: {guid} could not be found.");
        }
    }
}