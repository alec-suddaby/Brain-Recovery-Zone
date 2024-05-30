using UnityEngine;

namespace Elixr.MenuSystem
{
    public abstract class DecoratorNode : Node
    {

        [SerializeReference]
        [HideInInspector]
        public Node child;
    }
}
