using System;
using UnityEngine;
using UnityEngine.UI;

//扩展UGUI的功能
public static partial class GameUtils
{
    
}
public static partial class GameExtensions
{
    //根据图标Id 设置图片
    public static void SetIconId(this Image img, int iconId, bool isGray = false, Action callback = null)
    {
        if (iconId == -1)
        {
            return;
        }
      

    }

    //设置灰色
    public static void SetGrey(this Graphic img, bool isGray)
    {
        if (isGray == false)
        {
            img.SetMaterial("");
            return;
        }

        img.SetMaterial("Grey");
    }

    //设置图集
    public static void SetAtlas(this Image img, string atlasName, string iconName, Action callback = null)
    {
        var name = $"Image/{atlasName}/{iconName}.png";
        LoadManager.Instance.LoadAsset<Sprite>(name,  (res) =>
        {
            if (img == null)
            {
                return;
            }
            if (res != null)
            {
                img.sprite = res as Sprite;
            }
            else
            {
                Logger.Error($"{name} not found!");
            }

            callback?.Invoke();
        });
    }

    public static void SetAtlasJPG(this Image img, string atlasName, string iconName, Action callback = null)
    {
        var name = $"Image/{atlasName}/{iconName}.jpg";
        LoadManager.Instance.LoadAsset<Sprite>(name,  (res) =>
        {
            if (img == null)
            {
                return;
            }
            if (res != null)
            {
                img.sprite = res as Sprite;
            }
            else
            {
                Logger.Error($"{name} not found!");
            }

            callback?.Invoke();
        });
    }
    public static void SetTexture(this RawImage img, string path, Action callback = null,bool ifjpg = true)
    {
        var name = ifjpg ? $"{path}.jpg" : $"{path}.png";
        LoadManager.Instance.LoadAsset<Texture>(name,  (res) =>
        {
            if (img == null)
            {
                return;
            }
            if (res != null)
            {
                img.texture = res as Texture;
            }
            else
            {
                Logger.Error($"{name} not found!");
            }

            callback?.Invoke();
        });
    }
    //设置透明度
    public static void SetAlpha(this Graphic graphic, float alpha)
    {
        var color = graphic.color;
        color.a = alpha;
        graphic.color = color;
    }

    //设置材质球
    public static void SetMaterial(this Graphic img, string materialName)
    {
        var rq = img.material.renderQueue;
        if (string.IsNullOrEmpty(materialName))
        {
            img.material = null;
            return;
        }

        var fileName = $"Material/{materialName}.mat";

        LoadManager.Instance.LoadAsset<Material>(fileName,  (res) =>
        {
            if (img == null)
            {
                return;
            }
            if (res != null)
            {
                img.material = res as Material;
                //重新设置rq
                img.material.renderQueue = rq;
            }
            else
            {
                Logger.Error($"{fileName} not found!");
            }
        });
    }
}