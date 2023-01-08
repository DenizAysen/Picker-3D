using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DonerKol : MonoBehaviour
{
    bool Don;
    /* Kollar platformun a�a��s�ndan m�knat�s�n u�lar�na do�ru geliyor
    Kollar m�knat�sa yerle�in�e d�nmeye ba�l�yor */
    [SerializeField] private float DonusDegeri;//1 m�knat�sta de�eri + di�erinde - dir
    public void DonmeyeBasla()
    {
        Don = true;
    }
    void Update()
    {
        /*2 kolun toplar� almas� i�in m�knat�s�n i�ine do�ru d�nmesi gerekiyor
        DonusDegeri bu de�erle birbirlerine z�t �ekilde ve m�knat�s�n i�ine do�ru
        donuyorlar*/
        if (Don)
        transform.Rotate(0, 0, DonusDegeri, Space.Self);
    }
}
