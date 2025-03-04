using Sirenix.OdinInspector;
using UnityEngine;

public class TestScrollView : MonoBehaviour
{
    public ScrollViewExpand1 scrollViewExpand1;
    public TestScrollViewCell itemPrefab;

    [Button]
    public void Test()
    {
        scrollViewExpand1.Init<TestScrollViewCell>(10,itemPrefab.gameObject, (cell) =>
        {
         
        });
    }
}
