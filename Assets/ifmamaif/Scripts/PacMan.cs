using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class pacman : MonoBehaviour
{
    #region Types
    private enum TypeCell
    {
        Empty,
        Point,
        PowerUp,
        Portal,
        Wall,

    };
    #endregion

    #region Members
    private readonly string[] pacmanMap = new string[] {
" _____________________________________________________"  ,
"| . . . . . . . . . . . . | | . . . . . . . . . . . . |" ,
"| .  _____  .  _______  . | | .  _______  .  _____  . |" ,
"| . |     | . |       | . | | . |       | . |     | . |" ,
"| o |_____| . |_______| . |_| . |_______| . |_____| o |" ,
"| . . . . . . . . . . . . . . . . . . . . . . . . . . |" ,
"| .  _____  .  _  .  _____________  .  _  .  _____  . |" ,
"| . |_____| . | | . |_____   _____| . | | . |_____| . |" ,
"| . . . . . . | | . . . . | | . . . . | | . . . . . . |" ,
"|_________  . | |_____  . | | .  _____| | .  _________|" ,
"          | . |  _____|   |_|   |_____  | . |          " ,
"          | . | |                     | | . |          " ,
"          | . | |    _____   _____    | | . |          " ,
"__________| . |_|   |             |   |_| . |_________ " ,
"[           .       |             |       .          ] " ,
"__________  .  _    |             |    _  .  _________ " ,
"          | . | |   |_____________|   | | . |          " ,
"          | . | |                     | | . |          " ,
"          | . | |    _____________    | | . |          " ,
" _________| . |_|   |_____   _____|   |_| . |_________ " ,
"| . . . . . . . . . . . . | | . . . . . . . . . . . . |" ,
"| .  _____  .  _______  . | | .  _______  .  _____  . |" ,
"| . |___  | . |_______| . |_| . |_______| . |  ___| . |" ,
"| o . . | | . . . . . . . . . . . . . . . . | | . . o |" ,
"|___  . | | .  _  .  _____________  .  _  . | | .  ___|" ,
"|___| . |_| . | | . |_____   _____| . | | . |_| . |___|" ,
"| . . . . . . | | . . . . | | . . . . | | . . . . . . |" ,
"| .  _________| |_____  . | | .  _____| |_________  . |" ,
"| . |_________________| . |_| . |_________________| . |" ,
"| . . . . . . . . . . . . . . . . . . . . . . . . . . |" ,
"|_____________________________________________________|" ,
};

    private Dictionary<char, TypeCell> keyValues;
    private TypeCell[,] terrain;
    private GameObject[,] GameObjects = null;

    public Texture2D asciiTable;
    public int texelSize = 64;
    #endregion


    Sprite GetSprite(int x,int y)
    {
        //  return Sprite.Create(asciiTable, new Rect(texelSize * x / asciiTable.width, texelSize * y / asciiTable.height, texelSize, texelSize), new Vector2(0.5f, 0.5f));
        return Sprite.Create(asciiTable, new Rect(x*texelSize,y* texelSize, texelSize, texelSize), new Vector2(0.5f, 0.5f));
    }

    // Start is called before the first frame update
    void Start()
    {
        // keyValues.Add('|', TypeCell.Wall);
        // keyValues.Add('_', TypeCell.Wall);
        // keyValues.Add('.', TypeCell.Point);
        // keyValues.Add(' ', TypeCell.Empty);
        // keyValues.Add('o', TypeCell.PowerUp);
        // keyValues.Add('[', TypeCell.Portal);
        // keyValues.Add(']', TypeCell.Portal);
        //
        // terrain = new TypeCell[pacmanMap.Length, pacmanMap[0].Length];
        // for (int i = 0; i < pacmanMap.Length; i++)
        //     for (int j = 0; j < pacmanMap[i].Length; j++)
        //     {
        //         terrain[i, j] = keyValues[pacmanMap[i][j]];
        //     }
        //
        // GameObjects = new GameObject[pacmanMap.Length, pacmanMap[0].Length];
        // GameObject preObject = new GameObject();
        // preObject.AddComponent<SpriteRenderer>();



        //Text textComponenet = this.GetComponent<Text>();
        //string text = "";
        //foreach(string elem in pacmanMap)
        //{
        //    text += elem+"\n";
        //}
        //textComponenet.text = text;


        //GameObject gameObject = new GameObject();
        //gameObject.transform.SetParent(this.transform);
        //SpriteRenderer spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
        //spriteRenderer.sprite = Sprite.Create(asciiTable, new Rect(0, 0, asciiTable.width, asciiTable.height), new Vector2(0.5f, 0.5f));

        GameObjects = new GameObject[16, 16];

        // int i = 2, j = 2;
        int scale = 50;
        int size = 32;
        for (int i = 0; i < 16; i++)
            for (int j = 0; j < 16; j++)
            {
                GameObjects[i, j] = new GameObject(i + " " + j);
                GameObject gameObject = GameObjects[i, j];
                gameObject.transform.SetParent(this.transform);
                gameObject.transform.localScale = new Vector3(scale, scale, 0);
                gameObject.transform.localPosition = new Vector3((i-8)* size, (j-8)* size, 0);
                SpriteRenderer spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
                spriteRenderer.sprite = GetSprite(i, j);
                
            }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
