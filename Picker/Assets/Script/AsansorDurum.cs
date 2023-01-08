using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsansorDurum : MonoBehaviour
{
    [SerializeField] private GameManager _GameManager;
    [SerializeField] private Animator  BariyerAlani;
    public void BariyerKaldir()
    {
        _GameManager.EfektiGoster("Checkpoint", transform.position);
        BariyerAlani.Play("BariyerKaldir");
    }
    public void Bitti()
    {//Asansor animasyonu bittiginde m�knat�s� yeniden harekete ba�lat�r
        _GameManager.ToplayiciHareketDurumu = true;
    }
}
