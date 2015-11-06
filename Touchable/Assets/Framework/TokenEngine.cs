using UnityEngine;
using System.Collections;
using Assets.Framework.TokenEngine;
using Assets.Framework.TokenEngine.TokenTypes;
using Assets.Framework.MultiTouchManager;
using Assets.Framework;

public class TokenEngine : MonoBehaviour
{
    public int TokenType;
    public bool ForceClusterThreshold;
    public float ClusterThreshold = 200f;
    public bool Target60FPS = true;
    public bool TokenManagerEnabled;

    public bool MeanSquare;
    public bool ComputePixels;

    public float TranslationThr;
    public float RotationThr;

    void Awake()
    {
        Debug.Log("Token Engine Awake");
        ClusterManager.Instance.Initialize();
        

        switch (TokenType)
        {
            case 3:
                TokenManager.Instance.SetApplicationTokenType(new Token3x3());
                break;
            case 4:
                TokenManager.Instance.SetApplicationTokenType(new Token4x4());
                break;
            case 5:
                TokenManager.Instance.SetApplicationTokenType(new Token5x5());
                break;
        }

        if (ForceClusterThreshold)
            ClusterManager.Instance.SetClusterDistThreshold(ClusterThreshold);
        if (TokenManagerEnabled)
            TokenManager.Instance.Initialize();

        TokenManager.Instance.SetClassComputeReferenceSystem(MeanSquare);
        TokenManager.Instance.SetClassComputeDimensions(ComputePixels);

        TokenManager.Instance.SetTokenUpdateTranslationThr(TranslationThr);
        TokenManager.Instance.SetTokenUpdateRotationThr(RotationThr);

        if (Target60FPS)
            Application.targetFrameRate = 60;
        
    }
    void Start()
    {
    }
    void Update()
    {
        InputServer.Instance.Update();
        InputManager.UpdateFingersCancelled();
    }

    void OnDisable()
    {
        Debug.Log("Token Engine Disabled");
        ClusterManager.Instance.Disable();
        if (TokenManagerEnabled)
            TokenManager.Instance.Disable();
    }

    void OnDestroy()
    {
        Debug.Log("Token On Destroy");
    }


}
