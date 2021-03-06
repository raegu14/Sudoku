﻿using System.Collections;
using System.Collections.Generic;
using System.IO;  
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour {

    public GameObject[] tiles;
	public GameObject tilePrefab;

    public GameObject[] enemySpaces;
    public GameObject[] enemies;
    float spawnTimer;
    float spawnRate = 2f;

    string gameStatus;

    //timer to end game
    float timer = 500f;

    float currentTime;

    public GameObject controls;
	public AudioClip win;
	public AudioClip lose;
    private bool isActiveControl = true;
	private bool swapped = false;
	
	// Important objects to keep track of
	private Board board;
	private TileSpawn spawnner;
	public int switchTimer = 50;

    //zoom camera out
    int curIteration = 0;
    int finalIteration = 10;
    Camera cam;

    // Use this for initialization
    void Start () {
        currentTime = Time.time;
        isActiveControl = false;
        controls.SetActive(isActiveControl);
        enemySpaces = GameObject.FindGameObjectsWithTag("enemyspawn");
        tiles = GameObject.FindGameObjectsWithTag("tile");
        spawnTimer = Time.time;
		board = GameObject.Find("Board").GetComponent<Board>();
		spawnner = GameObject.Find("TileSpawnPoints").GetComponent<TileSpawn>();
        ReadLevel("board");
        gameStatus = "before";
        cam = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update () {
        if(Time.time - currentTime > 5f && gameStatus == "before")
        {
            gameStatus = "Pregame";
        }
        if(gameStatus == "Pregame")
        {
            if(curIteration <= finalIteration + 100)
            {
                curIteration++;
            }

            if (curIteration <= finalIteration)
            {
                float t = (float)curIteration / (float)finalIteration;
                cam.GetComponent<Camera>().orthographicSize = Mathf.Lerp(3f, 5.4f, t);
                cam.transform.position = new Vector3(0f, Mathf.Lerp(1.8f, 0, t), -1f);
            }

            if (curIteration == finalIteration + 20)
            {
                isActiveControl = true;
                controls.SetActive(isActiveControl);
            }

            if (curIteration == finalIteration + 100)
            {
                isActiveControl = false;
                controls.SetActive(isActiveControl);
            }
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
        {
            isActiveControl = !isActiveControl;
            controls.SetActive(isActiveControl);
        }

        if (gameStatus == "GameOver")
		{
			if(!swapped)
				switchMusic("lose");
			switchTimer--;
			if(Input.anyKeyDown && switchTimer < 0)
				SceneManager.LoadScene("Menu");
		}
		else if(gameStatus == "Winner")
		{
			if(!swapped)
				switchMusic("win");
			switchTimer--;
			if(Input.anyKeyDown && switchTimer < 0)
				SceneManager.LoadScene("Menu");
		}
        tiles = GameObject.FindGameObjectsWithTag("tile");		
	}
	void ReadLevel(string fileName) 
	{
		TextAsset levelData = (TextAsset)Resources.Load(fileName, typeof(TextAsset));
		StringReader reader = new StringReader(levelData.text);
		if(reader == null)
		{
			Debug.Log("Can't find or read level data.");
		}
		else
		{
			string line = reader.ReadLine();
			while(line != null)
			{
				string[] coord = line.Split(':');
				if (coord.Length > 0)
				{
					string firstRead = coord[0];
					if(firstRead == "ID") //ID 
					{
						// DO SOMETHING WITH THE ID VALUE
					}
					else if(firstRead == "Difficulty") // Difficulty
					{
						// DO SOMETHING WITH THE DIFFICULTY VALUE
					}
					else // Assumed to be actual coordinates
					{
						int val = int.Parse(coord[1]);
						int row = int.Parse(firstRead[1].ToString());
						char col = firstRead[0];
						board.SetSolution(col, row, val);
						if(coord[2] == "P")
						{
							spawnner.SpawnTile(col, row, val);
						}
						
					}
				}
				line = reader.ReadLine();
			}
		}		
	}
	
	
    public string getGameStatus()
    {
        return gameStatus;
    }
	
	public void setGameStatus(string gamestatus)
	{
		gameStatus = gamestatus;
	}
	
	public void switchMusic(string state)
	{
		AudioSource aud = gameObject.GetComponent<AudioSource>();
		GameObject.Find("BAM").GetComponent<SpriteRenderer>().enabled = true;
		if(state == "win")
		{
			aud.clip = win;
		}
		else // you lost
		{
			aud.clip = lose;
		}
		aud.Play();
		swapped = true;
	}

}
