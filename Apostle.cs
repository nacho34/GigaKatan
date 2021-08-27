using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Apostle
{
    public int domain;
    public ApostleSettings tileCap;
    public DomainTile pos;
    public List<DomainTile> tiles = new List<DomainTile>();

    public Apostle (int domain, ApostleSettings tileCap, DomainTile pos){
        this.domain = domain;
        this.tileCap = tileCap;
        this.pos = pos;
        this.pos.domain = domain;
        tiles.Add(pos);
    }

    List<int> illegalExpands = new List<int>();

    //returns whether has no space left to expand
    public bool update(int expansionDomain) {
        if(illegalExpands.Contains(expansionDomain)) {return true;}
        int nextIdx = tiles.Count-1;
        while(nextIdx != -1){
            pos = tiles[nextIdx];
            DomainTile nextTile = pos.randomAdjacent(expansionDomain);
            if(nextTile != null) {
                // Debug.Log("found tile next to " + pos.x + ", " + pos.y + " at: " + nextTile.x + ", " + nextTile.y);
                nextTile.domain = this.domain;
                pos = nextTile;
                tiles.Add(pos);
                return false;
            } else {
                // Debug.Log("found no tile next to " + pos.x + ", " + pos.y);
                nextIdx = tiles.IndexOf(pos)-1;
            }
        }

        illegalExpands.Add(expansionDomain);
        return true;

        // DomainTile nextTile = pos.randomAdjacent();
        // if(nextTile != null) {
        //     nextTile.domain = this.domain;
        //     pos = nextTile;
        //     tiles.Add(pos);
        //     return true;
        // } else {
        //     int nextIdx = tiles.IndexOf(pos)-1;
        //     if(nextIdx == -1) {return false;}
        //     pos = tiles[nextIdx];
        //     //update();
        //     return false;
        // }
    }
}
