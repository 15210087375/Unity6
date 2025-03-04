using TMPro;
using UnityEngine;

public class TestScrollViewCell : ScrollViewExpandCell
{
    public TextMeshProUGUI textShowIndex;
    public TextMeshProUGUI textDataIndex;
    public GameObject nodeSelect;
    
   
    
    public override void OnSelect(int index)
    {
        nodeSelect.gameObject.SetActive(index == dataIndex);
    }
    
    public void OnClickCell()
    {
        Debug.Log("OnClick:" + dataIndex);
        Sc.MoveToIndex(dataIndex);
    }
}
