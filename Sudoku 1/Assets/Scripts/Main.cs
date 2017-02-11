﻿using System.Collections;
using System.Collections.Generic;
using System.IO;  
using UnityEngine;

public class Main : MonoBehaviour {

    public GameObject[] tiles;
	public GameObject tilePrefab;
	public int setCount;

    public GameObject[] enemySpaces;
    float spawnTimer;
    float spawnRate = 2f;
    public GameObject[] enemies;
    
    //timer to end game
    float timer = 500f;
    
    // week 2, ranged combat to end game
	
	
	

	// Use this for initialization
	void Start () {
		ReadLevel("board");
        enemySpaces = GameObject.FindGameObjectsWithTag("enemyspawn");
        tiles = GameObject.FindGameObjectsWithTag("tile");
        spawnTimer = Time.time;
    }

    // Update is called once per frame
    void Update () {
        if(timer < Time.time)
        {
            print("gameOver");
        }
		if(setCount == 81)
		{
			print("winner");
		}

        print(Time.time);
        print(spawnTimer + spawnRate);

        if(spawnTimer + spawnRate < Time.time)
        {
            spawnTimer = Time.time;
            //spawnRate /= 0.01;
            int index = Random.Range(0, 20);
            int enemyType = Random.Range(0, 2);
            Instantiate(enemies[enemyType], enemySpaces[index].transform.position, enemySpaces[index].transform.rotation);
        }
		
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
						SetTile(firstRead[0], int.Parse(firstRead[1].ToString()), coord[1], "permanent");						
						// TODO set non permanent tiles
					}
				}
				line = reader.ReadLine();
			}
		}
	}
	
	// Set the tile at Column, Row with a specific property
	// i.e. setTile('D', 4, 4, "permanent"); or something like that
	void SetTile(char col, int row, string val, string property) 
	{
		GameObject tile = Instantiate(tilePrefab);
		tile.GetComponent<TileMovement>().tileType = property;
		float x, y;
		switch(col)
		{
			case 'A': // Leftmost column - -2.08
				x = -2.08f;
				break;
			case 'B': 
				x = -1.57f;
				break;
			case 'C': 
				x = -1.05f;
				break;
			case 'D': 
				x = -0.52f;
				break;
			case 'E': // Center column - 0
				x = 0;
				break;
			case 'F': //0.52
				x = 0.52f;
				break;
			case 'G': //1.05
				x = 1.05f;
				break;
			case 'H': //1.57
				x = 1.57f;
				break;
			case 'I': // Rightmost column - 2.08
				x = 2.08f;
				break;
			default:
				x = 0;
				break;
		}
		switch(row) 
		{
			case 1: // Top row
				y = 4.46f;
				break;
			case 2:
				y = 3.94f;
				break;
			case 3:
				y = 3.43f;
				break;
			case 4:
				y = 2.89f;
				break;
			case 5:
				y = 2.38f;
				break;
			case 6:
				y = 1.86f;
				break;
			case 7:
				y = 1.34f;
				break;
			case 8:
				y = 0.82f;
				break;
			case 9: //Bottom row
				y = 0.31f;
				break;
			default:
				y = 0;
				break;
		}
		if(property == "permanent")
			setCount++;
		tile.transform.GetChild(0).transform.GetComponent<TextMesh>().text = val;
		tile.GetComponent<TileMovement>().value = int.Parse(val);
		tile.GetComponent<TileMovement>().col = col;
		tile.GetComponent<TileMovement>().row = row;
		tile.transform.position = new Vector3(x, y, 0);
	}
	

}