using UnityEngine;
using System.Collections;

public class KeyArrow : MonoBehaviour
{
    public float cycle = 1f;
    public float amp = 0.01f;
    private Vector3 initialPos;
    private Quaternion initialRot;

    void Start()
    {
        initialPos = transform.localPosition;
        initialRot = transform.localRotation;
    }

	void Update()
    {
        transform.localRotation = initialRot * Quaternion.Euler(0f, 0f, 360f * Time.time / cycle);
        transform.localPosition = initialPos + amp * Vector3.up * Mathf.Sin(2 * Mathf.PI * Time.time / cycle);
	}
}
