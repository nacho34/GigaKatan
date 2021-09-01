using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;

public class GameLoop : MonoBehaviour
{
    public static GameState[] nonDisplayStates = {GameState.GOD, GameState.IDLE};
    public static GameState[] mapGenStates = {GameState.SUMMONING,GameState.CONSUMING,GameState.GODSMOOTHING, GameState.APOSTLES2};

    public GenTiles genTiles;
    GameState gameState;
    public Map map;
    CatanTiles tiles;
    public System.Random rand = new System.Random();
    public Tilemap cancer;

    // Start is called before the first frame update
    void Start()
    {   
        gameState = GameState.INIT;
        tiles = GetComponent<CatanTiles>();
        map = new Map(Constants.squareDim,rand,cancer,tiles);
        genTiles = new GenTiles(map,rand);
        map.clear();
        
    }

    private int ticksPassed = 0;

    void Update() {
        ticksPassed++;
        if (ticksPassed == 1) {
            UpdateGame();
            ticksPassed = 0;
        }
    }

    void UpdateGame() {
        //Debug.Log("gamestate: " + gameState);

        if(!containsCurrentState(nonDisplayStates)) {map.displayChanges();}

        switch (gameState) {
            case GameState.INIT:
                genTiles.initGod();
                gameState = GameState.GOD; break;
            case GameState.GOD:
                stateChange(genTiles.initialGod(),GameState.SUMMONING);break;
            case GameState.SUMMONING:
                stateChange(genTiles.summon(),GameState.CONSUMING);break;
            case GameState.CONSUMING:
                stateChange(genTiles.consume(),GameState.GODSMOOTHING);break;
            case GameState.GODSMOOTHING:
                stateChange(genTiles.godSmoothing(),GameState.APOSTLES2);break;
            case GameState.APOSTLES2:
                stateChange(genTiles.dahdiowdjiqode(),GameState.IDLE);break;
            case GameState.IDLE:
                Debug.Log("done");
                break;
        }

        if(containsCurrentState(mapGenStates)) {map = genTiles.map;}

    
    }

    bool containsCurrentState(GameState[] states) {
        return Array.Exists(states, state => state == gameState);
    }

    void stateChange(bool func, GameState done) {
        if(func) {gameState = done;}
    }

}
