using UnityEngine;

public class CamShakeBehavior : MonoBehaviour
{
	// Transform of the camera to shake. Grabs the gameObject's transform
	// if null.
	public Transform camTransform;

	float _startingShakeDuration;
	float _shakeTime;

	public float decreaseFactor = 1.0f;

	float currentShakeIntensity;
	float startingShakeIntensity;

	Vector3 originalPos;

	void Awake()
	{
		if (camTransform == null) {
			camTransform = GetComponent(typeof(Transform)) as Transform;
		}
	}

	void OnEnable()
	{
		originalPos = camTransform.localPosition;
	}

	void Update()
	{
		if (_shakeTime > 0) {
			camTransform.localPosition = originalPos + Random.insideUnitSphere * currentShakeIntensity;

			_shakeTime -= Time.deltaTime * decreaseFactor;
			currentShakeIntensity = startingShakeIntensity * _shakeTime / _startingShakeDuration;
		} else {
			_shakeTime = 0f;
			camTransform.localPosition = originalPos;
		}
	}

	public void Shake(float duration, float intensity)
    {
		_startingShakeDuration = duration;
		_shakeTime = duration;
		currentShakeIntensity = intensity;
		startingShakeIntensity = currentShakeIntensity;
    }
}