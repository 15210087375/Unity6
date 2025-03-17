using System.Collections.Generic;
using UnityEngine;
public enum RotateAngle
{
    Angle0 = 0,
    Angle90 = 1,
    Angle180 = 2,
    Angle270 = 3,
}
public partial class CubsManager
{
    private CubeCellGroup GeneratorGroup()
    {
        var data = _dataTempList[Random.Range(0, _dataTempList.Count)];
        var randomRotate = Random.Range(0, 4);
        var tempData = GetRotateData(data, (RotateAngle)(randomRotate));
        var color = Random.Range(1,(int)ColorType.Count );
        var group = new CubeCellGroup(tempData,(ColorType)color);
        return group;
    }
   
}
