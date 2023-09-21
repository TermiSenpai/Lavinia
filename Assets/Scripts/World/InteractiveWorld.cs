using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class InteractiveWorld : MonoBehaviour
{
    [SerializeField] private Tilemap m_Tilemap = null;
    [SerializeField] private Tilemap m_TilemapSelected = null;
    [SerializeField] private Camera m_Camera = null;
    [SerializeField] private TileBase selectedTile = null;

    [SerializeField] private List<TileData> tileDatas;
    private Dictionary<TileBase, TileData> dataFromTiles;

    private Vector3Int previousGridPos = new Vector3Int();

    /// <summary>
    /// Delay between moves used for debouncing
    /// </summary>
    public float m_Delay = 0.2f;

    //used for debouncing
    private float lastTime;

    //used for debouncing
    private float timeAccum;


    private void Awake()
    {
        dataFromTiles = new Dictionary<TileBase, TileData>();

        foreach(var tileData in tileDatas)
        {
            foreach(var tile in tileData.tiles)
            {
                dataFromTiles.Add(tile, tileData);
                
            }
        }
        Debug.Log(dataFromTiles.Values);
    }



    void Update()
    {

        Selected();
        
    }

    

    void Selected()
    {
        // +debounce
        timeAccum += Time.deltaTime;

        if (!Input.GetMouseButton(0))
            return;

        if (m_Delay < 0.2f)
            m_Delay = 0.2f;
        if (timeAccum < (lastTime + m_Delay))
            return;
        lastTime = timeAccum;

        // -debounce

        //get the mouse position
        var screenPos = Input.mousePosition;
        //test to ensure that it's within the visible area
        if (screenPos.x < 0 ||
            screenPos.y < 0 ||
            screenPos.x > Screen.width ||
            screenPos.y > Screen.height)
        {
            return;
        }

        var worldPos = m_Camera.ScreenToWorldPoint(screenPos);
        var gridPos = m_Tilemap.WorldToCell(worldPos);

        //test to ensure that it's within the visible area
        if (screenPos.x < 0 ||
            screenPos.y < 0 ||
            screenPos.x > Screen.width ||
            screenPos.y > Screen.height)
            return;

        if (!gridPos.Equals(previousGridPos))
        {
            var adjPos = gridPos + new Vector3Int(0, 0, 10);
            var adjPrev = previousGridPos + new Vector3Int(0, 0, 10);
            //Debug.Log("gridPos: " + adjPos);
            //Debug.Log("previousGridPos: " + adjPrev);
            m_TilemapSelected.SetTile(adjPrev, null); // Remove old tile
            m_TilemapSelected.SetTile(adjPos, selectedTile);
            previousGridPos = gridPos;


            //check type of tile
            //print();
            TileBase clickedOne = m_Tilemap.GetTile(adjPos);
            var type = dataFromTiles[clickedOne].Type;
            if (type == TileType.Construct)
            {
                if (!dataFromTiles[clickedOne].OcupedPos.ContainsKey(adjPos))
                {
                    dataFromTiles[clickedOne].OcupedPos.Add(adjPos, new GameObject());
                }
                    
                else
                    Debug.Log("Ocupado");
            }

            //Debug.Log(type);

        }






    }

}