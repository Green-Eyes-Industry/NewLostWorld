namespace Controllers
{
    /// <summary> Контроллер эффектов </summary>
    public class EffectsController : ParentController
    {
        public override void Init() { }
        
        public void ShowMessage(IMessage message)
        {
            MainController.instance.uIController.GameMessageMap(message.GetText());
            MainController.instance.animController.GameMessangeEffect(message.GetAnimationType());
        }
    }
}