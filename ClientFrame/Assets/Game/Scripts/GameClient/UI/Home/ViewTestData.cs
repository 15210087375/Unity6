
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
    }
    public void SetFlag()
    {
        PlayerInterFace.SetFlag(key);
    }

    public void GetFlag()
    {
        var value = PlayerInterFace.GetFlag(key);
        text.text = value.ToString();
    }
    
    public void ClearFlag()
    {
        PlayerInterFace.ClearFlag(key);
    }
    
    public void SetExData()
    {
        var value = PlayerInterFace.GetExData(key);
        PlayerInterFace.SetExData(key, value+1);
    }
    
    public void GetExData()
    {
        var value = PlayerInterFace.GetExData(key);
        text.text = value.ToString();
    }
    
    public void ClearExData()
    {
        PlayerInterFace.ClearExData(key);
    }
    
}