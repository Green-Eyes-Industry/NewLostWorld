using System.IO;
using UnityEngine;
using UnityEditor;

namespace GUIInspector.NodeEditor
{
    public class GraphChangeController : Editor
    {
        #region DATA

        public static GraphChangeController graphCC;

        public static GamePart selectedNode;

        #endregion

        #region ADD_PART

        public enum UserActions
        {
            ADD_TEXT_PART,
            ADD_CHANGE_PART,
            ADD_BATTLE_PART,
            ADD_MAZE_PART,
            ADD_EVENT_PART,
            ADD_FINAL_PART,
            ADD_LABEL_PART,
            ADD_SLIDESHOW_PART,
            ADD_TRANSIT
        }

        /// <summary> Создать новую ноду </summary>
        public static void AddNewNode(Event e)
        {
            if (graphCC == null) graphCC = (GraphChangeController)CreateInstance(typeof(GraphChangeController));

            GenericMenu menu = new GenericMenu();
            menu.AddItem(new GUIContent("Создать главу/Текстовая"), false, graphCC.AddNodeToWindow, UserActions.ADD_TEXT_PART);
            menu.AddItem(new GUIContent("Создать главу/Выбора"), false, graphCC.AddNodeToWindow, UserActions.ADD_CHANGE_PART);
            menu.AddItem(new GUIContent("Создать главу/Боя"), false, graphCC.AddNodeToWindow, UserActions.ADD_BATTLE_PART);
            menu.AddItem(new GUIContent("Создать главу/Загадка"), false, graphCC.AddNodeToWindow, UserActions.ADD_MAZE_PART);
            menu.AddItem(new GUIContent("Создать главу/Эвент"), false, graphCC.AddNodeToWindow, UserActions.ADD_EVENT_PART);
            menu.AddItem(new GUIContent("Создать главу/Финальная"), false, graphCC.AddNodeToWindow, UserActions.ADD_FINAL_PART);
            menu.AddItem(new GUIContent("Создать главу/Вставка"), false, graphCC.AddNodeToWindow, UserActions.ADD_LABEL_PART);
            menu.AddItem(new GUIContent("Создать главу/Слайдшоу"), false, graphCC.AddNodeToWindow, UserActions.ADD_SLIDESHOW_PART);
            menu.ShowAsContext();
            e.Use();
        }

        /// <summary> Проверка действия </summary>
        private void AddNodeToWindow(object o)
        {
            UserActions a = (UserActions)o;

            string pathToNode;
            bool sizeStady = false;
            Rect nodeRect;

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
                case UserActions.ADD_LABEL_PART: nameNode = numNode + "_LeandPart"; break;
                case UserActions.ADD_SLIDESHOW_PART: nameNode = numNode + "_MoviePart"; break;
                case UserActions.ADD_TRANSIT: nameNode = numNode + ""; break;
            }

            pathToNode = "Assets/Resources/GameParts/" + nameNode + ".asset";

            if (BehaviorEditor.storyData.nodesData != null)
            {
                if(BehaviorEditor.storyData.nodesData.Count > 0)
                {
                    sizeStady = BehaviorEditor.storyData.nodesData[0].windowSizeStady;
                }
            }
            else BehaviorEditor.storyData.nodesData = new System.Collections.Generic.List<GamePart>();


            if (sizeStady)
            {
                nodeRect = new Rect(
                    BehaviorEditor._mousePosition.x,
                    BehaviorEditor._mousePosition.y,
                    BehaviorEditor.storyData.baseNodeSmWidth,
                    BehaviorEditor.storyData.baseNodeSmHeight);
            }
            else
            {
                nodeRect = new Rect(
                    BehaviorEditor._mousePosition.x,
                    BehaviorEditor._mousePosition.y,
                    BehaviorEditor.storyData.baseNodeLgWidth,
                    BehaviorEditor.storyData.baseNodeLgHeight);
            }

            switch (a)
            {
                case UserActions.ADD_TEXT_PART:
                    AssetDatabase.CreateAsset(CreateInstance(typeof(TextPart)), pathToNode);
                    TextPart textPart = (TextPart)AssetDatabase.LoadAssetAtPath(pathToNode, typeof(TextPart));

                    textPart.windowSizeStady = sizeStady;
                    if (sizeStady)
                    {
                        textPart.windowTitle = nameNode.Substring(0, GetShortNameNode(nameNode));
                        textPart._memTitle = nameNode;
                    }
                    else textPart.windowTitle = nameNode;

                    textPart.windowRect = nodeRect;
                    BehaviorEditor.storyData.nodesData.Add(textPart);
                    break;

                case UserActions.ADD_CHANGE_PART:
                    AssetDatabase.CreateAsset(CreateInstance(typeof(ChangePart)), pathToNode);
                    ChangePart changePart = (ChangePart)AssetDatabase.LoadAssetAtPath(pathToNode, typeof(ChangePart));

                    changePart.windowSizeStady = sizeStady;
                    if (sizeStady)
                    {
                        changePart.windowTitle = nameNode.Substring(0, GetShortNameNode(nameNode));
                        changePart._memTitle = nameNode;
                    }
                    else changePart.windowTitle = nameNode;

                    changePart.windowRect = nodeRect;
                    BehaviorEditor.storyData.nodesData.Add(changePart);
                    break;

                case UserActions.ADD_BATTLE_PART:
                    AssetDatabase.CreateAsset(CreateInstance(typeof(BattlePart)), pathToNode);
                    BattlePart battlePart = (BattlePart)AssetDatabase.LoadAssetAtPath(pathToNode, typeof(BattlePart));

                    battlePart.windowSizeStady = sizeStady;
                    if (sizeStady)
                    {
                        battlePart.windowTitle = nameNode.Substring(0, GetShortNameNode(nameNode));
                        battlePart._memTitle = nameNode;
                    }
                    else battlePart.windowTitle = nameNode;

                    battlePart.windowRect = nodeRect;
                    BehaviorEditor.storyData.nodesData.Add(battlePart);
                    break;

                case UserActions.ADD_MAZE_PART:
                    AssetDatabase.CreateAsset(CreateInstance(typeof(PazzlePart)), pathToNode);
                    PazzlePart pazzlePart = (PazzlePart)AssetDatabase.LoadAssetAtPath(pathToNode, typeof(PazzlePart));

                    pazzlePart.windowSizeStady = sizeStady;
                    if (sizeStady)
                    {
                        pazzlePart.windowTitle = nameNode.Substring(0, GetShortNameNode(nameNode));
                        pazzlePart._memTitle = nameNode;
                    }
                    else pazzlePart.windowTitle = nameNode;

                    pazzlePart.windowRect = nodeRect;
                    BehaviorEditor.storyData.nodesData.Add(pazzlePart);
                    break;

                case UserActions.ADD_EVENT_PART:
                    AssetDatabase.CreateAsset(CreateInstance(typeof(EventPart)), pathToNode);
                    EventPart eventPart = (EventPart)AssetDatabase.LoadAssetAtPath(pathToNode, typeof(EventPart));

                    eventPart.windowSizeStady = sizeStady;
                    if (sizeStady)
                    {
                        eventPart.windowTitle = nameNode.Substring(0, GetShortNameNode(nameNode));
                        eventPart._memTitle = nameNode;
                    }
                    else eventPart.windowTitle = nameNode;

                    eventPart.windowRect = nodeRect;
                    BehaviorEditor.storyData.nodesData.Add(eventPart);
                    break;

                case UserActions.ADD_FINAL_PART:
                    AssetDatabase.CreateAsset(CreateInstance(typeof(FinalPart)), pathToNode);
                    FinalPart finalPart = (FinalPart)AssetDatabase.LoadAssetAtPath(pathToNode, typeof(FinalPart));

                    finalPart.windowSizeStady = sizeStady;
                    if (sizeStady)
                    {
                        finalPart.windowTitle = nameNode.Substring(0, GetShortNameNode(nameNode));
                        finalPart._memTitle = nameNode;
                    }
                    else finalPart.windowTitle = nameNode;

                    finalPart.windowRect = nodeRect;
                    BehaviorEditor.storyData.nodesData.Add(finalPart);
                    break;

                case UserActions.ADD_LABEL_PART:
                    AssetDatabase.CreateAsset(CreateInstance(typeof(LeandPart)), pathToNode);
                    LeandPart leandPart = (LeandPart)AssetDatabase.LoadAssetAtPath(pathToNode, typeof(LeandPart));

                    leandPart.windowSizeStady = sizeStady;
                    if (sizeStady)
                    {
                        leandPart.windowTitle = nameNode.Substring(0, GetShortNameNode(nameNode));
                        leandPart._memTitle = nameNode;
                    }
                    else leandPart.windowTitle = nameNode;

                    leandPart.windowRect = nodeRect;
                    BehaviorEditor.storyData.nodesData.Add(leandPart);
                    break;

                case UserActions.ADD_SLIDESHOW_PART:
                    AssetDatabase.CreateAsset(CreateInstance(typeof(MoviePart)), pathToNode);
                    MoviePart moviePart = (MoviePart)AssetDatabase.LoadAssetAtPath(pathToNode, typeof(MoviePart));

                    moviePart.windowSizeStady = sizeStady;
                    if (sizeStady)
                    {
                        moviePart.windowTitle = nameNode.Substring(0, GetShortNameNode(nameNode));
                        moviePart._memTitle = nameNode;
                    }
                    else moviePart.windowTitle = nameNode;

                    moviePart.windowRect = nodeRect;
                    BehaviorEditor.storyData.nodesData.Add(moviePart);
                    break;

                case UserActions.ADD_TRANSIT: break;
            }

            
            BehaviorEditor.SaveData();
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
            for (int i = 0; i < 4; i++)
            {
                if (longName[i] == '_') return i;
            }

            return 4;
        }

        #endregion

        #region ADD_EVENT

        public enum AddEventActions
        {
            CHECK_DECISION,
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

        public enum HelpNodeActions
        {
            EDIT_EVENT_PART
        }

        /// <summary> Добавить событие к главе </summary>
        public static void AddEventToPart(Event e)
        {
            if (graphCC == null) graphCC = (GraphChangeController)CreateInstance(typeof(GraphChangeController));

            GenericMenu menu = new GenericMenu();
            
            if (selectedNode is EventPart)
            {
                menu.AddItem(new GUIContent("Открыть редактор евента"), false, graphCC.AddPartToEvent, HelpNodeActions.EDIT_EVENT_PART);
            }
            else
            {
                menu.AddItem(new GUIContent("Добавить событие/Контрольная точка"), false, graphCC.AddEventMethod, AddEventActions.CHECK_POINT);
                menu.AddItem(new GUIContent("Добавить событие/Важное решение"), false, graphCC.AddEventMethod, AddEventActions.IMPORTANT_DECISION);
                menu.AddItem(new GUIContent("Добавить событие/Проверка решения"), false, graphCC.AddEventMethod, AddEventActions.CHECK_DECISION);
                menu.AddItem(new GUIContent("Добавить событие/Влияние на игрока"), false, graphCC.AddEventMethod, AddEventActions.PLAYER_INFL);
                menu.AddItem(new GUIContent("Добавить событие/Влияние на НПС"), false, graphCC.AddEventMethod, AddEventActions.NON_PLAYER_INFL);
                menu.AddItem(new GUIContent("Добавить событие/Проверка влияния персонажа"), false, graphCC.AddEventMethod, AddEventActions.CHECK_PLAYER_INFL);
                menu.AddItem(new GUIContent("Добавить событие/Взаимодействие с эффектом"), false, graphCC.AddEventMethod, AddEventActions.EFFECT_INTERACT);
                menu.AddItem(new GUIContent("Добавить событие/Взаимодействие с предметом"), false, graphCC.AddEventMethod, AddEventActions.ITEM_INTERACT);
                menu.AddItem(new GUIContent("Добавить событие/Использование предмета"), false, graphCC.AddEventMethod, AddEventActions.ITEM_INFL);
                menu.AddItem(new GUIContent("Добавить событие/Найдена локация"), false, graphCC.AddEventMethod, AddEventActions.LOCATION_FIND);
                menu.AddItem(new GUIContent("Добавить событие/Воспоминание"), false, graphCC.AddEventMethod, AddEventActions.MEMBER_TIME);
                menu.AddItem(new GUIContent("Добавить событие/Случайный переход"), false, graphCC.AddEventMethod, AddEventActions.RANDOM_PART);
            }
            
            menu.ShowAsContext();
            e.Use();
        }

        /// <summary> Проверка действия добавления евента </summary>
        private void AddEventMethod(object o)
        {
            AddEventActions a = (AddEventActions)o;

            string path = "Assets/Resources/GameEvents/";
            string nameEvent = selectedNode.mainEvents.Count.ToString();

            switch (a)
            {
                case AddEventActions.CHECK_DECISION: nameEvent += "_CheckDecision.asset"; break;
                case AddEventActions.CHECK_PLAYER_INFL: nameEvent += "_CheckPlayerInfl.asset"; break;
                case AddEventActions.CHECK_POINT: nameEvent += "_CheckPoint.asset"; break;
                case AddEventActions.EFFECT_INTERACT: nameEvent += "_EffectInteract.asset"; break;
                case AddEventActions.IMPORTANT_DECISION: nameEvent += "_ImportantDecision.asset"; break;
                case AddEventActions.ITEM_INFL: nameEvent += "_ItemInfl.asset"; break;
                case AddEventActions.ITEM_INTERACT: nameEvent += "_ItemInteract.asset"; break;
                case AddEventActions.LOCATION_FIND: nameEvent += "_LocationFind.asset"; break;
                case AddEventActions.MEMBER_TIME: nameEvent += "_MemberTime.asset"; break;
                case AddEventActions.NON_PLAYER_INFL: nameEvent += "_NonPlayerInfl.asset"; break;
                case AddEventActions.PLAYER_INFL: nameEvent += "_PlayerInfl.asset"; break;
                case AddEventActions.RANDOM_PART: nameEvent += "_RandomPart.asset"; break;
            }

            switch (a)
            {
                case AddEventActions.CHECK_DECISION:
                    AssetDatabase.CreateAsset(CreateInstance(typeof(CheckDecision)), path + nameEvent);
                    selectedNode.mainEvents.Add((CheckDecision)AssetDatabase.LoadAssetAtPath(path + nameEvent, typeof(CheckDecision)));
                    break;

                case AddEventActions.CHECK_PLAYER_INFL:
                    AssetDatabase.CreateAsset(CreateInstance(typeof(CheckPlayerInfl)), path + nameEvent);
                    selectedNode.mainEvents.Add((CheckPlayerInfl)AssetDatabase.LoadAssetAtPath(path + nameEvent, typeof(CheckPlayerInfl)));
                    break;

                case AddEventActions.CHECK_POINT:
                    AssetDatabase.CreateAsset(CreateInstance(typeof(CheckPoint)), path + nameEvent);
                    selectedNode.mainEvents.Add((CheckPoint)AssetDatabase.LoadAssetAtPath(path + nameEvent, typeof(CheckPoint)));
                    break;

                case AddEventActions.EFFECT_INTERACT:
                    AssetDatabase.CreateAsset(CreateInstance(typeof(EffectInteract)), path + nameEvent);
                    selectedNode.mainEvents.Add((EffectInteract)AssetDatabase.LoadAssetAtPath(path + nameEvent, typeof(EffectInteract)));
                    break;

                case AddEventActions.IMPORTANT_DECISION:
                    AssetDatabase.CreateAsset(CreateInstance(typeof(ImportantDecision)), path + nameEvent);
                    selectedNode.mainEvents.Add((ImportantDecision)AssetDatabase.LoadAssetAtPath(path + nameEvent, typeof(ImportantDecision)));
                    break;

                case AddEventActions.ITEM_INFL:
                    AssetDatabase.CreateAsset(CreateInstance(typeof(ItemInfl)), path + nameEvent);
                    selectedNode.mainEvents.Add((ItemInfl)AssetDatabase.LoadAssetAtPath(path + nameEvent, typeof(ItemInfl)));
                    break;

                case AddEventActions.ITEM_INTERACT:
                    AssetDatabase.CreateAsset(CreateInstance(typeof(ItemInteract)), path + nameEvent);
                    selectedNode.mainEvents.Add((ItemInteract)AssetDatabase.LoadAssetAtPath(path + nameEvent, typeof(ItemInteract)));
                    break;

                case AddEventActions.LOCATION_FIND:
                    AssetDatabase.CreateAsset(CreateInstance(typeof(LocationFind)), path + nameEvent);
                    selectedNode.mainEvents.Add((LocationFind)AssetDatabase.LoadAssetAtPath(path + nameEvent, typeof(LocationFind)));
                    break;

                case AddEventActions.MEMBER_TIME:
                    AssetDatabase.CreateAsset(CreateInstance(typeof(MemberTime)), path + nameEvent);
                    selectedNode.mainEvents.Add((MemberTime)AssetDatabase.LoadAssetAtPath(path + nameEvent, typeof(MemberTime)));
                    break;

                case AddEventActions.NON_PLAYER_INFL:
                    AssetDatabase.CreateAsset(CreateInstance(typeof(NonPlayerInfl)), path + nameEvent);
                    selectedNode.mainEvents.Add((NonPlayerInfl)AssetDatabase.LoadAssetAtPath(path + nameEvent, typeof(NonPlayerInfl)));
                    break;

                case AddEventActions.PLAYER_INFL:
                    AssetDatabase.CreateAsset(CreateInstance(typeof(PlayerInfl)), path + nameEvent);
                    selectedNode.mainEvents.Add((PlayerInfl)AssetDatabase.LoadAssetAtPath(path + nameEvent, typeof(PlayerInfl)));
                    break;

                case AddEventActions.RANDOM_PART:
                    AssetDatabase.CreateAsset(CreateInstance(typeof(RandomPart)), path + nameEvent);
                    selectedNode.mainEvents.Add((RandomPart)AssetDatabase.LoadAssetAtPath(path + nameEvent, typeof(RandomPart)));
                    break;
            }
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