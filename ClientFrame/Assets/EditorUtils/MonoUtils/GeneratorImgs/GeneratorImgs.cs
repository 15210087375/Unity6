using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
public class GeneratorImgs:MonoBehaviour
{
    public RectTransform imgNode;
    public List<Image> imgList;
    [Button("生成图片")]
    public void GeneratorImg()
    {
        TestMerger(imgList,imgNode);
    }
    public  void TestMerger(List<Image> imgList,Transform imgNode)
    {
        var t = CombineIconGroupTextures(imgList,imgNode);
        if (t != null)
        {
            
            // 将纹理保存为PNG文件
            byte[] bytes = t.EncodeToPNG();
            string fileName = $"Assets/EditorUtils/MonoUtils/GeneratorImgs/combined_texture_{System.DateTime.Now.ToString("yyyyMMdd_HHmmss")}.png";
            System.IO.File.WriteAllBytes(fileName, bytes);
            Debug.Log($"纹理已保存到: {fileName}");
#if UNITY_EDITOR
            AssetDatabase.Refresh();
#endif
        }
    }
    private  Texture2D CombineIconGroupTextures(List<Image> imgs, Transform imgNode)
    {
        // 创建新的纹理，使用更大的尺寸以确保旋转后的图像不会被裁剪
        RectTransform tileRect = imgNode.GetComponent<RectTransform>();
        int targetWidth = (int)tileRect.sizeDelta.x; // 扩大尺寸以容纳旋转后的图像
        int targetHeight = (int)tileRect.sizeDelta.y;
        Vector2 centerOffset = new Vector2(targetWidth / 4f, targetHeight / 4f); // 中心偏移量

        Texture2D combinedTexture = new Texture2D(targetWidth, targetHeight);
        Color[] combinedColors = new Color[targetWidth * targetHeight];

        // 初始化为透明
        for (int i = 0; i < combinedColors.Length; i++)
        {
            combinedColors[i] = Color.clear;
        }

        // 获取tile的RectTransform
        Vector2 tileSize = tileRect.rect.size;

        var list = new List<Image>();
        list.AddRange(imgs);
        Logger.Error("imgNode child count:" + list.Count);
        // list.Add(mCenterImage);
        // 对每个图片进行处理
        foreach (Image img in list)
        {
            if (img == null || img.mainTexture == null)
                continue;

            Texture2D tex = img.mainTexture as Texture2D;
            if (tex == null)
                continue;
            var rate = img.rectTransform.sizeDelta.x / tileSize.x;
            GeneratorOne(img, imgNode, tileSize, targetWidth, targetHeight, centerOffset, tex, combinedColors,rate);
        }


        // 应用合并后的颜色
        combinedTexture.SetPixels(combinedColors);
        combinedTexture.Apply();

        return combinedTexture;
    }

    private  void GeneratorOne(Image img, Transform imgNode, Vector2 tileSize, int targetWidth, int targetHeight,
        Vector2 centerOffset, Texture2D tex, Color[] combinedColors, float scaleRate = 1)
    {
        // 获取图片相对于tile的本地变换信息
        RectTransform imgRect = img.rectTransform;
        Vector2 localPosition = imgNode.InverseTransformPoint(imgRect.position);
        Vector2 pivot = imgRect.pivot;
        Vector3 scale = imgRect.localScale * scaleRate;
        Quaternion rotation = img.gameObject.name.StartsWith("shadow") ? imgRect.parent.rotation : imgRect.rotation;

        // 计算图片在tile中的实际位置（考虑pivot）
        float pivotOffsetX = (pivot.x - 0.5f) * imgRect.rect.width;
        float pivotOffsetY = (pivot.y - 0.5f) * imgRect.rect.height;
        Vector2 adjustedPosition = localPosition - new Vector2(pivotOffsetX, pivotOffsetY);

        Logger.Error("tileSize:" + tileSize);
        // 创建完整的变换矩阵
        Matrix4x4 transformMatrix = Matrix4x4.TRS(
            new Vector3(adjustedPosition.x / tileSize.x * targetWidth + centerOffset.x,
                adjustedPosition.y / tileSize.y * targetHeight + centerOffset.y,
                0),
            rotation,
            new Vector3(scale.x, scale.y, 1)
        );

        // 遍历目标纹理的每个像素
        for (int y = 0; y < targetHeight; y++)
        {
            for (int x = 0; x < targetWidth; x++)
            {
                // 将像素坐标转换为以图片中心为原点的坐标
                Vector2 pixelPos = new Vector2(
                    x - centerOffset.x,
                    y - centerOffset.y
                );

                // 应用逆变换，获取原始纹理的采样坐标
                Vector3 sourcePos = transformMatrix.inverse.MultiplyPoint3x4(new Vector3(pixelPos.x, pixelPos.y, 0));
                Vector2 sourceUV = new Vector2(
                    (sourcePos.x / targetWidth) + 0.5f,
                    (sourcePos.y / targetHeight) + 0.5f
                );

                // 检查UV是否在有效范围内
                if (sourceUV.x >= 0 && sourceUV.x <= 1 && sourceUV.y >= 0 && sourceUV.y <= 1)
                {
                    // 使用双线性插值采样原始纹理
                    Color sourceColor = SampleTexture(tex, sourceUV);
                    Color tintedColor = sourceColor * img.color;

                    // 混合颜色
                    int pixelIndex = y * targetWidth + x;
                    combinedColors[pixelIndex] = BlendColors(combinedColors[pixelIndex], tintedColor);
                }
            }
        }
    }

    // 双线性插值采样纹理
    private  Color SampleTexture(Texture2D tex, Vector2 uv)
    {
        float x = uv.x * (tex.width - 1);
        float y = uv.y * (tex.height - 1);

        int x1 = Mathf.FloorToInt(x);
        int y1 = Mathf.FloorToInt(y);
        int x2 = Mathf.Min(x1 + 1, tex.width - 1);
        int y2 = Mathf.Min(y1 + 1, tex.height - 1);

        float fx = x - x1;
        float fy = y - y1;

        Color c11 = tex.GetPixel(x1, y1);
        Color c12 = tex.GetPixel(x1, y2);
        Color c21 = tex.GetPixel(x2, y1);
        Color c22 = tex.GetPixel(x2, y2);

        Color interpolatedColor = Color.Lerp(
            Color.Lerp(c11, c12, fy),
            Color.Lerp(c21, c22, fy),
            fx
        );

        return interpolatedColor;
    }

    // 混合两个颜色
    private  Color BlendColors(Color bottom, Color top)
    {
        float alpha = top.a;
        return new Color(
            bottom.r * (1 - alpha) + top.r * alpha,
            bottom.g * (1 - alpha) + top.g * alpha,
            bottom.b * (1 - alpha) + top.b * alpha,
            Mathf.Max(bottom.a, top.a)
        );
    }
}