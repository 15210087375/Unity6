using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TPAtlas", menuName = "TPAtlas")]
public class TPAtlas : ScriptableObject
{
    public List<Sprite> sprites;
    public Texture texture;
    public Dictionary<string,Sprite> spriteDict;
}
