using System;
using UnityEngine;

public class TestTouch : MonoBehaviour
{
    [SerializeField] private Transform scaleNode;
    [SerializeField] private Camera uiCamera;
    public float zoomSpeed = 2f;
    public float minZoom = 1f;
    public float maxZoom = 2.5f;
    public Vector3 basePos = new Vector3(0, 0, 0);
    private float lastS = 1f;
    private Vector3 lastPos = new Vector3(0, 0, 0);
    private bool firstPress = false;
    private float initDis = 0;
    private Vector3 initScale ;

    private void Start()
    {
        uiCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        RealZoom();
    }
     private void RealZoom()
    {
        // 检测双指触摸
        if (Input.touchCount == 2)
        {
            Touch touch1 = Input.GetTouch(0);
            Touch touch2 = Input.GetTouch(1);

            // 触摸开始：记录初始距离和缩放值
            if (touch2.phase == TouchPhase.Began)
            {
                initDis = Vector2.Distance(touch1.position, touch2.position);
                initScale = scaleNode.localScale;
            }

            // 触摸移动：计算缩放
            if (touch1.phase == TouchPhase.Moved || touch2.phase == TouchPhase.Moved)
            {
                if (touch1.phase == TouchPhase.Moved && touch2.phase == TouchPhase.Moved &&Vector3.Angle(touch1.deltaPosition, touch2.deltaPosition) < 45)
                {
                    //双指同时移动且同方向，拖拽
                    var input = uiCamera.ScreenToWorldPoint(touch1.position);
                    var localPos = scaleNode.parent.InverseTransformPoint(input);
                    scaleNode.localPosition = SetPos(scaleNode.localPosition + (localPos - lastPos),scaleNode.localScale.x);
                }
                else
                {
                    //缩放
                    
                    Vector2 currentTouch1Pos = touch1.position;
                    Vector2 currentTouch2Pos = touch2.position;
                    float currentDistance = Vector2.Distance(currentTouch1Pos, currentTouch2Pos);

                    // 避免初始距离为0导致除零错误
                    if (initDis == 0) initDis = currentDistance;

                    // 计算缩放比例
                    float scaleFactor = (currentDistance / initDis) * zoomSpeed;
                    Vector3 newScale = initScale * scaleFactor;

                    // 限制缩放范围
                    newScale.x = Mathf.Clamp(newScale.x, minZoom, maxZoom);
                    newScale.y = Mathf.Clamp(newScale.y, minZoom, maxZoom);
                    newScale.z = 1; // 保持Z轴不变

                    // 应用缩放
                    scaleNode.localScale = newScale;
                }
              
            }
        }
    }
     Vector3 SetPos(Vector3 newPos,float nowScale)
     {
         var size = scaleNode.parent.GetComponent<RectTransform>().sizeDelta;
         float[] re = new[]
         {
             basePos.y + (basePos.y + size.y / 2) * (nowScale - minZoom),
             basePos.y + (basePos.y - size.y / 2) * (nowScale - minZoom),
             basePos.x + (basePos.x - size.x / 2) * (nowScale - minZoom),
             basePos.x + (basePos.x + size.x / 2) * (nowScale - minZoom),
         };
         return new Vector3(Mathf.Clamp(newPos.x, re[2], re[3]), Mathf.Clamp(newPos.y, re[1], re[0]), 0);
     }

}
