using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeManager : MonoBehaviour
{
    //TODO: make neater starting positions for shapes
    public Vector3Int[] startingShapePositions;

    public List<GameObject> playableShapes; //shapes that can be played from the start of the game
    public List<GameObject> hardPlayableShapes; //shapes that will be added to playable shapes once player reaches certain score

    public List<GameObject> currPlayableShapes;

    public List<GameObject> emptySpaces; //shapes that are the negative space the players must try and make borders around
    public List<GameObject> hardEmptySpaces; //shapes that will be added to empty spaces once player reaches a certain score

    public GameObject currEmptySpace; //the current negative space the player must try and make a border around
    public Vector3Int emptySpaceDisplayPos; //where to display the visualisation of the shape the player needs to make 

    void Awake()
    {
        SetShapes();
    }

    void Update()
    {
        //if no more playable shapes on screen, instantiate three more for player
        if(currPlayableShapes.Count == 0)
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

            currPlayableShapes.Add(newShape);
        }
    }

    private void SetEmptySpace()
    {
        currEmptySpace = emptySpaces[Random.Range(0, emptySpaces.Count - 1)];

        Instantiate(currEmptySpace);

        //TODO: make game object instance for display, and set position

    }
}
