using Sirenix.OdinInspector;
using UnityEngine;

public class TestScrollView : MonoBehaviour
{
    public ScrollViewExpand scrollViewExpand;
    public TestScrollViewCell itemPrefab;

    [Button]
    public void Test()
    {
        scrollViewExpand.Init<TestScrollViewCell>(10,itemPrefab.gameObject, (cell) =>
        {
         
        });
    }
}
