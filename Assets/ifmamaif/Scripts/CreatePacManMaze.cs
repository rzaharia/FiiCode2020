using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;

public class CreatePacManMaze : MonoBehaviour
{
    public Sprite[] sprites;
    public Sprite doubleWallTop;
    public Sprite doubleWallLeft;
    public Sprite doubleWallRight;
    public Sprite doubleWallDown;
    public Sprite doubleCornerTopLeft;
    public Sprite doubleCornerTopRight;
    public Sprite doubleCornerBottomLeft;
    public Sprite doubleCornerBottomRight;
    public Sprite point;
    public Material generalMaterial;
    public int collumns = 28;
    public int rows = 31;


    private GameObject rootGameObject = null;
    [SerializeField]
    public GameObject[,] walls;

    private static readonly string saveFile = "PacManMaze.load";
    private static readonly string pathSaveFile = "Assets/ifmamaif/Resources/" + saveFile;
    private int oldCollumns;
    private int oldRows;

    // Start is called before the first frame update
    void Start()
    {
        oldCollumns = collumns;
        oldRows = rows;

        rootGameObject = new GameObject("PacMan-Maze");
        rootGameObject.transform.SetParent(this.gameObject.transform);
        rootGameObject.transform.localPosition = new Vector3(0, 0, 0);

        CreateMaze();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void CreateGameObject(int i, int j,Sprite newSprite)
    {
        GameObject gameObject = new GameObject(i + " " + j);
        SpriteRenderer spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = newSprite;

        gameObject.transform.SetParent(rootGameObject.transform);
        gameObject.transform.localPosition = new Vector3(1 + j, 30-i, -0.1672395f);

        walls[i, j] = gameObject;
    }

    void CreateMaze()
    {
        walls = new GameObject[rows, collumns];

        //Top
        for (int j = 1; j < collumns-1; j++)
        {
            CreateGameObject(0, j, doubleWallTop);
        }
        //Left
        for (int j = 1; j < rows - 1; j++)
        {
            CreateGameObject(j, 0, doubleWallLeft);
        }
        //Right
        for (int j = 1; j < rows-1; j++)
        {
            CreateGameObject(j,collumns-1, doubleWallRight );
        }
        //Bottom
        for (int j = 1; j < collumns-1; j++)
        {
            CreateGameObject(rows - 1, j, doubleWallDown);
        }

        CreateGameObject(0, 0, doubleCornerTopLeft);
        CreateGameObject(rows - 1, 0, doubleCornerBottomLeft);
        CreateGameObject(0, collumns - 1, doubleCornerTopRight);
        CreateGameObject(rows - 1, collumns - 1, doubleCornerBottomRight);


        // Create the points
        for (int i = 1; i < rows - 1; i++)
            for (int j = 1; j < collumns - 1; j++)
            {
                CreateGameObject(i, j, point);
            }
    }


    public void ReStart()
    {
        if (rootGameObject == null)
        {
            rootGameObject = new GameObject("PacMan-Maze");
            rootGameObject.transform.SetParent(this.gameObject.transform);
            rootGameObject.transform.localPosition = new Vector3(0, 0, 0);
        }

        if (walls != null)
        {
            for (int i = 0; i < oldRows; i++)
                for (int j = 0; j < oldCollumns; j++)
                {
                    Destroy(walls[i, j]);
                }
        }

        CreateMaze();

        oldCollumns = collumns;
        oldRows = rows;
    }

    public void SaveScript()
    {
        StreamWriter writer = new StreamWriter(pathSaveFile);

        for (int i = 0; i < rows ; i++)
        {
            writer.Write(walls[i, 0].GetComponent<SpriteRenderer>().sprite.name);
            for (int j = 1; j < collumns; j++)
            {
                writer.Write(","+walls[i,j].GetComponent<SpriteRenderer>().sprite.name);
            }
            writer.Write("\n");
        }
        writer.Close();
    }

    public void LoadScript()
    {
        if (walls == null)
            return;

        StreamReader reader = new StreamReader(pathSaveFile);
        string holeFileContent = reader.ReadToEnd();
        string[] lines = holeFileContent.Split('\n');
        for (int i=0;i<lines.Length-1;i++)
        {
            string[] cells = lines[i].Split(',');
            for(int j=0;j< cells.Length;j++)
            {
                walls[i,j].GetComponent<SpriteRenderer>().sprite = GetSprite(cells[j]);
            }
        }

        reader.Close();
    }

    Sprite GetSprite(string name)
    {
        foreach(Sprite sprite in sprites)
        {
            if(sprite.name == name)
            {
                return sprite;
            }
        }

        return sprites[0];
    }
}
