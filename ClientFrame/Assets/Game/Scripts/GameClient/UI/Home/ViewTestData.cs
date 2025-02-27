
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ViewTestData : ViewBase
{

    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private TextMeshProUGUI text;

    private int key;
    private void Awake()
    {
        text.text = "";
        inputField.onValueChanged.AddListener(OnNumChange);
    }

    private void OnNumChange(string t)
    {
        key = int.Parse(t);
        Debug.Log("key:"+key);
    }
    public void SetFlag()
    {
        PlayerInterFace.SetFlag(key);
    }

    public void GetFlag()
    {
        PlayerInterFace.GetFlag(key, out var value);
        text.text = value.ToString();
    }
    
    public void ClearFlag()
    {
        PlayerInterFace.ClearFlag(key);
    }
    
    public void SetExData()
    {
        PlayerInterFace.GetExData(key, out var value);
        PlayerInterFace.SetExData(key, value+1);
    }
    
    public void GetExData()
    {
        PlayerInterFace.GetExData(key, out var value);
        text.text = value.ToString();
    }
    
    public void ClearExData()
    {
        PlayerInterFace.ClearExData(key);
    }
    
}