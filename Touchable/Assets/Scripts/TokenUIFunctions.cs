using UnityEngine;
using System.Collections;
using Assets.Framework.TokenEngine;
using UnityEngine.UI;

public class TokenUIFunctions : MonoBehaviour {

    public Text RotationThrText;
    public Text TranslationThrTxt;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void TranslationThreshold(float value)
    {
        TokenManager.Instance.SetTokenUpdateTranslationThr(value);
        TranslationThrTxt.text = "Translation Thr: " + value;
    }

    public void RotationThreshold(float value)
    {
        TokenManager.Instance.SetTokenUpdateRotationThr(value);
       RotationThrText.text = "Rotation Thr: " + value;
    }

    public void EnableClassComputeRefSystem(bool value)
    {
        TokenManager.Instance.SetClassComputeReferenceSystem(value);
    }

    public void EnableClassComputeDimensions(bool value)
    {
        TokenManager.Instance.SetClassComputeDimensions(value);
    }
}
