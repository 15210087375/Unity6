using System;
using System.Collections.Generic;
public enum eBagType
{
    Res = 0,        //资源
   
    Count ,
}

public class PlayerBag
{
    public Dictionary<int, BagOne> Bags = new Dictionary<int, BagOne>();

    public BagOne AddBag(eBagType e)
    {
        return AddBag((int)e);
    }
    public BagOne AddBag(int id)
    {
        var one = new BagOne(id);
        Bags[id] = one;
        return one;
    }
    public bool AddItem(int id, int count)
    {
        var tbItem = TableManager.Instance.Tables.Item.Get(id);
        if (tbItem == null)
        {
            Logger.Error("AddItem not find item id={0}", id);
            return false;
        }
        var bag = GetBag(tbItem.BagType);
        if(bag == null)
        {
            AddBag(tbItem.BagType);
            Logger.Error("not find bagId = {0}", tbItem.BagType);
        }
        Bags[tbItem.BagType].AddItem(id, count);
        return true;
    }
    public BagOne GetBag(int id)
    {
        return Bags.TryGetValue(id, out var one) ? one : null;
    }
}
public class BagOne
{
    public int BagId;
    public List<ItemBase> Items = new List<ItemBase>();
    public BagOne(int id)
    {
        BagId = id;
    }
    public ItemBase GetItemId(int id)
    {
        foreach (var item in Items)
        {
            if(item.ItemId == id)
            {
                return item;
            }
        }
        return null;
    }

    
    public void AddItem(int id,int count)
    {
        var tbItem = TableManager.Instance.Tables.Item.Get(id);
        if(tbItem == null)
        {
            Logger.Error("AddItem not find item id={0}", id);
            return;
        }
      
        //查询相同id（是否可堆叠）
        if (tbItem.MaxCount == -1)
        {
            //无限堆叠
            var theItem = GetItemId(id);
            if(theItem == null)
            {
                var item = ItemBase.CreateItem( id, count);//new ItemBase(id, count);
                Items.Add(item);
            }
            else
            {
                theItem.Count += count;
            }
        }
        else
        {
            if (tbItem.MaxCount == 1)
            {
                //不可堆叠
                for (int i = 0; i < count; i++)
                {
                    ItemBase item = ItemBase.CreateItem(id, 1);// new ItemBase(id, 1);
                    Items.Add(item);
                }
            }
            else
            {
                //可堆叠(先堆叠满已有格）
                foreach (var item in Items)
                {
                    if (item.ItemId == id)
                    {
                        if(item.Count < tbItem.MaxCount)
                        {
                            int needCount = tbItem.MaxCount - item.Count;
                            if(needCount < count)
                            {
                                item.Count += needCount;
                                count -= needCount;
                            }
                            else
                            {
                                item.Count += count;
                                break;
                            }
                        }
                    }
                }
            }
        }
    }
    public void ChangeItem(int id, int count)
    {
        var item = GetItemId(id);
        if (item != null)
        {
            var curCount = item.Count+ count;
            if(curCount <= 0)
            {
                Items.Remove(item);
            }
            else
            {
                item.Count = curCount;
            }
        }
    }
    public bool DeleteItem(int itemId)
    {
        var item = GetItemId(itemId);
        if (item == null)
        {
            Logger.Error("当前背包{0}中不存在物品item id={1}",BagId, itemId);
            return false;
        }
        return Items.Remove(item);
    }
}
