using System.Collections;
using UnityEngine;

/// <summary> Запуск визуального эффекта </summary>
public class EffectHelper : MonoBehaviour
{
    [SerializeField] private GameObject _particle;

    private void OnMouseDown()
    {
        _particle.SetActive(true);
        _particle.GetComponent<ParticleSystem>().Play();
        StartCoroutine(WaitForDisable());
    }

    /// <summary> Задержка отключения эффекта </summary>
    private IEnumerator WaitForDisable()
    {
        yield return new WaitForSeconds(_particle.GetComponent<ParticleSystem>().duration * 3);
        _particle.SetActive(false);
    }
}