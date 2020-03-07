using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;

public class CreatePacManMaze : MonoBehaviour
{
    enum TypeWall
    {
        Wall = 0,
        Empty = 1,
        Point = 2,
        Power = 3,
        Teleport = 4,
    };

    [SerializeField]
    static private GameObject[,] walls = null;
    [SerializeField]
    private Sprite[] sprites = null;
    [SerializeField]
    private Material generalMaterial = null;

    private static int collumns = 28;
    private static int rows = 31;
    private GameObject rootGameObject = null;
    private static readonly string pathSaveFile = "Assets/ifmamaif/Resources/PacMan/PacManMaze.load";
    private static readonly string pathToSprites = "PacMan/Textures";
    private static readonly string pathToGeneralMaterial = "PacMan/Materials/Text_Color";
    private int oldCollumns;
    private int oldRows;
    private Dictionary<int, (TypeWall,string)> mapSprites = new Dictionary<int, (TypeWall, string)>();
    private static Vector2 distanceBetweenWalls;

    // Start is called before the first frame update
    void Start()
    {
        object[] loadedSprites = Resources.LoadAll(pathToSprites, typeof(Sprite));
        sprites = new Sprite[loadedSprites.Length];
        loadedSprites.CopyTo(sprites, 0);

        generalMaterial = Resources.Load<Material>(pathToGeneralMaterial);
        
        oldCollumns = collumns;
        oldRows = rows;

        rootGameObject = new GameObject("PacMan-Maze");
        rootGameObject.transform.SetParent(this.gameObject.transform);
        rootGameObject.transform.localPosition = new Vector3(0, 0, 0);
        rootGameObject.transform.localScale = new Vector3(1, 1, 1);

        CreateMaze();

        distanceBetweenWalls = new Vector2(
           (walls[rows - 1, collumns - 1].transform.position.x - walls[0, 0].transform.position.x) / collumns,
            (walls[0, 0].transform.position.y - walls[rows - 1, collumns - 1].transform.position.y) / rows
           );

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
            writer.WriteLine("{0,2:D3} ,{1} ,{2}", elem.Key, (int)(elem.Value.Item1),elem.Value.Item2);
        }

        writer.WriteLine("{0} , {1}", rows, collumns);

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
        string[] line;

        for (int i = 0; i < numberOfSprites; i++)
        {
            line = reader.ReadLine().Split(',');
            mapSprites.Add(int.Parse(line[0]),((TypeWall)int.Parse(line[1]) ,line[2]));
        }

        line = reader.ReadLine().Split(',');
        int newRows = int.Parse(line[0]);
        int newCollumns = int.Parse(line[1]);

        if(newRows != rows || newCollumns != collumns)
        {
            collumns = newCollumns;
            rows = newRows;
            ReStart();
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
                SetCollision(gameObject);
                break;
            case TypeWall.Power:
                gameObject.tag = "Power";
                SetCollision(gameObject);
                break;
            case TypeWall.Wall:
                gameObject.tag = "Wall";
                SetCollision(gameObject);
                gameObject.GetComponent<SpriteRenderer>().material = generalMaterial;
                break;
            case TypeWall.Teleport:
                gameObject.tag = "Teleport";
                SetCollision(gameObject);
                break;
            default:
                gameObject.tag = "Empty";
                break;
        }
    }

    static void SetCollision(GameObject gameObject)
    {
        BoxCollider2D boxCollider = gameObject.AddComponent<BoxCollider2D>();
        boxCollider.isTrigger = true;
        boxCollider.size = new Vector2(0.5f, 0.5f);
        gameObject.AddComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
    }

    public static Vector2Int GetIndices(Vector2 position)
    {
        Vector2 indices = new Vector2 (
            (walls[0, 0].gameObject.transform.position.y - position.y) / distanceBetweenWalls.y,
            (position.x - walls[0, 0].gameObject.transform.position.x) / distanceBetweenWalls.x
            );


        //return new Vector2Int(Mathf.RoundToInt(indices.x), Mathf.RoundToInt(indices.y));
        return new Vector2Int((int)(indices.x), (int)(indices.y));
    }

    public static Transform GetNode(Vector2Int indices)
    {
        if (walls == null)
            return null;

        if (walls[indices.x, indices.y].gameObject.CompareTag("Wall"))
            return null;

        return walls[indices.x, indices.y].gameObject.transform;
    }
}
