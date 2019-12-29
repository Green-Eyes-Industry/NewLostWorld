using Data;
using Helpers;

namespace Controllers
{
    /// <summary> Контроллер эффектов </summary>
    public class EffectsController : ParentController
    {
        public override void Init() { }

        /// <summary> Сообщение о получении предмета </summary>
        public void AddItemMessage(GameItem item)
        {
            MainController.instance.uIController.GameMessageInventory("Получен предмет\n" + item.itemName);
            MainController.instance.animController.GameMessangeEffect(AnimController.MessangeType.ITEM_MS);
        }

        /// <summary> Сообщение о потере предмета </summary>
        public void LostItemMessage(GameItem item)
        {
            MainController.instance.uIController.GameMessageInventory("Потерян предмет\n" + item.itemName);
            MainController.instance.animController.GameMessangeEffect(AnimController.MessangeType.ITEM_MS);
        }

        /// <summary> Сообщение о наложении еффекта на персонажа </summary>
        public void AddEffectMessage(GameEffect effect)
        {
            MainController.instance.uIController.GameMessageCharacter("Получен еффект\n" + effect.nameEffect);
            MainController.instance.animController.GameMessangeEffect(AnimController.MessangeType.EFFECT_MS);
        }

        /// <summary> Сообщение о снатии эффекта с персонажа </summary>
        public void LostEffectMessage(GameEffect effect)
        {
            MainController.instance.uIController.GameMessageCharacter("Снят еффект\n" + effect.nameEffect);
            MainController.instance.animController.GameMessangeEffect(AnimController.MessangeType.EFFECT_MS);
        }

        /// <summary> Сообщение о получении заметки </summary>
        public void AddNoteMessage(Note note)
        {
            MainController.instance.uIController.GameMessageNotes("Новая заметка\n" + note.noteName);
            MainController.instance.animController.GameMessangeEffect(AnimController.MessangeType.NOTE_MS);
        }

        /// <summary> Сообщение об открытии новой територии </summary>
        public void AddMapMarkMessage(MapMark mapMark)
        {
            MainController.instance.uIController.GameMessageMap("Найдена локация\n" + mapMark.nameLocation);
            MainController.instance.animController.GameMessangeEffect(AnimController.MessangeType.NOTE_MS);
        }
    }
}