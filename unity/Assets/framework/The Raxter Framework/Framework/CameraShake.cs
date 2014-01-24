using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Camera))]
public class CameraShake : MonoBehaviour
{
	float _shakeIntensity = 0f;
	
	[SerializeField]
	float _shakeIntensityMultiplier = 1f;
	
	[SerializeField]
	float _shakeDecay = 0.7f;
	
	Vector2 _offset = Vector2.zero;
	
	Vector3 _originalPosition;
	
	
	// Update is called once per frame
	void LateUpdate () 
	{
		_originalPosition = transform.position - (Vector3)_offset;
		
		if (Input.GetKeyDown(KeyCode.Space))
		{
			_shakeIntensity += 1f;
		}
		
//		Debug.Log (_shakeIntensity);
		_offset = Random.insideUnitCircle * _shakeIntensity;
		
		_shakeIntensity *= _shakeDecay;
		
		transform.position = _originalPosition + (Vector3)_offset;
	}
	
	
	public static void ShakeMainCamera(float shakeToAdd)
	{
		CameraShake cameraShake = Camera.main.GetComponent<CameraShake>();
		
		if (cameraShake != null) cameraShake._shakeIntensity += shakeToAdd*cameraShake._shakeIntensityMultiplier;
	}
}
