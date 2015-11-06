using UnityEngine;
using System.Collections;
using Assets.Framework.Utils;
using System.Text;

public class TokenStatsDisplay : MonoBehaviour {

    private TokenStatistics ts;
    private int tokenIdentificationPercentage;
    private int totalTokens;
    private int failedIdentificationTokens;
    private float currentTokenIdentificationTime;
    private float avgTokenIdentificationTime;
    private int totalTokenRequestClass;
    private int successfullTokenClassRecon;


	// Use this for initialization
	void Start () {

        ts = TokenStatistics.Instance;

    }
	
	// Update is called once per frame
	void Update () {

        tokenIdentificationPercentage = ts.TokenIdentificationSuccessRate;
        totalTokens = ts.TotalTokens;
        failedIdentificationTokens = ts.FailedIdentificationTokens;

        currentTokenIdentificationTime = ts.LastTokenIdentificationTime;
        avgTokenIdentificationTime = ts.AvgTokenIdentificationTime;

        totalTokenRequestClass = ts.TotalTokenClassRequest;
        successfullTokenClassRecon = ts.SuccessfullTokenClassRecon;

    }

    void OnGUI()
    {
        int w = Screen.width, h = Screen.height;

        GUIStyle style = new GUIStyle();

        Rect rect = new Rect(20, h - 400, 300, 360);
        style.alignment = TextAnchor.UpperLeft;
        style.fontSize = h * 2 / 100;
        style.normal.textColor = new Color(255f, 246f, 229f, 1.0f);
        GUI.Label(rect, Stats(), style);
         
    }

    private string Stats()
    {
        StringBuilder sb = new StringBuilder("Token Stats").AppendLine();
        sb.Append("Tot Tokens: " + totalTokens).AppendLine();
        sb.Append("Failed Idendtification: " + failedIdentificationTokens).AppendLine();
        sb.Append("Successfull Identification: " + tokenIdentificationPercentage + "%").AppendLine();
        sb.Append("Last Token id. Time: " + currentTokenIdentificationTime + " ms").AppendLine();
        sb.Append("AVG Ident. Time: " + avgTokenIdentificationTime + " ms").AppendLine();
        sb.Append("Tot Tokens Class Request: " + totalTokenRequestClass).AppendLine();
        sb.Append("Successfull Class Recon: " + successfullTokenClassRecon);

        return sb.ToString();

    }
}
