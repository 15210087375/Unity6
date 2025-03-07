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
    public List<int> mExdata = new List<int>();
    public ItemRecord ItemRecord;
    public ItemBase(int id,int count)
    {
        ItemId = id;
        Count = count;
        ItemRecord = TableManager.Instance.Tables.Item.Get(id);
    }
    protected ItemBase(ItemRecord item, int count)
    {
        ItemId = item.Id;
        Count = count;
        ItemRecord = item;
    }

    public static ItemBase CreateItem(int id, int count)
    {
        var item = TableManager.Instance.Tables.Item.Get(id);
        return CreateItem(item, count);
    }
    public static ItemBase CreateItem(ItemRecord item, int count)
    {
        Player.Instance.BagDirty = true;
        switch ((eBagType)item.BagType)
        {
            case eBagType.Res:
                break;
            case eBagType.Card:
            {
                return new ItemCard(item, count, 0);
            }
        }
        return null;
    }
  
    
   
    protected void SetExData(int idx, int value)
    {
        if (idx >= mExdata.Count)
        {
            for (int i = mExdata.Count; i <= idx; i++)
            {
                mExdata.Add(0);
            }
        }
        mExdata[idx] = value;
        Player.Instance.BagDirty = true;
    }
    public int GetExData(int idx)
    {
        if (idx >= mExdata.Count)
        {
            SetExData(idx, 0);
            return 0;
        }
        return mExdata[idx];
    }
    public void DeleteCount(int count)
    {
        Count -= count;
        Player.Instance.BagDirty = true;
    }
}

public class ItemCard : ItemBase
{
    
    public ItemCard(int id, int count, int level) : base(id, count)
    {
        SetExData(0, level);
    }
    public ItemCard(ItemRecord item, int count, int level) : base(item, count)
    {
        SetExData(0, level);
    }
}