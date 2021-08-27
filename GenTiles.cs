using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEditor;
using System;

public class GenTiles : MonoBehaviour
{
    public System.Random rand = new System.Random();
     
    public Tilemap cancer;

    private static int squareDim = 250;
    private static int apostleDim = 150;
    private static int finalMapSize = 4000;
    private static int numApostles = 16;

    public God god;
    public Map map;

    public List<Apostle> apostles;

    public void init() {
        CatanTiles tiles = GetComponent<CatanTiles>();

        // Debug.Log(tiles.get(3));
        map = new Map(squareDim,rand,cancer,tiles);
        god = new God(squareDim,map);

        landCreated = false;
    }

    public void EightApostles(){

        apostles = new List<Apostle>();
        ApostleSettings[] settings = new ApostleSettings[numApostles];

        for (int i = 0; i<settings.Length; i++) {
            settings[i] = new ApostleSettings(400);
        }

        // settings[0] = new ApostleSettings(400);
        // settings[1] = new ApostleSettings(400);
        // settings[2] = new ApostleSettings(400);
        // settings[3] = new ApostleSettings(400);
        // settings[4] = new ApostleSettings(400);
        // settings[5] = new ApostleSettings(400);
        // settings[6] = new ApostleSettings(400);
        // settings[7] = new ApostleSettings(400);

        List<(int, int)> spawnedPositions = new List<(int,int)>();

        mainMan = new Apostle(1,settings[0],map.get(squareDim/2,squareDim/2));
        apostles.Add(mainMan);
        spawnedPositions.Add((squareDim/2,squareDim/2));

        for(int i = 1; i < numApostles; i++) {
            (int, int) pos = ((100-apostleDim/2) + rand.Next(1,apostleDim+1),(100-apostleDim/2) + rand.Next(1,apostleDim+1));
            while (spawnedPositions.Contains(pos)) {
                pos = (85 + rand.Next(1,apostleDim+1), 85 + rand.Next(1,apostleDim+1));
            }
            apostles.Add(new Apostle(i+1, settings[i], map.get(pos.Item1, pos.Item2)));
            spawnedPositions.Add(pos);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        cancer.ClearAllTiles();
        init();
    }

    private int ticksPassed = 0;

    void Update() {
        ticksPassed++;
        if (ticksPassed == 1) {
            UpdateGame();
            ticksPassed = 0;
        }
    }

    bool landCreated;
    bool apostleUpdateFinished = true;
    bool apostlePartOneFinished = false;
    bool apostlePartTwoFinished = false;
    bool godSmoothingFinished = false;
    int currentFillDomain = 2;
    Apostle mainMan;
    int godTicks = 0;
    int maxGodTicks = 2;

    // Update is called once per frame
    void UpdateGame()
    {   
        // map.display();
        if(!landCreated) {
            if(god.CreateTheLand(0)) {
                landCreated = true;
                map = god.getMap();
                map.updateAdjacents();
                EightApostles();
            }
        } else {
            if(apostleUpdateFinished && !apostlePartOneFinished) {
                int doneCount = 0;
                apostleUpdateFinished = false;
                foreach(var cancerman in apostles){
                    if(cancerman.update(0)){doneCount++;}
                }
                apostleUpdateFinished = true;
                if(doneCount == apostles.Count) {apostlePartOneFinished = true;}
            } else if(!apostlePartTwoFinished) {
                if(mainMan.tiles.Count < finalMapSize) {
                    for(int i = currentFillDomain; i<numApostles; i++) {
                        if(!mainMan.update(i)) {
                            currentFillDomain = i;
                            break;
                        }
                    }
                } else {
                    apostlePartTwoFinished = true;
                    Debug.Log("done");
                    god.setMap(map);
                    god.total = 0;
                } 
            } else if(!godSmoothingFinished) {
                if(godTicks == maxGodTicks) {
                    godSmoothingFinished = true;
                    map = god.getMap();
                } else {
                    Debug.Log("god creating the land");
                    god.CreateTheLand(1);
                    godTicks++;
                }
            }
            map.display();
        }
    }

}
