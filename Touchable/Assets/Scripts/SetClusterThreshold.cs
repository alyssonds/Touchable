using UnityEngine;
using System.Collections;
using Assets.Framework.MultiTouchManager;
using UnityEngine.UI;

public class SetClusterThreshold : MonoBehaviour {

    public Text thresholdText;
    public Slider slider;

    void Start()
    {
        thresholdText.text = "Cluster THR: " + slider.value;
    }

    public void SetSliderClusterThreshold(float value)
    {
        ClusterManager.Instance.SetClusterDistThreshold(value);
        thresholdText.text = "Cluster THR: " + value;
    }
}
