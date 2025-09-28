using UnityEngine;

public class ColorByHeight : MonoBehaviour
{
    [Header("Настройки градиента")]
    [Tooltip("Позиция по Y (0=низ, 1=верх), ВЫШЕ которой спрайт будет полностью светлым (topColor).")]
    [Range(0f, 1f)]
    public float gradientStartPoint = 0.5f; // Начинаем затемнять, когда объект прошел 20% экрана сверху

    [Tooltip("Позиция по Y (0=низ, 1=верх), НИЖЕ которой спрайт будет полностью темным (bottomColor).")]
    [Range(0f, 1f)]
    public float gradientEndPoint = 0f; // Объект становится полностью темным у самого низа экрана

    [Header("Настройки цвета")]
    [Tooltip("Цвет, который будет у спрайта, когда он находится вверху экрана.")]
    public Color topColor = Color.white;

    [Tooltip("Цвет, который будет у спрайта, когда он находится внизу экрана.")]
    public Color bottomColor = new Color(0.2f, 0.2f, 0.2f, 1f);

    private SpriteRenderer spriteRenderer;
    private Camera mainCamera;

    void Start()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError("SpriteRenderer не найден!", this.gameObject);
            enabled = false;
            return;
        }
        mainCamera = Camera.main;
    }

    void Update()
    {
        // 1. Конвертируем позицию в координаты вьюпорта
        Vector3 viewportPos = mainCamera.WorldToViewportPoint(transform.position);

        // 2. ВЫЧИСЛЯЕМ ФАКТОР ГРАДИЕНТА (САМОЕ ГЛАВНОЕ ИЗМЕНЕНИЕ)
        // Используем InverseLerp, чтобы найти, на каком "проценте" пути
        // находится объект между нашими двумя точками градиента.
        float t = Mathf.InverseLerp(gradientEndPoint, gradientStartPoint, viewportPos.y);

        // 3. Clamp01 все еще полезен, чтобы значения не выходили за пределы 0-1
        t = Mathf.Clamp01(t);

        // 4. Смешиваем цвета, как и раньше
        Color newColor = Color.Lerp(bottomColor, topColor, t);

        // 5. Применяем цвет
        spriteRenderer.color = newColor;
    }
}