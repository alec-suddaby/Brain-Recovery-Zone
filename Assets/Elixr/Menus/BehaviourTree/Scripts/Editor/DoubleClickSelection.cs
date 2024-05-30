using UnityEditor;
using UnityEngine.UIElements;

namespace Elixr.MenuSystem
{
    public class DoubleClickSelection : MouseManipulator
    {
        private double time;
        private double doubleClickDuration = 0.3;

        public DoubleClickSelection()
        {
            time = EditorApplication.timeSinceStartup;
        }

        protected override void RegisterCallbacksOnTarget()
        {
            target.RegisterCallback<MouseDownEvent>(OnMouseDown);
        }

        protected override void UnregisterCallbacksFromTarget()
        {

            target.UnregisterCallback<MouseDownEvent>(OnMouseDown);
        }

        private void OnMouseDown(MouseDownEvent evt)
        {
            if (target is not BehaviourTreeView graphView)
            {
                return;
            }

            double duration = EditorApplication.timeSinceStartup - time;
            if (duration < doubleClickDuration)
            {
                SelectChildren(evt);
            }

            time = EditorApplication.timeSinceStartup;
        }

        private void SelectChildren(MouseDownEvent evt)
        {
            if (target is not BehaviourTreeView graphView)
            {
                return;
            }

            if (!CanStopManipulation(evt))
            {
                return;
            }

            if (evt.target is not NodeView clickedElement)
            {
                var ve = evt.target as VisualElement;
                clickedElement = ve.GetFirstAncestorOfType<NodeView>();
                if (clickedElement == null)
                {
                    return;
                }
            }

            // Add children to selection so the root element can be moved
            BehaviourTree.Traverse(clickedElement.node, node =>
            {
                var view = graphView.FindNodeView(node);
                graphView.AddToSelection(view);
            });
        }
    }
}