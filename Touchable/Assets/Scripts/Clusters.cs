using UnityEngine;
using System.Collections;
using Assets.Framework.MultiTouchManager;
using System.Collections.Generic;

public class Clusters : MonoBehaviour {

    public GameObject clusterCircle;
    private GameObject[] clusterCircles = new GameObject[11];
    private ClusterManager cm;
    private List<Cluster> clusters;

    // Use this for initialization
    void Start () {

        for (int i = 0; i < 11; i++)
        {
            GameObject c = Instantiate(clusterCircle);
            c.SetActive(false);
            clusterCircles[i] = c;
        }

        cm = ClusterManager.Instance;

    }
	
	// Update is called once per frame
	void Update () {

        clusters = cm.Clusters;

        for (int i = 0; i < clusters.Count; i++)
        {
            clusterCircles[i].SetActive(true);
            Vector3 dstScreen = new Vector3(clusters[i].Centroid.x, clusters[i].Centroid.y, Camera.main.nearClipPlane);
            Vector3 dst = Camera.main.ScreenToWorldPoint(dstScreen);
            dst.z = -9.7f;
            clusterCircles[i].transform.position = dst;
            Vector3 scale = clusterCircles[i].transform.localScale;
            Vector3 scaleDst = new Vector3(0.1f * (1.5f * clusters[i].Points.Count), 0.1f * (1.5f * clusters[i].Points.Count), 0.0f);
            clusterCircles[i].transform.localScale = Vector3.Lerp(scale, scaleDst, 30.0f * Time.deltaTime);

        }

        for (int i = clusters.Count; i < clusterCircles.Length; i++)
        {
            clusterCircles[i].SetActive(false);
        }

    }
}
