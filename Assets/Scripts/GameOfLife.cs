using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOfLife : MonoBehaviour

{
    [SerializeField]
    private Vector3Int _gridDimensions = new Vector3Int(10, 1, 10);
    private VoxelGrid _grid;
    // Start is called before the first frame update
    void Start()
    {
        //Creation of the Voxel Grid
        _grid = new VoxelGrid(_gridDimensions);

        //Go through each Voxel in the Grid 

        for (int x = 0; x < _gridDimensions.x; x++)
        {
            for (int y = 0; y < _gridDimensions.y; y++)
            {
                for (int z = 0; z < _gridDimensions.z; z++)
                {

                    //randomly togel voxels (below is individual logic)

                    Vector3Int voxelIndex = new Vector3Int(x, y, z); //xyz values for the current iteration the program is on 
                    Voxel currentVoxel = _grid.GetVoxelByIndex(voxelIndex); // establishing a voxel using prior coordinates
                    if (Random.value < 0.3f) //randomizer factor to establish if the voxel is alive or dead
                    {
                        currentVoxel.Alive = false; // setting voxels on or off
                    }
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        PerformRaycast();
        PerformGameOfLifeIteration();
    }

    private void PerformRaycast()
    {
        //when we click left mouse
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.tag == "Voxel")
                {
                    GameObject hitObject = hit.transform.gameObject;
                    var voxel = hitObject.GetComponent<VoxelTrigger>().AttachedVoxel;

                    //toggle alive or dead
                    voxel.Alive = !voxel.Alive;
                }
            }
        }

    }
    private void PerformGameOfLifeIteration()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            DoGameOfLifeIteration();
        }
    }

    private void DoGameOfLifeIteration()
    {
        //basic looking logic once again 
        for (int x = 0; x < _gridDimensions.x; x++)
        {
            for (int y = 0; y < _gridDimensions.y; y++)
            {
                for (int z = 0; z < _gridDimensions.z; z++)
                {
                    Vector3Int voxelIndex = new Vector3Int(x, y, z); //current coordinates
                    Voxel currentVoxel = _grid.GetVoxelByIndex(voxelIndex); //referencing a certain voxel via index

                    List <Voxel> neighbours = currentVoxel.GetNeighbourList();  //establishing a list of neighbors to our chosen voxel
                    int aliveCount = 0; //number of neighbours that are living
                    foreach (Voxel neighbour in neighbours)
                    {
                        if (neighbour.Alive)
                        {
                            aliveCount++; //adding to the previuosly made counter that is used for the Game of Life ruleset
                        }
                    }

                    if (currentVoxel.Alive)
                    {
                        if(aliveCount == 2 || aliveCount == 3)
                        {
                            currentVoxel.Alive = true; // the voxel is alive if it does not meet the requirments
                        } 
                        else
                        {
                            currentVoxel.Alive = false; // the voxel is dead if it does not meet the requirements 
                        }


                    }
                    else
                    {
                        if (aliveCount == 3)
                        {
                            currentVoxel.Alive = true; // this stage will bring back a voxel to life if it is surrounded by 3 neighbours
                        }
                    }
                }
            }
        }
    }
}


