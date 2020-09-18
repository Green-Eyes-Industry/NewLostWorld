using Controllers;
using UnityEngine;

/// <summary> Управление игровыми параметрами </summary>
[RequireComponent(typeof(DataController), typeof(GameController), typeof(AnimController))]
[RequireComponent(typeof(SoundController), typeof(UIController), typeof(EffectsController))]
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

    private void Update() => uIController.IUpdate();
}