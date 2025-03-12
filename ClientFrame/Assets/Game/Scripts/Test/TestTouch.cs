using System;
using UnityEngine;

public class TestTouch : MonoBehaviour
{
    [SerializeField] private Transform scaleNode;
    [SerializeField] private Camera uiCamera;
    public float zoomSpeed = 2f;
    public float minZoom = 1f;
    public float maxZoom = 2.5f;
    private Vector3 lastPos = new Vector3(0, 0, 0);
    public Vector3 basePos = new Vector3(0, 0, 0);
    private float lastS = 1f;
    private bool firstPress = false;
    private Vector3 initScale ;

    private int _continueSacleTick = 0;
    private int _continueMoveTick = 0;
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
                initScale = scaleNode.localScale;
                Debug.LogError("初始" + initScale);
            }

            // 触摸移动：计算缩放
            if (touch1.phase == TouchPhase.Moved || touch2.phase == TouchPhase.Moved)
            {
                Debug.LogError($"{touch1.phase}  {touch2.phase}  Dir1:{touch1.deltaPosition}  Dir2:{touch2.deltaPosition}  Angle:{Vector3.Angle(touch1.deltaPosition, touch2.deltaPosition)}");

                if (touch1.phase == TouchPhase.Moved && touch2.phase == TouchPhase.Stationary ||
                    touch1.phase == TouchPhase.Stationary && touch2.phase == TouchPhase.Moved ||
                    touch1.phase == TouchPhase.Moved && touch2.phase == TouchPhase.Moved &&
                    Vector3.Angle(touch1.deltaPosition, touch2.deltaPosition) > 30)
                {
                    //缩放
                    if (_continueSacleTick > 2)
                    {
                        Vector2 touch0PrevPos = touch1.position - touch1.deltaPosition;
                        Vector2 touch1PrevPos = touch2.position - touch2.deltaPosition;
                    
                        float prevMagnitude = (touch0PrevPos - touch1PrevPos).magnitude;
                        float currentMagnitude = (touch1.position - touch2.position).magnitude;
                    
                        float difference = currentMagnitude - prevMagnitude;
                        // Debug.LogError($"{difference}   {touch1.position}  {touch1.deltaPosition}  {touch2.position}  {currentMagnitude}  {prevMagnitude} ");
                        Zoom(difference/100 * zoomSpeed);
                    }
                    
                    
                    _continueSacleTick++;
                }
                else if(touch1.phase == TouchPhase.Moved && touch2.phase == TouchPhase.Moved )
                {
                    if (_continueMoveTick > 2)
                    {
                        var input = uiCamera.ScreenToWorldPoint(Input.mousePosition);
                        var localPos = scaleNode.parent.InverseTransformPoint(input);
                        if (!firstPress)
                        {
                            lastPos = localPos;
                            firstPress = true;
                        }
                        var newPos = scaleNode.localPosition + (localPos - lastPos);
                        lastPos = localPos;
                        scaleNode.localPosition = SetPos(newPos,lastS);
                    }
                    _continueMoveTick++;
                    
                }
                else
                {
                    _continueSacleTick = 0;
                    _continueMoveTick = 0;
                }
               
              
            }
            else
            {
                _continueSacleTick = 0;
                _continueMoveTick = 0;
            }
        }
      
    }
     void Zoom(float increment)
     {
         var lastScale = scaleNode.localScale.x;
         float s = Mathf.Clamp(lastScale + increment, minZoom, maxZoom);
         scaleNode.localScale = new Vector3(s,s,0);

         var input = uiCamera.ScreenToWorldPoint(Input.mousePosition);
         var localPos = scaleNode.parent.InverseTransformPoint(input);
         var diff = (scaleNode.localPosition - localPos) / lastS;
         var newPos = localPos + diff * s;
         lastS = s;
         scaleNode.localPosition = SetPos(newPos,s);
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
