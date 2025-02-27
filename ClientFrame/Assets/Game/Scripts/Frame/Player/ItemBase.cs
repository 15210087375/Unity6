using System.Collections.Generic;
using cfg.Res;
public enum ItemType
{
    Res = 0,        //资源
}
public class ItemBase
{
    public int ItemId;
    public int Count;
    public List<int> DataList = new List<int>();

    public ItemBase(int id,int count)
    {
        ItemId = id;
        Count = count;
    }
    public static ItemBase CreateItem(int id, int count)
    {
        Player.Instance.BagDirty = true;
        var item = new ItemBase(id, count);
        return item;
    }
    
   
    public void SetExData(int idx, int value)
    {
        if (idx >= DataList.Count)
        {
            for (int i = DataList.Count; i <= idx; i++)
            {
                DataList.Add(0);
            }
        }
        DataList[idx] = value;
        Player.Instance.BagDirty = true;
    }
    public int GetExData(int idx)
    {
        if (idx >= DataList.Count)
        {
            SetExData(idx, 0);
            return 0;
        }
        return DataList[idx];
    }
    public void DeleteCount(int count)
    {
        Count -= count;
        Player.Instance.BagDirty = true;
    }
}
