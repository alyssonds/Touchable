using UnityEngine;
using System.Collections;
using Assets.Framework.TokenEngine;
using Assets.Framework.TokenEngine.TokenTypes;

public class TokenEngine : MonoBehaviour
{
    public int TokenType;   

    void Awake()
    {
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
        
    }

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }
}
