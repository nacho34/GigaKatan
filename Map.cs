
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

    public List<DomainTile> recentChanges;

    public Map(int squareDim,System.Random rand,Tilemap cancer, CatanTiles tiles) {
        this.tiles = tiles;
        this.cancer = cancer;
        this.rand = rand;
        this.squareDim = squareDim;
        bigMan = new DomainTile[squareDim+2,squareDim+2];

        recentChanges = new List<DomainTile>();

        for(int i = 0; i < squareDim+2; i++){
            for(int j = 0; j < squareDim+2; j++){
                if((j == 0 || j ==squareDim+1) || (i == 0 || i == squareDim+1)) {
                    bigMan[i,j] = new DomainTile(i,j, false, rand);
                } else {
                    bigMan[i,j] = new DomainTile(i,j, true, rand);
                }
                bigMan[i,j].domainChange += tileDomainChanged;
            }
        }

        updateAdjacents();
    }

    public void clear() {
        cancer.ClearAllTiles();
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
        recentChanges.Add(tile);
    }


    public DomainTile get(int x, int y) {return bigMan[x,y];}

    public void killAllOtherTiles(int dom){
        for(int i = 0; i < Constants.squareDim; i++){
            for(int j = 0; j < Constants.squareDim; j++){
                if(bigMan[i,j].getDomain() != dom){
                    bigMan[i,j].setDomain(-1);
                }
            }
        }
    }
    
    public void display() {
        for(int i = 1; i < squareDim+1; i++){
            for(int j = 1; j < squareDim+1; j++){
                int dom = bigMan[i,j].getDomain();
                // if(dom == 0) {
                    cancer.SetTile(new Vector3Int(i,j,0), tiles.get(dom));
                // }
            }
        }
    }
    
    public void displayChanges(){
        foreach(var cancerman in recentChanges){
            cancer.SetTile(new Vector3Int(cancerman.x, cancerman.y, 0), tiles.get(cancerman.getDomain()));
        }
        recentChanges.Clear();
    }

    public void tileDomainChanged(System.Object sender,EventArgs e) {
        DomainTile realSender = (DomainTile) sender;
        recentChanges.Add(realSender);
        Debug.Log("updating tile");
        realSender.domainChange += tileDomainChanged;
    }
}