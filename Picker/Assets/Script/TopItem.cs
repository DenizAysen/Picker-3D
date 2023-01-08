using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopItem : MonoBehaviour
{
    //TopItem class� 2 farkl� top t�r� i�in kullan�lm��t�r
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
                //Sahnedeki her ye�il b�y�k top, bir tane top obje havuzu tutuyor
                //Toplar�n havuzlar� kar��mas�n diye scriptte index g�nderiyorum
                _GameManager.BonusToplariEkle(BonusTopIndex);
                gameObject.SetActive(false);
            }
        }
    }
}
