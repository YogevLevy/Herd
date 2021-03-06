﻿using UnityEngine;
using System;
using System.Collections;

public class GoalController : MonoBehaviour {
    
    private bool[] sheepsArrived;

	public GameObject[] sheeps;

    public GameObject levelEndDialog;

    public GameObject theCamera;

    public string nextLevelName;

    private bool levelCompleted = false;
    
    void Start() {
		sheepsArrived = new bool[10];
    }

    void OnCollisionEnter(Collision collision)
    {
		Debug.Log ("Collision: " + collision.collider.name);
        if (collision.collider.name.Contains("Sheep")){
			int sheepNum = getSheepNum(collision.collider.name);
            sheepsArrived[sheepNum] = true;

			//Debug.Log("Sheep" + sheepNum + " is in the goal zone");

			if (allSheep()) {
				Debug.Log("All the sheeps are in the goal zone");
				int score = calculateSickness();
				Debug.Log("Player scored " + score + " out of 1000");

                levelEndDialog.SetActive(true);

                theCamera.transform.position = new Vector3(0f, 10f, -10f);
                
                theCamera.transform.LookAt(levelEndDialog.transform.position);

                levelCompleted = true;
            }
        }
    }


    
    void OnCollisionExit(Collision collision)
    {
        if (!collision.collider.name.Contains("Sheep")) {
            return;
        }

        int sheepNum = getSheepNum(collision.collider.name);

        bool reallyWithinGoal = false;
        if(sheeps[sheepNum].transform.position.x > 3 &&
            sheeps[sheepNum].transform.position.z > 3)
        {
            reallyWithinGoal = true;
        }

		if (!reallyWithinGoal)
        {			
			sheepsArrived[sheepNum] = false;            

			//Debug.Log("Sheep" + sheepNum + " has left the goal zone");
		}
    }


	bool allSheep () {
		// goes over the sheeps array, returns true if all the array is true (i.e.  all sheeps are in the goal zone)
		int i;
		for (i=0; i<10; i++) {
			if (!sheepsArrived[i]) {
				return false;
			}
		}

		return true;
	}

	int getSheepNum (string sheep) {
		//All sheep have a number: Takes the sheep's number from it's name -> Takes the 6th char, turns it into a number (double), converts it to int

		return Convert.ToInt32(Char.GetNumericValue(sheep[5]));
	}

	int calculateSickness() {
		// for each sheep calculate how sick it is. Then do avarage on  all sheeps.
		// a sheep starts with red 1.0, for each collision it loses some (0.3 ATM, could change)
		// so a max score is numOfSheeps * 100 (1000)

		float[] sickness = new float[10];
		int i;
		int sum = 0;


		for(i=0; i<10; i++){
            
			sickness[i] = sheeps[i].GetComponent<Renderer>().material.color.r * 100;
			sum += Convert.ToInt32(sickness[i]);
		}

		return sum;
	}

    void Update()
    {
        if (Input.GetKeyUp("enter") || Input.GetKeyUp("space"))
        {
            if(nextLevelName != "None")
                Application.LoadLevel(nextLevelName);
        }
            
    }
}
