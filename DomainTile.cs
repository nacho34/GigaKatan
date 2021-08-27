using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;
using UnityEditor;
using System;

public class DomainTile
{
    public int x;
    public int y;
    public int domain;
    public DomainTile[] adjacents = new DomainTile[4];
    private System.Random rand;
    public bool isReal;

    public DomainTile(int posx, int posy, bool isReal, System.Random rand){
        domain = -1;//<- VERY ASS CANCER
        x = posx;
        y = posy;
        this.isReal = isReal;
        this.rand = rand;
    }

    public DomainTile randomAdjacent(int dom) {
        List<DomainTile> validOptions = new List<DomainTile>();
        foreach (var tile in adjacents) {
            // Debug.Log(tile);
        }
        foreach (var tile in adjacents) {
            if(!tile.isReal) {continue;}
            if(tile.domain == dom) {validOptions.Add(tile);}
        }
        if(validOptions.Count == 0) {
            return null;
        }

        int roll = rand.Next(0,validOptions.Count);

        return validOptions[roll];
    }

    public DomainTile randomDifferent() {
        List<DomainTile> validOptions = new List<DomainTile>();
        foreach (var tile in adjacents) {
            // Debug.Log(tile);
        }
        foreach (var tile in adjacents) {
            if(!tile.isReal) {continue;}
            if(tile.domain != domain) {validOptions.Add(tile);}
        }
        if(validOptions.Count == 0) {
            return null;
        }

        int roll = rand.Next(0,validOptions.Count);

        return validOptions[roll];
    }

    public DomainTile getCopy() {
        DomainTile bullshit = new DomainTile(x,y,isReal,rand);
        bullshit.domain = domain;
        return bullshit;
    }

}