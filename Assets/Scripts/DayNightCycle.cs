using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    [Header("Cycle Settings")]
    public Light sun; // Assign your Directional Light here
    public float dayDuration = 60f; // Seconds for a full 24-hour cycle
    public Gradient lightColorOverDay;
    public AnimationCurve lightIntensityOverDay;

    private float timeOfDay = 0.5f; // 0 = midnight, 0.5 = noon, 1 = midnight again

    void Update()
    {
        timeOfDay += Time.deltaTime / dayDuration;
        if (timeOfDay > 1f)
            timeOfDay -= 1f;

        UpdateSun();
    }

    void UpdateSun()
    {
        float sunAngle = timeOfDay * 360f - 90f;
        sun.transform.rotation = Quaternion.Euler(sunAngle, 170f, 0f);

        sun.color = lightColorOverDay.Evaluate(timeOfDay);
        sun.intensity = lightIntensityOverDay.Evaluate(timeOfDay);
        RenderSettings.ambientLight = sun.color * 0.5f;

        RenderSettings.fogColor = sun.color * 0.4f;

    }
}
