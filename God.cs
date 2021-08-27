
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEditor;
using System;

public class God {

    public int squareDim;
    public int max;
    public int total;
    private Map map;

    public God(int squareDim, Map map) {
        this.map = map;
        this.squareDim = squareDim;
        this.map.get(squareDim/2,squareDim/2).domain = 0;
        this.max = 25000;
        this.total = 1;
    }

    public bool CreateTheLand(int domainToExpand) {
        bool finished = false;
        Map updated = map.getCopy();

        //change tiles by randomadjacent
        for(int i = 0; i < squareDim+2; i++){
            // Debug.Log("outer loop");
            for(int j = 0; j < squareDim+2; j++){
                if(total == max) {
                    Debug.Log("finished");
                    finished = true;
                    break;
                }
                DomainTile current = map.get(i,j);
                if(current.domain == domainToExpand) {
                    DomainTile place = current.randomDifferent();
                    if(place != null) {
                        // Debug.Log(place.x);
                        updated.get(place.x,place.y).domain = domainToExpand;
                        total++;
                    }
                }
            }
            if(total == max) {
                break;
            }
        }

        map = updated;
        map.display();

        return finished;
    }

    public Map getMap() {return map;}

    public void setMap(Map map) {
        this.map = map;
    }

}