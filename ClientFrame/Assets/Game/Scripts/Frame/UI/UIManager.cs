using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

public enum UIType
{
    None,
    Layer,
    View
}
public class UIManager : Singleton<UIManager>
{
    private Dictionary<WindowID, UIBase> _uiDic = new Dictionary<WindowID, UIBase>();
    private List<ViewBase> _viewList = new List<ViewBase>();

    
    private LayerBase _previousLayer;
    private LayerBase _curLayer;
    
    public void InitFirstScene()
    {
       // SwitchLayer(WindowID.LayerHome);
       OpenView(WindowID.ViewTabBar,null,true);
    }
    private void OpenUI(WindowID windowID,Action<UIBase> action,bool isResident = false)
    {
        if(_uiDic.ContainsKey(windowID))
        {
            var t = _uiDic[windowID];
            OpenUI(t,action,isResident);
        }
        else
        {
            var pathData = UIPathDefine.WindowPath[windowID];
            var path = UIPathDefine.FrontPath + pathData.Path;
            LoadManager.Instance.LoadAsset<GameObject>(path, (obj) =>
            {
                if(obj == null)
                {
                    Debug.LogError($"{path} 加载失败");
                    return;
                }
                var go = Object.Instantiate(obj,UIRoot.Instance.nodeLayers[(int)pathData.LayerIndex].transform);
                var t = go.GetComponent<UIBase>();
                t.windowID = windowID;
                _uiDic.Add(windowID,t);
                OpenUI(t,action,isResident);
           
            });
        }
       
    }
 
    private void OpenUI(UIBase t,Action<UIBase> action,bool isResident)
    {
       
        t.OnInit();
        var transform = t.transform;
        transform.localPosition = Vector3.zero;
        transform.localScale = Vector3.one;
        t.gameObject.SetActive(true);
        t.isResident = isResident;
        t.OnOpen();
        action?.Invoke(t);
    }
    
   
    public void CloseUI(WindowID id,Action action = null)
    {
        if (!_uiDic.ContainsKey(id)) return;
        var t = _uiDic[id];
        t.gameObject.SetActive(false);
        action?.Invoke();
    }
    
    private void CloseAllView()
    {
        for (var i = 0; i < _viewList.Count; i++)
        {
            if(_viewList[i].isResident) continue;
            _viewList[i].Close();
        }
        _viewList.Clear();
    }
    

    public void SwitchLayer(WindowID id,Action action = null)
    {
        if (_curLayer != null)
        {
            _previousLayer = _curLayer;
        }
        
        OpenUI(id, (layer) =>
        {
            _curLayer = layer as LayerBase;
            if (_curLayer == null)
            {
                Debug.LogError("CurLayer is null");
                return;
            }
            CloseAllView();
            var moveAnim = _previousLayer != null && _previousLayer.windowID.Group() == WindowGroup.Home && layer.windowID.Group() == WindowGroup.Home;
            if (moveAnim)
            {
                var left = _previousLayer.windowID > layer.windowID;
                var animType = left ? WindowAnimType.MoveToRight : WindowAnimType.MoveToLeft;
                _previousLayer.LayerAnim(animType, false, () =>
                {
                    _previousLayer.gameObject.SetActive(false);
                    CloseUI(_previousLayer.windowID, null);
                    action?.Invoke();
                });
                _curLayer.LayerAnim(animType, true);
            }else
            {
                if (_previousLayer)
                {
                    CloseUI(_previousLayer.windowID, null);
                }
                action?.Invoke();
            }
            
        });
    }
    
    
    public void OpenView(WindowID id,Action action = null,bool isResident = false)
    {
        OpenUI(id, (view) =>
        {
            var panel = view as ViewBase;
            if (panel != null)
            {
                _viewList.Add(panel);
            }
            action?.Invoke();
        },isResident);
    }
}
