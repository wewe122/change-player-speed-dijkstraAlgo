using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

/**
 * This component moves its object towards a given target position.
 */
public class TargetMover: MonoBehaviour {
    [SerializeField] Tilemap tilemap = null;
    [SerializeField] AllowedTiles allowedTiles = null;

    [Tooltip("The speed by which the object moves towards the target, in meters (=grid units) per second")]
    [SerializeField] float speed = 2f;

    [Tooltip("Maximum number of iterations before BFS algorithm gives up on finding a path")]
    [SerializeField] int maxIterations = 1000;

    [Tooltip("The target position in world coordinates")]
    [SerializeField] Vector3 targetInWorld;

    [Tooltip("The target position in grid coordinates")]
    [SerializeField] Vector3Int targetInGrid;

    enum TileType
    {            
        bushes ,
        grass,
        hills,
        swamp
    }


    protected bool atTarget;  // This property is set to "true" whenever the object has already found the target.

    public void SetTarget(Vector3 newTarget) {
        if (targetInWorld != newTarget) {
            targetInWorld = newTarget;
            targetInGrid = tilemap.WorldToCell(targetInWorld);
            atTarget = false;
        }
    }

    public Vector3 GetTarget() {
        return targetInWorld;
    }

    private TilemapGraph tilemapGraph = null;
    private float timeBetweenSteps =0;

    protected virtual void Start() {
        tilemapGraph = new TilemapGraph(tilemap, allowedTiles.Get());
        timeBetweenSteps = 1 / speed;
        
        StartCoroutine(MoveTowardsTheTarget());
    }

    IEnumerator MoveTowardsTheTarget() {
        for(;;) {
            yield return new WaitForSeconds(timeBetweenSteps);
            
            if (enabled && !atTarget)
            {
                
                MakeOneStepTowardsTheTarget();
            }
                
        }
    }

    private void MakeOneStepTowardsTheTarget() {
        
        Vector3Int startNode = tilemap.WorldToCell(transform.position);
        Vector3Int endNode = targetInGrid;
        List<Vector3Int> shortestPath = BFS.GetPath(tilemapGraph, startNode, endNode, maxIterations);
        
        Debug.Log("shortestPath = " + string.Join(" , ",shortestPath));
        if (shortestPath.Count >= 2) {
            Vector3Int nextNode = shortestPath[1];
            TileBase neighborTile = tilemap.GetTile(nextNode);
            if (allowedTiles.Get().Contains(neighborTile))// int cost שינוי מהירות עבור המשבצת (אריח) הבא בהתאם למשקלים (מחיר) שנתנו בפונקצית 
            {                                              //בשביל שיראה הגיוני
                 if(allowedTiles.Get()[(int)TileType.bushes].Equals(neighborTile))
                {
                
                    //Debug.Log("in bushes " + (int)TileType.bushes);
                    timeBetweenSteps = 3 / speed;
                }
                     
                if(allowedTiles.Get()[(int)TileType.grass].Equals(neighborTile))
                {
                    //Debug.Log("in grass " + (int)TileType.grass);
                    timeBetweenSteps = 5 / speed;
                }
                if(allowedTiles.Get()[(int)TileType.hills].Equals(neighborTile))//המהירות היא הכי איטית hills עלות של 60 עבור אריח
                {
                    //Debug.Log("in hills " + (int)TileType.hills);
                    timeBetweenSteps = 7 / speed;
                }
                if(allowedTiles.Get()[(int)TileType.swamp].Equals(neighborTile))//המהירות היא הכי מהירה swamp עלות של 1 עבור אריח
                {
                    //Debug.Log("in swamp " + (int)TileType.swamp);
                    timeBetweenSteps = 1 / speed;
                }
            }
            transform.position = tilemap.GetCellCenterWorld(nextNode);
        } else {
            atTarget = true;
        }
    }
}
