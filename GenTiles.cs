using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEditor;
using System;

public class GenTiles
{

    public int godsDomain = -2;
    public int mainDomain = 0;
    public System.Random rand;

    private static int apostleDim = 150;
    private static int finalMapSize = 4000;
    private static int numApostles = 16;
    private static int numApostles2 = 8;

    public God god;
    public Map map;

    public List<Apostle> apostles;
    public List<Apostle> apostles2;

    public GenTiles(Map map, System.Random rand) {
        this.map = map;
        this.rand = rand;
    }

    public void EightApostles(){

        apostles = new List<Apostle>();
        List<(int, int)> spawnedPositions = new List<(int,int)>();

        mainMan = new Apostle(mainDomain,map.get(Constants.squareDim/2,Constants.squareDim/2));
        apostles.Add(mainMan);
        spawnedPositions.Add((Constants.squareDim/2,Constants.squareDim/2));

        for(int i = 1; i < numApostles; i++) {
            (int, int) pos = ((100-apostleDim/2) + rand.Next(1,apostleDim+1),(100-apostleDim/2) + rand.Next(1,apostleDim+1));
            while (spawnedPositions.Contains(pos)) {
                pos = (85 + rand.Next(1,apostleDim+1), 85 + rand.Next(1,apostleDim+1));
            }
            apostles.Add(new Apostle(i+9, map.get(pos.Item1, pos.Item2)));
            spawnedPositions.Add(pos);
        }
    }

    public void initGod() {
        map.get(Constants.squareDim/2,Constants.squareDim/2).setDomain(godsDomain);
        god = new God(map);
    }

    //apostles2 contains domains 1-8
    public void EightApostles2(){

        map.killAllOtherTiles(0);

        apostles2 = new List<Apostle>();
        ApostleSettings[] settings = new ApostleSettings[numApostles2];

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

        for(int i = 1; i < numApostles2+1; i++) {
            DomainTile currentTile = apostles[mainDomain].tiles[rand.Next(0, apostles[mainDomain].tiles.Count)];
            (int, int) pos = (currentTile.x, currentTile.y);
            while (spawnedPositions.Contains(pos)) {
                currentTile = apostles[mainDomain].tiles[rand.Next(0, apostles[mainDomain].tiles.Count)];
                pos = (currentTile.x, currentTile.y);
            }
            apostles2.Add(new Apostle(i, settings[i-1], map.get(pos.Item1, pos.Item2)));
            spawnedPositions.Add(pos);
        }
    }

    
    int currentFillDomain = 10;
    Apostle mainMan;
    int godTicks = 0;
    int maxGodTicks = 2;

    public bool initialGod() {
        // Debug.Log("god: " + god);
        if(god.CreateTheLand(godsDomain)) {
            map = god.getMap();
            map.updateAdjacents();
            EightApostles();
            return true;
        }
        return false;
    }

    public bool summon(){
        int doneCount = 0;
        foreach(var cancerman in apostles){
            if(cancerman.update(godsDomain)) {doneCount++;}
        }
        if(doneCount == apostles.Count) {return true;}
        return false;
    }

    public bool consume() {
        if(mainMan.tiles.Count < finalMapSize) {
            for(int i = currentFillDomain; i<numApostles+9; i++) {
                if(!mainMan.update(i)) {
                    currentFillDomain = i;
                    break;
                }
                if(currentFillDomain == numApostles + 9 - 1){
                    currentFillDomain = 10;
                }
            }
        } else {
            Debug.Log("done");
            god.setMap(map);
            god.total = 0;
            return true;
        }
        return false;
    }

    public bool godSmoothing() {
        if(godTicks == maxGodTicks) {
            map = god.getMap();
            map.updateAdjacents();
            EightApostles2();
            return true;
        }
        Debug.Log("god creating the land");
        god.CreateTheLand(mainDomain);
        godTicks++;
        return false;
    }

    public bool dahdiowdjiqode(){
        int doneCount = 0;
        foreach(var cancerman in apostles2){
            if(cancerman.update(mainDomain)) {doneCount++;}
        }
        if(doneCount == apostles2.Count) {return true;}
        return false;
    }

}
