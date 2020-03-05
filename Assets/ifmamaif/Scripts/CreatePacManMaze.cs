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

    private static readonly string saveFile = "PacManMaze.load";
    private static readonly string pathSaveFile = "Assets/ifmamaif/Resources/" + saveFile;
    private int oldCollumns;
    private int oldRows;
    private Dictionary<int, string> mapSprites = new Dictionary<int, string>();

    // Start is called before the first frame update
    void Start()
    {
        oldCollumns = collumns;
        oldRows = rows;

        rootGameObject = new GameObject("PacMan-Maze");
        rootGameObject.transform.SetParent(this.gameObject.transform);
        rootGameObject.transform.localPosition = new Vector3(0, 0, 0);

        CreateMaze();
        LoadScript();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void CreateGameObject(int i, int j,Sprite newSprite = null)
    {
        GameObject gameObject = new GameObject(i + " " + j);
        SpriteRenderer spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
        if (newSprite != null)
            spriteRenderer.sprite = newSprite;

        gameObject.transform.SetParent(rootGameObject.transform);
        gameObject.transform.localPosition = new Vector3(1 + j, 30-i, -0.1672395f);

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
            writer.WriteLine("{0,2:D3} ,{1}", elem.Key, elem.Value);
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
            mapSprites.Add(int.Parse(line[0]),line[1]);
        }

        string holeFileContent = reader.ReadToEnd();
        string[] lines = holeFileContent.Split('\n');
        for (int i=0;i<lines.Length-1;i++)
        {
            string[] cells = lines[i].Split(',');
            for(int j=0;j< cells.Length;j++)
            {
                walls[i, j].GetComponent<SpriteRenderer>().sprite = GetSprite(mapSprites[int.Parse(cells[j])]);
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

    int GetIDSprite(string name)
    {
        for (int i=0; i<sprites.Length;i++)
        {
            if (sprites[i].name == name)
            {
                return i;
            }
        }

        return 0;
    }

    int FindIDSprite(string name)
    {
        foreach ( var elem in mapSprites)
        {
            if (elem.Value == name)
            {
                return elem.Key;
            }
        }

        return 0;
    }
}
