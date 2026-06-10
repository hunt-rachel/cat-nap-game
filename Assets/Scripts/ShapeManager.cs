using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShapeManager : MonoBehaviour
{
    //TODO: make neater starting positions for shapes
    public Vector3Int[] startingShapePositions;
    [Space] //added spaces to help visually in inspector
    [Space]

    public List<GameObject> playableShapes; //shapes that can be played from the start of the game
    public List<GameObject> hardPlayableShapes; //shapes that will be added to playable shapes once player reaches certain score
    public GameObject playableShapesHolder;
    [Space]
    [Space]

    public List<GameObject> currPlayableShapes;
    [Space]
    [Space]

    public List<GameObject> emptySpaces; //shapes that are the negative space the players must try and make borders around
    public List<GameObject> mediumEmptySpaces; //shapes that will be added to empty spaces once player reaches a certain score
    public List<GameObject> hardEmptySpaces; //shapes that will be added to empty spaces once player reaches a certain, higher score
    public GameObject emptySpaceHolder;
    [Space]
    [Space]

    public GameObject currEmptySpace; //the current negative space the player must try and make a border around
    public Vector3Int emptySpaceDisplayPos; //where to display the visualisation of the shape the player needs to make 

    void Update()
    {
        //if no more playable shapes on screen, instantiate three more for player
        if(currPlayableShapes.Count == 0)
        {
            SetShapes();
        }

        //TODO: set new empty space if current one made

        //TODO: MAKE CODE FOR CHECKING IF SHAPE CAN BE PLACED     
    }

    public void SetShapes()
    {
        //clear current playable shapes (if any) to allow for new ones
        foreach(Transform child in playableShapesHolder.transform)
        {
            Destroy(child.gameObject);
        }
        
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
            GameObject newShape = Instantiate(playableShapes[randIndices[i]], playableShapesHolder.transform);

            newShape.GetComponent<Shape>().startPos = startingShapePositions[i];
            newShape.GetComponent<Shape>().SetShapeFeatures();

            currPlayableShapes.Add(newShape);
        }
    }

    public void SetEmptySpace()
    {
        //clear current empty space (if any) to allow for new one
        //clear current playable shapes (if any) to allow for new ones
        foreach (Transform child in emptySpaceHolder.transform)
        {
            Destroy(child.gameObject);
        }

        currEmptySpace = emptySpaces[Random.Range(0, emptySpaces.Count - 1)];

        GameObject emptyToMake = Instantiate(currEmptySpace, emptySpaceHolder.transform);
        emptyToMake.transform.position = emptySpaceDisplayPos;

    }
}
