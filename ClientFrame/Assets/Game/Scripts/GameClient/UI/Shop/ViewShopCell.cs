
using cfg.Shop;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ViewShopCell : MonoBehaviour
{
   [SerializeField] private CanvasGroup nodeGroup;
   [SerializeField] private Image icon;
   [SerializeField] private TextMeshProUGUI nameText;
   [SerializeField] private TextMeshProUGUI priceText;
   [SerializeField] private Image imgDiscount;


   public void Init(StoreRecord storeRecord)
   {
      icon.SetIconId(storeRecord.Icon);
      nameText.SetText(storeRecord.Name);
      priceText.text =storeRecord.Price[0].ToString();
      imgDiscount.gameObject.SetActive(false);
      // imgDiscount.fillAmount = storeRecord.Discount / 100f;
   }
   
}