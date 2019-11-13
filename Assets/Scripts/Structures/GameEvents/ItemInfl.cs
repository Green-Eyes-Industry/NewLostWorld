using UnityEngine;

[CreateAssetMenu(fileName = "New event", menuName = "Игровые обьекты/Новый эвент/Влияние предмета")]
public class ItemInfl : GameEvent
{
    public UsableItem useItem;

    private Player _gamePlayer;

    /// <summary>
    /// Старт события
    /// </summary>
    public override bool EventStart()
    {
        _gamePlayer = GameObject.FindGameObjectWithTag("GameController").GetComponent<DataController>().playerData;

        bool finded = false;

        if (_gamePlayer.playerInventory.Count != 0)
        {
            for (int i = 0; i < _gamePlayer.playerInventory.Count; i++)
            {
                if(_gamePlayer.playerInventory[i] is UsableItem)
                {
                    if(_gamePlayer.playerInventory[i].Equals(useItem))
                    {
                        finded = true;
                        UseThisItem();
                        _gamePlayer.playerInventory.RemoveAt(i);
                        break;
                    }
                }
            }
        }

        return finded;
    }

    /// <summary>
    /// Использовать найденый предмет
    /// </summary>
    private void UseThisItem()
    {
        _gamePlayer.playerHealth += useItem.healthInf;
        _gamePlayer.playerMind += useItem.healthInf;
    }
}