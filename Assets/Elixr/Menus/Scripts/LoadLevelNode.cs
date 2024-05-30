using UnityEngine;

namespace Elixr.MenuSystem
{
    public class LoadLevelNode : ActionNode
    {
        public string LevelName;
        public int DifficultyLevel = 1;
        public int Stage = 0;
        public AudioClip descriptionAudio;
        public GameObject extraMenuInfo;
        protected override void OnStop()
        {
        }

        protected override State OnUpdate()
        {
            return State.Running;
        }
    }
}
