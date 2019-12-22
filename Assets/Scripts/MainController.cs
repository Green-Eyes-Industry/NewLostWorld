using UnityEngine;

namespace NLW
{
    /// <summary> Управление игровыми параметрами </summary>
    [RequireComponent(typeof(DataController))]
    [RequireComponent(typeof(GameController))]
    [RequireComponent(typeof(AnimController))]
    [RequireComponent(typeof(UIController))]
    [RequireComponent(typeof(SoundController))]
    public class MainController : MonoBehaviour
    {
        public static MainController Instance;

        [HideInInspector] public Data.Player mainPlayer;
        [HideInInspector] public Data.GameSettings mainSettings;

        [HideInInspector] public DataController dataController;
        [HideInInspector] public GameController gameController;
        [HideInInspector] public AnimController animController;
        [HideInInspector] public UIController uIController;
        [HideInInspector] public SoundController soundController;

        /// <summary> Инициализация контроллера </summary>
        protected virtual void Init() { }

        private void Start()
        {
            Instance = this;
            mainPlayer = (Data.Player)Resources.Load("Players/MainPlayer", typeof(Data.Player));
            mainSettings = (Data.GameSettings)Resources.Load("MainSettings", typeof(Data.GameSettings));

            if (mainPlayer == null)
            {
                Debug.LogError("Отсутствует компонент : Player");
                return;
            }

            if (mainSettings == null)
            {
                Debug.LogError("Отсутствует компонент : GameSettings");
                return;
            }

            dataController = GetComponent<DataController>();
            gameController = GetComponent<GameController>();
            animController = GetComponent<AnimController>();
            uIController = GetComponent<UIController>();
            soundController = GetComponent<SoundController>();

            dataController.Init();
            gameController.Init();
            animController.Init();
            uIController.Init();
            soundController.Init();
        }
    }
}