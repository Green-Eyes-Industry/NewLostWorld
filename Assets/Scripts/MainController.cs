using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Управление игровыми параметрами
/// </summary>
public class MainController : MonoBehaviour
{
    private DataController _dataController;
    private MoveController _moveController;

    private void Start()
    {
        ConnectControllers();
        ConnectComponents();
    }

    /// <summary>
    /// Подключение контроллеров
    /// </summary>
    private void ConnectControllers()
    {
        _moveController = GetComponent<MoveController>();
    }

    /// <summary>
    /// Подключение компонентов
    /// </summary>
    private void ConnectComponents()
    {
        _moveController.Init();
    }
}