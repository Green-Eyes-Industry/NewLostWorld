using UnityEngine;

namespace NLW
{
    /// <summary> Управление игровыми параметрами </summary>
    [RequireComponent(typeof(DataController))]
    [RequireComponent(typeof(GameController))]
    [RequireComponent(typeof(AnimController))]
    [RequireComponent(typeof(UIController))]
    [RequireComponent(typeof(SoundController))]
    [RequireComponent(typeof(EffectsController))]
    public class MainController : MonoBehaviour
    {
        public static MainController instance;

        [HideInInspector] public DataController dataController;
        [HideInInspector] public GameController gameController;
        [HideInInspector] public AnimController animController;
        [HideInInspector] public UIController uIController;
        [HideInInspector] public SoundController soundController;
        [HideInInspector] public EffectsController effectsController;

        private void Awake()
        {
            instance = this;

            dataController = GetComponent<DataController>();
            gameController = GetComponent<GameController>();
            animController = GetComponent<AnimController>();
            uIController = GetComponent<UIController>();
            soundController = GetComponent<SoundController>();
            effectsController = GetComponent<EffectsController>();
        }

        private void Start()
        {
            dataController.Init();
            gameController.Init();
            animController.Init();
            uIController.Init();
            soundController.Init();
            effectsController.Init();
        }
    }
}