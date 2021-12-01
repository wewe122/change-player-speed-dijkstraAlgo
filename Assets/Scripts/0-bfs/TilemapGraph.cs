using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;

/**
 * A graph that represents a tilemap, using only the allowed tiles.
 */
public class TilemapGraph: IGraph<Vector3Int> {
    private Tilemap tilemap;
    private TileBase[] allowedTiles;

    enum TileType//allowedTiles שמכיל את האינדקסים של enum מערך של 
    {            //enum צריך גם להוסיף אותו למערך של allowedTiles אם מוסיפים אריח למערך של
        bushes ,
        grass,
        hills,
        swamp
    }

    public TilemapGraph(Tilemap tilemap, TileBase[] allowedTiles) {
        this.tilemap = tilemap;
        this.allowedTiles = allowedTiles;
    }



    static Vector3Int[] directions = {
            new Vector3Int(-1, 0, 0),
            new Vector3Int(1, 0, 0),
            new Vector3Int(0, -1, 0),
            new Vector3Int(0, 1, 0),
    };

    public IEnumerable<Vector3Int> Neighbors(Vector3Int node) {
        foreach (var direction in directions) {
            Vector3Int neighborPos = node + direction;
            TileBase neighborTile = tilemap.GetTile(neighborPos);
            if (allowedTiles.Contains(neighborTile))
                yield return neighborPos;
        }
    }



    public int cost(Vector3Int neighbornode)//עבור כל שכן הפונקציה מחזירה את המחיר
    {
            TileBase neighborTile = tilemap.GetTile(neighbornode);
           
            if (allowedTiles.Contains(neighborTile))
            {

                if(allowedTiles[(int)TileType.bushes].Equals(neighborTile))//bushes עלות של 10 עבור אריח
                {
                    
                    //Debug.Log("in bushes " + (int)TileType.bushes);
                    return 10;
                }
                     
                if(allowedTiles[(int)TileType.grass].Equals(neighborTile))//grass עלות של 5 עבור אריח
                {
                    //Debug.Log("in grass " + (int)TileType.grass);
                    return 5;
                }
                if(allowedTiles[(int)TileType.hills].Equals(neighborTile))//hills עלות של 60 עבור אריח
                {
                    //Debug.Log("in hills " + (int)TileType.hills);
                    return 60;
                }
                if(allowedTiles[(int)TileType.swamp].Equals(neighborTile))//swamp עלות של 1 עבור אריח
                {
                    //Debug.Log("in swamp " + (int)TileType.swamp);
                    return 1;
                }
            }
                
        
         return 0;
    }
}
