using System.Collections.Generic;
using UnityEngine;

/**
 * A generic implementation of the BFS algorithm.
 * @author Erel Segal-Halevi
 * @since 2020-02
 */
public class BFS {



    public static void FindPath<NodeType>(
            IGraph<NodeType> graph, 
            NodeType startNode, NodeType endNode, 
            List<NodeType> outputPath, int maxiterations=1000)
    {
        
        Dictionary<NodeType, NodeType> previous = new Dictionary<NodeType, NodeType>();
        Dictionary<NodeType, int> costToReachTile = new Dictionary<NodeType, int>();//מילון נוסף על מנת לשמור את השכן (מיקום האריח) והמחיר שלו
        PriorityQueue<NodeType> frontier = new PriorityQueue<NodeType>();//על מנת לתת משקלים PriorityQueue מבנה נתונים של
                                                                        //ולסדר לפי עדיפות
        frontier.Enqueue(startNode, 0);//ומחיר 0 startNode איתחול של תור עדיפויות עם
        costToReachTile[startNode] = 0;//כנ"ל גם למילון
        int i; 
        for (i = 0; i < maxiterations; ++i)
        {
        
            if(frontier.Count == 0)
                break;
            while (frontier.Count > 0)
            {
                NodeType curTile = frontier.Dequeue();
                if (curTile.Equals(endNode))
                {
                    outputPath.Add(endNode);
                    while (previous.ContainsKey(curTile))
                    {
                        curTile = previous[curTile];
                        outputPath.Add(curTile);
                    }
                    outputPath.Reverse();
                    break;
                }
                
            
                foreach (var neighbor in graph.Neighbors(curTile))
                {
                    int price = graph.cost(neighbor);//מחיר לעבור דרך השכן
                    int newCost = costToReachTile[curTile] + price;//המחיר שלי + המחיר של השכן
                    //Debug.Log("price: "+ x + " new price: " + newCost);
                    if (costToReachTile.ContainsKey(neighbor) == false || newCost < costToReachTile[neighbor])//בדיקה של אם השכן לא קיים במילון
                    {                                                                                         //או שהמחיר החדש קטן מהמחיר של השכן

                        costToReachTile[neighbor] = newCost;//המחיר של השכן משתנה למחיר החדש
                        int priority = newCost;
                        //Debug.Log("priority: "+ newCost);
                        frontier.Enqueue(neighbor, priority);//הוספה לתור עדיפיות שכן עם עדיפות חדשה
                        previous[neighbor] = curTile;//שומרים את האריח הנוכחי עם שכן חדש/ישן    
                    }
                }


            }



        }
    }

    public static List<NodeType> GetPath<NodeType>(IGraph<NodeType> graph, NodeType startNode, NodeType endNode, int maxiterations=1000) {
        List<NodeType> path = new List<NodeType>();
        FindPath(graph, startNode, endNode, path, maxiterations);
        return path;
    }

}