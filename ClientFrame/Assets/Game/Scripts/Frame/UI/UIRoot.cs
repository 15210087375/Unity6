using System.Collections.Generic;
using UnityEngine;

public class UIRoot : MonoSingleton<UIRoot>
{
    public Canvas canvas;

    public GameObject nodeUI;

    public List<Transform> nodeLayers;
    
}
