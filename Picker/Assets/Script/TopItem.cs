using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopItem : MonoBehaviour
{
    //TopItem classý 2 farklý top türü için kullanýlmýþtýr
    [SerializeField] private GameManager _GameManager;
    [SerializeField] private string ItemTuru;
    [SerializeField] private int BonusTopIndex;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ToplayiciSinirObjesi"))
        {
            _GameManager.EfektiGoster("Item", transform.position);
            if (ItemTuru == "Palet")
            {
                _GameManager.PaletLeriOrtayaCikart();
                gameObject.SetActive(false);
            }
            else
            {
                //Sahnedeki her yeþil büyük top, bir tane top obje havuzu tutuyor
                //Toplarýn havuzlarý karýþmasýn diye scriptte index gönderiyorum
                _GameManager.BonusToplariEkle(BonusTopIndex);
                gameObject.SetActive(false);
            }
        }
    }
}
