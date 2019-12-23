using UnityEngine;

namespace NLW
{
    /// <summary> Управление игровыми параметрами </summary>
    [RequireComponent(typeof(DataController))]
    [RequireComponent(typeof(GameController))]
    [RequireComponent(typeof(AnimController))]
    [RequireComponent(typeof(UIController))]
    [RequireComponent(typeof(SoundController))]
    public abstract class MainController : MonoBehaviour
    {
        public static MainController Instance;

        [HideInInspector] public DataController dataController;
        [HideInInspector] public GameController gameController;
        [HideInInspector] public AnimController animController;
        [HideInInspector] public UIController uIController;
        [HideInInspector] public SoundController soundController;

        /// <summary> Инициализация контроллера </summary>
        protected abstract void Init();

        private void Awake()
        {
            Instance = this;

            dataController = GetComponent<DataController>();
            gameController = GetComponent<GameController>();
            animController = GetComponent<AnimController>();
            uIController = GetComponent<UIController>();
            soundController = GetComponent<SoundController>();
        }

        private void Start()
        {
            dataController.Init();
            gameController.Init();
            animController.Init();
            uIController.Init();
            soundController.Init();
        }
    }
}