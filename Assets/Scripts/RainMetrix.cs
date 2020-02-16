using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainMetrix : MonoBehaviour
{
    public Vector3 particleSize;     //particleSize = new Vector3(10, 10, 1);
    public Texture2D[] textures;
    public Color color;
    public float SpeedTime = 0.5f;
    public float Distance = 700.0f; // 700
    public float Area;

    public GameObject[] Particles;
    public int NumberOfParticles;
    public int NumberOfParticlesPerLines;

    private Sprite[] Sprites;
    private float DeltaSpeed =0;
    private int NumberOfParticlesPerColumn;
    private int NumberOfParticlesMax;

    Sprite GetSprite(int idOfTexture)
    {
        return Sprite.Create(textures[idOfTexture], new Rect(0, 0, textures[idOfTexture].width, textures[idOfTexture].height), new Vector2(0.5f, 0.5f));
    }

    // Start is called before the first frame update
    void Start()
    {
        if (NumberOfParticles < 1)
        {
            Debug.LogWarning("Not enough particles");
            return;
        }

        if (Distance == 0)
        {
            Debug.LogWarning("Distance can't be zero");
            return;
        }

        if (Distance == 0)
        {
            Debug.LogWarning("Area can't be zero");
            return;
        }

        Sprites = new Sprite[textures.Length];
        for (int i = 0; i < textures.Length; i++)
        {
            Sprites[i] = GetSprite(i);
        }

        NumberOfParticlesPerColumn = NumberOfParticles / NumberOfParticlesPerLines;
        NumberOfParticlesMax = NumberOfParticlesPerColumn * NumberOfParticlesPerLines;
        Particles = new GameObject[NumberOfParticlesMax];

        GameObject preObject = new GameObject();
        preObject.transform.SetParent(this.transform);
        //preObject.transform.localPosition = new Vector3();
        preObject.transform.localScale = particleSize;
        SpriteRenderer spriteRenderer = preObject.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = Sprites[0];
        spriteRenderer.color = color;
        Particles[0] = preObject;

        for (int i = 1; i < NumberOfParticlesMax; i++)
        {
            GameObject gameObject = GameObject.Instantiate(preObject);
            gameObject.transform.SetParent(this.gameObject.transform);
            gameObject.GetComponent<SpriteRenderer>().sprite = Sprites[Random.Range(0, Sprites.Length)];
            Particles[i] = gameObject;
        }

        float DistanceBeetweenParticlesY = textures[0].height / 10;
        for (int i = 0; i < NumberOfParticlesPerColumn; i++)
            for (int j = 0; j < NumberOfParticlesPerLines; j++)
            {
                Particles[i * NumberOfParticlesPerLines + j].transform.localPosition = new Vector3(Area * j / NumberOfParticlesPerLines - Area / 2, -i * DistanceBeetweenParticlesY, 0);
            }
    }

    // Update is called once per frame
    void Update()
    {
        DeltaSpeed -= Time.deltaTime;
        if (DeltaSpeed <= 0)
        {
            DeltaSpeed += SpeedTime;

            foreach (GameObject gameObject in Particles)
            {
                var pos = gameObject.transform.localPosition;
                if (pos.y < -Distance)
                {
                    gameObject.transform.localPosition = new Vector3(pos.x, 0, pos.z);
                }
                else
                {
                    gameObject.transform.localPosition = new Vector3(pos.x, pos.y - 1.0f, pos.z);
                }
            }
        }
    }
}
