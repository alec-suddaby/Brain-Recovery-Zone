using UnityEngine;

namespace Elixr.MenuSystem
{
    [System.Serializable]
    public class Menu : CompositeNode
    {
        public AudioClip descriptionAudio;
        protected override void OnStop()
        {
        }

        protected override State OnUpdate()
        {
            return State.Running;
        }
    }
}