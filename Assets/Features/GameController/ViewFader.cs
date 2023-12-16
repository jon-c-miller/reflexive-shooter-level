using UnityEngine;

///<summary>Handles fading the screen display in and out. Accepts a method as a delegate to execute logic on fade complete.</summary>
public class ViewFader : MonoBehaviour
{
    [SerializeField] Canvas faderCanvas;
	[SerializeField] UnityEngine.UI.Image display;
	[SerializeField] Color fadeColor;
	[SerializeField] float fadeRateModifier = 1;
	[SerializeField] bool showLogs;

	public delegate void CompleteDelegate();
	CompleteDelegate OnComplete;

	Color currentColor;
	bool isFadingIn;
	bool isActive;

	///<summary>Starts fading at desired rate, and executes onComplete upon fade finish. toTransparent true adjusts alpha from black to transparent.</summary>
	public void ExecuteFade(bool toTransparent, float transitionRate, CompleteDelegate onComplete)
	{
		if (showLogs) UnityEngine.Debug.Log($"ExecuteFade() called with {transitionRate} fadeRate value. Fading in = {toTransparent}.");
		isFadingIn = toTransparent;
		currentColor = fadeColor;
		fadeRateModifier = transitionRate;
		OnComplete = onComplete;
		currentColor.a = isFadingIn ? 1 : 0;
		faderCanvas.enabled = true;
		isActive = true;
	}

	void Update()
	{
		if (!isActive) return;

		if (isFadingIn)
		{
			if (currentColor.a < 0)
			{
				currentColor.a = 0;
				display.color = currentColor;
				isActive = false;
				OnComplete?.Invoke();
				return;
			}
			currentColor.a -= Time.deltaTime * fadeRateModifier;
		}
		else
		{
			if (currentColor.a > 1)
			{
				currentColor.a = 1;
				display.color = currentColor;
				isActive = false;
				OnComplete?.Invoke();
				return;
			}
			currentColor.a += Time.deltaTime * fadeRateModifier;
		}

		display.color = currentColor;
	}

	void OnDisable() => OnComplete = null;
}