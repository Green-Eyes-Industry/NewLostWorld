using UnityEngine;

/// <summary>
/// Запуск эффекта
/// </summary>
public class EffectHelper : MonoBehaviour
{
    [SerializeField] private ParticleSystem _particle;

    private void OnMouseDown() => _particle.Play();
}