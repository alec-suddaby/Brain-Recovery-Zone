using Elixr.MenuSystem;
using UnityEngine;
using UnityEngine.UI;

namespace Elixr.MenuSystem
{
    public class LoadLevelOptionsHandler : LoadLevelMenuHandler
    {
        public PreferenceHandler OptionMenu;

        public VerticalLayoutGroup OptionParent;

        public override void SetupMenu(LoadLevelNode level)
        {
            base.SetupMenu(level);

            LoadLevelOptionsNode node = (LoadLevelOptionsNode)level;

            node.Preferences.ForEach(preference =>
            {
                PreferenceHandler preferenceHandler = Instantiate(OptionMenu.gameObject, OptionParent.transform).GetComponent<PreferenceHandler>();
                preferenceHandler.Init(preference);
                LayoutRebuilder.ForceRebuildLayoutImmediate(preferenceHandler.GetComponent<RectTransform>());
            });

            node.extraUiElements.ForEach(uiElement =>
            {
                Instantiate(uiElement, OptionParent.transform);
            });

            LayoutRebuilder.ForceRebuildLayoutImmediate(OptionParent.GetComponent<RectTransform>());
        }
    }
}
