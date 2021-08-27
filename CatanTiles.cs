
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEditor;
using System;

public class CatanTiles : MonoBehaviour {

    public Tile domainminus2;
    public Tile domainminus1;
    public Tile domain0;
    public Tile domain1;
    public Tile domain2;
    public Tile domain3;
    public Tile domain4;
    public Tile domain5;
    public Tile domain6;
    public Tile domain7;
    public Tile domain8;

    public List<Tile> tiles = new List<Tile>();
    public List<Tile> ntiles = new List<Tile>();


    public Tile get(int domain) {
        if(domain < 0){
            return ntiles[-1*domain-1];
        } else if(domain > 8) {
            return tiles[domain % 7 + 2];
        }
        return tiles[domain];
        // return domain0;
    }

    void Start() {
        foreach (var tile in tiles) {
            Debug.Log(tile);
        }

        ntiles.Add(domainminus2);
        ntiles.Add(domainminus1);
        tiles.Add(domain0);
        tiles.Add(domain1);
        tiles.Add(domain2);
        tiles.Add(domain3);
        tiles.Add(domain4);
        tiles.Add(domain5);
        tiles.Add(domain6);
        tiles.Add(domain7);
        tiles.Add(domain8);

        foreach (var tile in tiles) {
            Debug.Log(tile);
        }

        for(int i = 0; i < 9; i++) {
            tiles.Remove(null);
        }

        foreach (var tile in tiles) {
            Debug.Log(tile);
        }
    }

}