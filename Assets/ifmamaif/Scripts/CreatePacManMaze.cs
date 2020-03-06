using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;

public class CreatePacManMaze : MonoBehaviour
{
    public Sprite[] sprites;
    public Material generalMaterial;
    public int collumns = 28;
    public int rows = 31;

    private GameObject rootGameObject = null;
    [SerializeField]
    public GameObject[,] walls;

    enum TypeWall
    {
        Wall = 0,
        Empty = 1,
        Point = 2,
        Power = 3
    };

    private static readonly string saveFile = "PacManMaze.load";
    private static readonly string pathSaveFile = "Assets/ifmamaif/Resources/" + saveFile;
    private int oldCollumns;
    private int oldRows;
    private Dictionary<int, (TypeWall,string)> mapSprites = new Dictionary<int, (TypeWall, string)>();



    // Start is called before the first frame update
    void Start()
    {
        oldCollumns = collumns;
        oldRows = rows;

        rootGameObject = new GameObject("PacMan-Maze");
        rootGameObject.transform.SetParent(this.gameObject.transform);
        rootGameObject.transform.localPosition = new Vector3(0, 0, 0);
        rootGameObject.transform.localScale = new Vector3(1, 1, 1);

        CreateMaze();
        LoadScript();
    }

    void CreateGameObject(int i, int j,Sprite newSprite = null, TypeWall type = TypeWall.Empty)
    {
        GameObject gameObject = new GameObject(i + " " + j);
        SpriteRenderer spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
        if (newSprite != null)
            spriteRenderer.sprite = newSprite;

        gameObject.transform.SetParent(rootGameObject.transform);
        gameObject.transform.localPosition = new Vector3(1 + j, 30 - i, 0);// -0.1672395f);
        gameObject.transform.localScale = new Vector3(1, 1, 1);
        SetType(gameObject, type);

        walls[i, j] = gameObject;
    }

    void CreateMaze()
    {
        walls = new GameObject[rows, collumns];

        for (int i = 0; i < rows ; i++)
            for (int j = 0; j < collumns ; j++)
            {
                CreateGameObject(i, j, null);
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
        if (walls == null)
            return;

        StreamWriter writer = new StreamWriter(pathSaveFile);

        writer.WriteLine(mapSprites.Count);

        foreach (var elem in mapSprites)
        {
            writer.WriteLine("{0,2:D3} ,{1} ,{2}", elem.Key, elem.Value.Item1,elem.Value.Item2);
        }

        for (int i = 0; i < rows; i++)
        {
            writer.Write("{0,2:D3}", FindIDSprite(walls[i, 0].GetComponent<SpriteRenderer>().sprite.name));
            for (int j = 1; j < collumns; j++)
            {
                writer.Write(" , {0,2:D3}", FindIDSprite(walls[i, j].GetComponent<SpriteRenderer>().sprite.name));
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
        
        int numberOfSprites = int.Parse(reader.ReadLine());
        mapSprites.Clear();
        
        for (int i = 0; i < numberOfSprites; i++)
        {
            string[] line = reader.ReadLine().Split(',');
            mapSprites.Add(int.Parse(line[0]),((TypeWall)int.Parse(line[1]) ,line[2]));
        }

        string holeFileContent = reader.ReadToEnd();
        string[] lines = holeFileContent.Split('\n');
        for (int i=0;i<lines.Length-1;i++)
        {
            string[] cells = lines[i].Split(',');
            for(int j=0;j< cells.Length;j++)
            {
                int index = int.Parse(cells[j]);
                walls[i, j].GetComponent<SpriteRenderer>().sprite = GetSprite(mapSprites[index].Item2);
                SetType(walls[i, j], mapSprites[index].Item1);
                walls[i, j].SetActive(true);
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

    int FindIDSprite(string name)
    {
        foreach ( var elem in mapSprites)
        {
            if (elem.Value.Item2 == name)
            {
                return elem.Key;
            }
        }

        return 0;
    }

    void SetType(GameObject gameObject,TypeWall type)
    {
        switch (type)
        {
            case TypeWall.Empty:
                gameObject.tag = "Empty";
                break;
            case TypeWall.Point:
                gameObject.tag = "Point";
                break;
            case TypeWall.Power:
                gameObject.tag = "Power";
                break;
            case TypeWall.Wall:
                gameObject.tag = "Wall";
                break;
            default:
                gameObject.tag = "Empty";
                break;
        }
    }
}
