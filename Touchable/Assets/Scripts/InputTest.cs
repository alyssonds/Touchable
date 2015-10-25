using UnityEngine;
using System.Collections;
using Assets.Framework.MultiTouchManager;
using Assets.Framework;
using System.Collections.Generic;
using System.Text;

public class InputTest : MonoBehaviour {

    public GameObject circle;
    private GameObject[] circles;
    private List<Cluster> clusters;
    public Camera camera;

	// Use this for initialization
	void Start () {

        ClusterManager.Instance.Initialize().SetClusterDistThreshold(200f);
        circles = new GameObject[10];

        for(int i=0; i < 10; i++)
        {
            GameObject c = Instantiate(circle);
            c.SetActive(false);
            c.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            circles[i] = c;
     
        }
       
    }
	
	// Update is called once per frame
	void Update () {

        //int i = Touches.BeganTouches.Count;
        InputServer.Instance.Update();
        //Debug.Log(ClusterDebug());
        Debug.Log(""+ InputManager.FingersCount());

        clusters = ClusterManager.Instance.Clusters;

        for(int i=0; i < clusters.Count; i++)
        {
            circles[i].SetActive(true);
            Vector3 pos = circles[i].transform.position;
            Vector3 dstScreen = new Vector3(clusters[i].Centroid.x, clusters[i].Centroid.y, camera.nearClipPlane);
            Vector3 dst = Camera.main.ScreenToWorldPoint(dstScreen);
            dst.z = -9.7f;
            circles[i].transform.position = Vector3.Lerp(pos, dst, 50.0f * Time.deltaTime);
            Vector3 scale = circles[i].transform.localScale;
            Vector3 scaleDst = new Vector3(0.1f * (1.5f * clusters[i].Points.Count), 0.1f * (1.5f * clusters[i].Points.Count),0.0f);
            circles[i].transform.localScale = Vector3.Lerp(scale, scaleDst, 50.0f * Time.deltaTime);

        }


        for(int i = clusters.Count; i < circles.Length; i++)
        {
            circles[i].SetActive(false);
        }

	}

    private void OnInputsUpdated(object sender, InputUpdateEventArgs e)
    {
        //Debug.Log("Inputs: " + InternalTouches.List.Count + " B: " + InternalTouches.BaganTouhBuffer.Count + " M: " + InternalTouches.MovedTouchBuffer.Count + " C: " + InternalTouches.CancelledTouchBuffer.Count);
    }

    private string ClusterDebug()
    {
        StringBuilder sb = new StringBuilder("Clusters");
        foreach(Cluster c in ClusterManager.Instance.Clusters)
        {
            sb.Append("C: ").Append(c.Hash).Append(" P: ").Append(c.Points.Count).Append(" // ");
        }

        return sb.ToString();
    }
}
