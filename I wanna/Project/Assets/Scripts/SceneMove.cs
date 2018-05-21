using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneMove : MonoBehaviour {

    [SerializeField]
    SCENES scene;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Move();
	}

    void Move()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            SceneManage.SceneMove(scene);
        }
    }
}
