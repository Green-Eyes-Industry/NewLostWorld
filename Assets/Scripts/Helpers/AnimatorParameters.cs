namespace Helpers
{
    /// <summary> Имена параметров в аниматоре </summary>
    public struct AnimatorParameters
    {
        #region VARIABLE

        // Меню

        public string mStady;
        public string gStady;

        // Кнопки меню

        public string buttonMenu_1;
        public string buttonMenu_2;
        public string buttonMenu_3;
        public string buttonMenu_4;

        public string buttonAchiveCases;
        public string buttonAchiveCloseCases;

        public string buttonAchiveLeft;
        public string buttonAchiveRight;
        public string buttonAchiveBack;

        // Кнопки в игре

        public string buttonGame;

        public string buttonEventLeft;
        public string buttonEventRight;

        public string buttonInventory;
        public string buttonPlayer;
        public string buttonMap;
        public string buttonNote;

        // Инвентарь

        public string buttonGameInventInfo;
        public string buttonGameInventUse;
        public string buttonGameInventRemove;

        public string buttonGameInventNullInfo;
        public string buttonGameInventNullUse;
        public string buttonGameInventNullRemove;

        public string buttonGameInventLeft;
        public string buttonGameInventRight;

        public string buttonGameInventCases;

        public string buttonInventoryIYes;
        public string buttonInventoryINo;

        // Окно игрока

        public string buttonPlayerCases;
        public string buttonPlayerBack;

        // Эффекты

        public string stadySettings_1;
        public string stadySettings_2;
        public string stadySettings_3;

        public string achiveDescript;
        public string achiveSlidePage;

        public string startGameDeskript;
        public string effectSave;
        public string switchGameText;
        public string returnToMenu;
        public string switchTextToAbout;

        public string messangeInventory;
        public string messangePlayer;
        public string messangeMap;
        public string messangeNote;

        public string gameInventDescript;
        public string gamePlayerDescript;

        public string inventCaseSelected;

        public string gameInventPage;

        #endregion

        public void InitParam()
        {
            // Меню

            mStady = "MenuStady";
            gStady = "GameStady";

            // Кнопки

            buttonMenu_1 = "Menu_1_Button";
            buttonMenu_2 = "Menu_2_Button";
            buttonMenu_3 = "Menu_3_Button";
            buttonMenu_4 = "Menu_4_Button";

            buttonAchiveCases = "AchiveCase_";
            buttonAchiveCloseCases = "AchiveCaseClose_";

            buttonAchiveLeft = "Achives_Left";
            buttonAchiveRight = "Achives_Right";
            buttonAchiveBack = "Achives_Back";

            // Кнопки в игре

            buttonGame = "GameButton_";

            buttonEventLeft = "GameButton_EvLeft";
            buttonEventRight = "GameButton_EvRight";

            buttonInventory = "Btb_Inventory";
            buttonPlayer = "Btb_Player";
            buttonMap = "Btb_Map";
            buttonNote = "Btb_Notes";

            buttonGameInventInfo = "GameInvent_Bt_Info";
            buttonGameInventUse = "GameInvent_Bt_Use";
            buttonGameInventRemove = "GameInvent_Bt_Remove";

            buttonGameInventNullInfo = "GameInventNull_Bt_Info";
            buttonGameInventNullUse = "GameInventNull_Bt_Use";
            buttonGameInventNullRemove = "GameInventNull_Bt_Remove";

            buttonGameInventLeft = "GameInvent_Bt_Left";
            buttonGameInventRight = "GameInvent_Bt_Right";

            buttonGameInventCases = "GameInvent_Case_";

            buttonInventoryIYes = "InventoryIYes";
            buttonInventoryINo = "InventoryINo";

            buttonPlayerCases = "GamePlayer_Case_";
            buttonPlayerBack = "GamePlayer_DescriptBack";

            // Эффекты

            stadySettings_1 = "Settings_1_St";
            stadySettings_2 = "Settings_2_St";
            stadySettings_3 = "Settings_3_St";

            achiveDescript = "AchiveDescript";
            achiveSlidePage = "AchiveSlidePage";

            startGameDeskript = "StartGameDescript";
            effectSave = "Effect_Save";
            switchGameText = "SwitchGameText";
            returnToMenu = "ReturnToMenu";
            switchTextToAbout = "SwitchTextToAbout";

            messangeInventory = "Messange_Item";
            messangePlayer = "Messange_Player";
            messangeMap = "Messange_Map";
            messangeNote = "Messange_Note";

            gameInventDescript = "GameInvent_DescriptMenu";
            gamePlayerDescript = "GamePlayer_Descript";

            inventCaseSelected = "InventSelectedCase";

            gameInventPage = "GameInvent_Page";
        }
    }
}