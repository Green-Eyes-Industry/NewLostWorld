using System.IO;
using Data;
using Data.GameEvents;
using Data.GameParts;
using UnityEditor;
using UnityEngine;

namespace Editor.NodeEditor
{
    public class GraphChangeController : UnityEditor.Editor
    {
        #region DATA

        private static GraphChangeController _graphCc;

        public static GamePart selectedNode;

        #endregion

        #region ADD_PART

        private enum UserActions
        {
            ADD_TEXT_PART,
            ADD_CHANGE_PART,
            ADD_BATTLE_PART,
            ADD_MAZE_PART,
            ADD_EVENT_PART,
            ADD_FINAL_PART,
            ADD_LEAND_PART,
            ADD_MOVIE_PART
        }

        /// <summary> Создать новую ноду </summary>
        public static void AddNewNode(Event e)
        {
            if (_graphCc == null) _graphCc = (GraphChangeController)CreateInstance(typeof(GraphChangeController));

            GenericMenu menu = new GenericMenu();
            menu.AddItem(new GUIContent("Создать главу/Один вариант ответа"), false, _graphCc.AddNodeToWindow, UserActions.ADD_TEXT_PART);
            menu.AddItem(new GUIContent("Создать главу/Два варианта ответа"), false, _graphCc.AddNodeToWindow, UserActions.ADD_CHANGE_PART);
            menu.AddItem(new GUIContent("Создать главу/Три варианта ответа"), false, _graphCc.AddNodeToWindow, UserActions.ADD_BATTLE_PART);
            menu.AddItem(new GUIContent("Создать главу/Событие на время"), false, _graphCc.AddNodeToWindow, UserActions.ADD_EVENT_PART);
            menu.AddItem(new GUIContent("Создать главу/Головоломка"), false, _graphCc.AddNodeToWindow, UserActions.ADD_MAZE_PART);
            menu.AddItem(new GUIContent("Создать главу/Текстовая вставка"), false, _graphCc.AddNodeToWindow, UserActions.ADD_LEAND_PART);
            menu.AddItem(new GUIContent("Создать главу/Слайдшоу"), false, _graphCc.AddNodeToWindow, UserActions.ADD_MOVIE_PART);
            menu.AddItem(new GUIContent("Создать главу/Финальная"), false, _graphCc.AddNodeToWindow, UserActions.ADD_FINAL_PART);
            menu.ShowAsContext();
            e.Use();
        }

        /// <summary> Проверка действия </summary>
        private void AddNodeToWindow(object o)
        {
            UserActions a = (UserActions)o;

            bool sizeStady = false;
            GamePart part;

            int numNode = 0;

            do { numNode++; } while (PartNameExistor(numNode));

            string nameNode = numNode.ToString();

            switch (a)
            {
                case UserActions.ADD_TEXT_PART: nameNode = numNode + "_TextPart"; break;
                case UserActions.ADD_CHANGE_PART: nameNode = numNode + "_ChangePart"; break;
                case UserActions.ADD_BATTLE_PART: nameNode = numNode + "_BattlePart"; break;
                case UserActions.ADD_MAZE_PART: nameNode = numNode + "_PazzlePart"; break;
                case UserActions.ADD_EVENT_PART: nameNode = numNode + "_EventPart"; break;
                case UserActions.ADD_FINAL_PART: nameNode = numNode + "_FinalPart"; break;
                case UserActions.ADD_LEAND_PART: nameNode = numNode + "_LeandPart"; break;
                case UserActions.ADD_MOVIE_PART: nameNode = numNode + "_MoviePart"; break;
            }

            string  pathToNode = "Assets/Resources/GameParts/" + nameNode + ".asset";

            if (BehaviorEditor.storyData.nodesData != null && BehaviorEditor.storyData.nodesData.Count > 0)
                sizeStady = BehaviorEditor.storyData.nodesData[0].windowSizeStady;
            else BehaviorEditor.storyData.nodesData = new System.Collections.Generic.List<GamePart>();


            Rect nodeRect = new Rect(
                BehaviorEditor.mousePosition.x,
                BehaviorEditor.mousePosition.y,
                (sizeStady) ? BehaviorEditor.storyData.baseNodeSmWidth : BehaviorEditor.storyData.baseNodeLgWidth,
                (sizeStady) ? BehaviorEditor.storyData.baseNodeSmHeight : BehaviorEditor.storyData.baseNodeLgHeight);

            switch (a)
            {
                case UserActions.ADD_TEXT_PART: CreateObjectPart<TextPart>(pathToNode, nameNode, sizeStady, nodeRect, new GamePart[1]); break;
                case UserActions.ADD_CHANGE_PART: CreateObjectPart<ChangePart>(pathToNode, nameNode, sizeStady, nodeRect, new GamePart[2]); break;
                case UserActions.ADD_BATTLE_PART: CreateObjectPart<BattlePart>(pathToNode, nameNode, sizeStady, nodeRect, new GamePart[3]); break;
                case UserActions.ADD_MAZE_PART: CreateObjectPart<PazzlePart>(pathToNode, nameNode, sizeStady, nodeRect, new GamePart[3]); break;
                case UserActions.ADD_EVENT_PART: break;
                case UserActions.ADD_FINAL_PART: CreateObjectPart<FinalPart>(pathToNode, nameNode, sizeStady, nodeRect, new GamePart[0]); break;
                case UserActions.ADD_LEAND_PART: CreateObjectPart<LeandPart>(pathToNode, nameNode, sizeStady, nodeRect, new GamePart[1]); break;
                case UserActions.ADD_MOVIE_PART: CreateObjectPart<MoviePart>(pathToNode, nameNode, sizeStady, nodeRect, new GamePart[1]); break;
            }

            BehaviorEditor.SaveData();
        }

        private void CreateObjectPart<T>(string path, string nameNode, bool size, Rect nodeRect, GamePart[] movePart) where T : GamePart
        {
            AssetDatabase.CreateAsset(CreateInstance(typeof(T)), path);
            T part = (T)AssetDatabase.LoadAssetAtPath(path, typeof(T));

            part.movePart = movePart;

            if (part is BattlePart battlePart) battlePart.buttonText = new string[3];
            else if (part is ChangePart changePart) changePart.buttonText = new string[2];
            
            part.windowSizeStady = size;
            if (size)
            {
                part.windowTitle = nameNode.Substring(0, GetShortNameNode(nameNode));
                part.memTitle = nameNode;
            }
            else part.windowTitle = nameNode;

            part.windowRect = nodeRect;
            BehaviorEditor.storyData.nodesData.Add(part);
        }

        /// <summary> Поиск дубликатов </summary>
        private bool PartNameExistor(int partId)
        {
            if (File.Exists("Assets/Resources/GameParts/" + partId + "_TextPart" + ".asset")) return true;
            if (File.Exists("Assets/Resources/GameParts/" + partId + "_ChangePart" + ".asset")) return true;
            if (File.Exists("Assets/Resources/GameParts/" + partId + "_BattlePart" + ".asset")) return true;
            if (File.Exists("Assets/Resources/GameParts/" + partId + "_EventPart" + ".asset")) return true;
            if (File.Exists("Assets/Resources/GameParts/" + partId + "_PazzlePart" + ".asset")) return true;
            if (File.Exists("Assets/Resources/GameParts/" + partId + "_FinalPart" + ".asset")) return true;
            if (File.Exists("Assets/Resources/GameParts/" + partId + "_MoviePart" + ".asset")) return true;
            if (File.Exists("Assets/Resources/GameParts/" + partId + "_LeandPart" + ".asset")) return true;

            return false;
        }

        /// <summary> Насколько сокращать имя </summary>
        private int GetShortNameNode(string longName)
        {
            for (int i = 0; i < 4; i++) if (longName[i] == '_') return i;

            return 4;
        }

        #endregion

        #region ADD_EVENT

        private enum AddEventActions
        {
            CHECK_DECISION,
            CHECK_HEALTH,
            CHECK_MIND,
            CHECK_PLAYER_INFL,
            CHECK_POINT,
            EFFECT_INTERACT,
            IMPORTANT_DECISION,
            ITEM_INFL,
            ITEM_INTERACT,
            LOCATION_FIND,
            MEMBER_TIME,
            NON_PLAYER_INFL,
            PLAYER_INFL,
            RANDOM_PART
        }

        private enum HelpNodeActions
        {
            EDIT_EVENT_PART
        }

        /// <summary> Добавить событие к главе </summary>
        public static void AddEventToPart(Event e)
        {
            if (_graphCc == null) _graphCc = (GraphChangeController)CreateInstance(typeof(GraphChangeController));

            GenericMenu menu = new GenericMenu();
            
            if (selectedNode is EventPart) menu.AddItem(new GUIContent("Открыть редактор евента"), false, _graphCc.AddPartToEvent, HelpNodeActions.EDIT_EVENT_PART);
            else
            {
                menu.AddItem(new GUIContent("Добавить событие/Контрольная точка"), false, _graphCc.AddEventMethod, AddEventActions.CHECK_POINT);
                menu.AddItem(new GUIContent("Добавить событие/Проверка здоровья"), false, _graphCc.AddEventMethod, AddEventActions.CHECK_HEALTH);
                menu.AddItem(new GUIContent("Добавить событие/Проверка рассудка"), false, _graphCc.AddEventMethod, AddEventActions.CHECK_MIND);
                menu.AddItem(new GUIContent("Добавить событие/Важное решение"), false, _graphCc.AddEventMethod, AddEventActions.IMPORTANT_DECISION);
                menu.AddItem(new GUIContent("Добавить событие/Проверка решения"), false, _graphCc.AddEventMethod, AddEventActions.CHECK_DECISION);
                menu.AddItem(new GUIContent("Добавить событие/Влияние на игрока"), false, _graphCc.AddEventMethod, AddEventActions.PLAYER_INFL);
                menu.AddItem(new GUIContent("Добавить событие/Влияние на отношение НПС"), false, _graphCc.AddEventMethod, AddEventActions.NON_PLAYER_INFL);
                menu.AddItem(new GUIContent("Добавить событие/Проверка отношения с НПС"), false, _graphCc.AddEventMethod, AddEventActions.CHECK_PLAYER_INFL);
                menu.AddItem(new GUIContent("Добавить событие/Наложение или потеря эффекта"), false, _graphCc.AddEventMethod, AddEventActions.EFFECT_INTERACT);
                menu.AddItem(new GUIContent("Добавить событие/Найти потерять предмет"), false, _graphCc.AddEventMethod, AddEventActions.ITEM_INTERACT);
                menu.AddItem(new GUIContent("Добавить событие/Использовать предмет"), false, _graphCc.AddEventMethod, AddEventActions.ITEM_INFL);
                menu.AddItem(new GUIContent("Добавить событие/Найдена локация"), false, _graphCc.AddEventMethod, AddEventActions.LOCATION_FIND);
                menu.AddItem(new GUIContent("Добавить событие/Найдена заметка"), false, _graphCc.AddEventMethod, AddEventActions.MEMBER_TIME);
                menu.AddItem(new GUIContent("Добавить событие/Случайный переход"), false, _graphCc.AddEventMethod, AddEventActions.RANDOM_PART);
            }
            
            menu.ShowAsContext();
            e.Use();
        }

        /// <summary> Проверка действия добавления евента </summary>
        private void AddEventMethod(object o)
        {
            AddEventActions a = (AddEventActions)o;

            const string path = "Assets/Resources/GameEvents/";
            string nameEvent = selectedNode.mainEvents.Count.ToString() + "_" + selectedNode.name;

            switch (a)
            {
                case AddEventActions.CHECK_POINT: CreateEventObject<CheckPoint>(path + nameEvent + "_CheckPoint.asset"); break;
                case AddEventActions.CHECK_HEALTH: CreateEventObject<PlayerHealthCheck>(path + nameEvent + "_CheckHealth.asset"); break;
                case AddEventActions.CHECK_MIND: CreateEventObject<PlayerMindCheck>(path + nameEvent + "_CheckMind.asset"); break;
                case AddEventActions.CHECK_DECISION: CreateEventObject<CheckDecision>(path + nameEvent + "_CheckDecision.asset"); break;
                case AddEventActions.CHECK_PLAYER_INFL: CreateEventObject<CheckPlayerInfl>(path + nameEvent + "_CheckPlayerInfl.asset"); break;
                case AddEventActions.EFFECT_INTERACT: CreateEventObject<EffectInteract>(path + nameEvent + "_EffectInteract.asset"); break;
                case AddEventActions.IMPORTANT_DECISION: CreateEventObject<ImportantDecision>(path + nameEvent + "_ImportantDecision.asset"); break;
                case AddEventActions.ITEM_INFL: CreateEventObject<ItemInfl>(path + nameEvent + "_ItemInfl.asset"); break;
                case AddEventActions.ITEM_INTERACT: CreateEventObject<ItemInteract>(path + nameEvent + "_ItemInteract.asset"); break;
                case AddEventActions.LOCATION_FIND: CreateEventObject<LocationFind>(path + nameEvent + "_LocationFind.asset"); break;
                case AddEventActions.MEMBER_TIME: CreateEventObject<MemberTime>(path + nameEvent + "_MemberTime.asset"); break;
                case AddEventActions.NON_PLAYER_INFL: CreateEventObject<NonPlayerInfl>(path + nameEvent + "_NonPlayerInfl.asset"); break;
                case AddEventActions.PLAYER_INFL: CreateEventObject<PlayerInfl>(path + nameEvent + "_PlayerInfl.asset"); break;
                case AddEventActions.RANDOM_PART: CreateEventObject<RandomPart>(path + nameEvent + "_RandomPart.asset"); break;
            }
        }

        private void CreateEventObject<T>(string path) where T : GameEvent
        {
            AssetDatabase.CreateAsset(CreateInstance(typeof(T)), path);
            selectedNode.mainEvents.Add((T)AssetDatabase.LoadAssetAtPath(path, typeof(T)));
        }

        /// <summary> Открыть редактор евента </summary>
        private void AddPartToEvent(object o)
        {
            EventPart ePart = (EventPart)selectedNode;
            EventEditor.eventGraph = ePart;
        }

        #endregion
    }
}