using Zenta.Core.Runtime.Managers;

namespace Zenta.Core.Runtime.UI.Buttons
{
    public class PlayButton : ButtonBase
    {
        protected override void Start()
        {
            this.onClick.AddListener(() =>
            {
                GameManager.Instance.StartLevel();
            });

            base.Start();
        }
    }
}