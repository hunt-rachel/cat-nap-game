using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeManager : MonoBehaviour
{
    //TODO: make neater starting positions for shapes
    public Vector3Int[] startingShapePositions;

    public List<GameObject> playableShapes; //shapes that can be played from the start of the game
    public List<GameObject> hardShapes; //shapes that will be added to playable shapes once player reaches certain score

    public List<GameObject> currShapes;

    void Awake()
    {
        SetShapes();
    }

    void Update()
    {
        //if no more playable shapes on screen, instantiate three more for player
        if(currShapes.Count == 0)
        {
            SetShapes();
        }
    }

    private void SetShapes()
    {
        //get unique random indexes to select shapes for player
        List<int> randIndices = new List<int>();

        while(randIndices.Count < 3)
        {
            int rand = Random.Range(0, playableShapes.Count - 1);

            if(!randIndices.Contains(rand))
            {
                randIndices.Add(rand);
            }
        }
        
        for(int i = 0; i < 3; i++)
        {
            GameObject newShape = Instantiate(playableShapes[randIndices[i]]);

            newShape.GetComponent<Shape>().startPos = startingShapePositions[i];
            newShape.GetComponent<Shape>().SetShapeFeatures();

            currShapes.Add(newShape);
        }
    }
}
