using UnityEngine;
using System.Collections;

public class Tooltip : MonoBehaviour {

	private float sensitivity = 1f;
	private float tempTime = 0f;
	private bool hideTooltipAtStart = false;
	private bool hideTooltip = true;

	void Start()
	{
		gameObject.SetActive(false);
	}
}
