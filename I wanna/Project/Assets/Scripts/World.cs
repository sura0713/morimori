using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class World : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        //Rキーを押したらシーンを再読み込み
        Restart();
	}

    void Restart()
    {
        Scene loadScene = SceneManager.GetActiveScene();

        if (Input.GetKey(KeyCode.R))
        {
            SceneManager.LoadScene(loadScene.name);
        }
    }
}
