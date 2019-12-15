using System.Collections;
using UnityEngine;

/// <summary> Запуск визуального эффекта </summary>
public class EffectHelper : MonoBehaviour
{
    public GameObject particle;

    [System.Obsolete]
    private void OnMouseDown()
    {
        particle.SetActive(true);
        particle.GetComponent<ParticleSystem>().Play();
        StartCoroutine(WaitForDisable());
    }

    /// <summary> Задержка отключения эффекта </summary>
    [System.Obsolete]
    private IEnumerator WaitForDisable()
    {
        yield return new WaitForSeconds(particle.GetComponent<ParticleSystem>().duration * 3);
        particle.SetActive(false);
    }
}