
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FpsToText : MonoBehaviour{

	[SerializeField ]
	TextMeshProUGUI text;

	[SerializeField, Range(0, 5)]
	int second = 1;

	int frameCount = 0;
	float oldTime = 0.0f;

	void Start()
	{
		text =  GameObject.Find("FPSCounter").GetComponent<TextMeshProUGUI>();
	}
	void Update(){
		
		frameCount++;

		
		float time = Time.realtimeSinceStartup - oldTime;

	
		if(time >= second){
			
			float fps = frameCount / time;

			
			text.SetText("{0:2} FPS", fps);

			
			frameCount = 0;
			oldTime = Time.realtimeSinceStartup;
		}
	}

}