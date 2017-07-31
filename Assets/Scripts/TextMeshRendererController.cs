using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextMeshRendererController : MonoBehaviour {
    public string TextMeshLayer;
	// Use this for initialization
	void Start () {
        GetComponent<MeshRenderer>().sortingLayerName = TextMeshLayer;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
