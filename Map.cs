
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEditor;
using System;

public class Map {

    private DomainTile[,] bigMan;
    public int squareDim;
    public System.Random rand;
    public Tilemap cancer;
    public CatanTiles tiles;

    public Map(int squareDim,System.Random rand,Tilemap cancer, CatanTiles tiles) {

        this.tiles = tiles;
        this.cancer = cancer;
        this.rand = rand;
        this.squareDim = squareDim;
        bigMan = new DomainTile[squareDim+2,squareDim+2];

        for(int i = 0; i < squareDim+2; i++){
            for(int j = 0; j < squareDim+2; j++){
                if((j == 0 || j ==squareDim+1) || (i == 0 || i == squareDim+1)) {
                    bigMan[i,j] = new DomainTile(i,j, false, rand);
                } else {
                    bigMan[i,j] = new DomainTile(i,j, true, rand);
                }
            }
        }

        updateAdjacents();
    }

    public void updateAdjacents() {
        for(int i = 1; i < squareDim+1; i++){
            for(int j = 1; j < squareDim+1; j++){
                bigMan[i,j].adjacents[0] = bigMan[i-1,j];
                bigMan[i,j].adjacents[1] = bigMan[i+1,j];
                bigMan[i,j].adjacents[2] = bigMan[i,j-1];
                bigMan[i,j].adjacents[3] = bigMan[i,j+1];
            }
        }
    }

    public Map getCopy() {
        Map cop = new Map(squareDim,rand,cancer,tiles);
        for(int i = 0; i < squareDim+2; i++){
            for(int j = 0; j < squareDim+2; j++){
                // Debug.Log(bigMan[i,j].getCopy());
                cop.set(bigMan[i,j].getCopy());
            }
        }
        updateAdjacents();
        return cop;
    }

    public void set(DomainTile tile) {
        // Debug.Log(tile.x);
        bigMan[tile.x,tile.y] = tile;
    }

    public DomainTile get(int x, int y) {return bigMan[x,y];}
    
    public void display() {
        for(int i = 1; i < squareDim+1; i++){
            for(int j = 1; j < squareDim+1; j++){
                int dom = bigMan[i,j].domain;
                // if(dom == 1) {
                    cancer.SetTile(new Vector3Int(i,j,0), tiles.get(dom));
                // }
            }
        }
    }
}