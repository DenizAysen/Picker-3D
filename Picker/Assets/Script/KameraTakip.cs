using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KameraTakip : MonoBehaviour
{
    [SerializeField] private Transform Target;
    [SerializeField] private Vector3 Target_offset;//Kamera Takip Mesafesi
    void LateUpdate()
    {// Kamera editorde girilen uzaklýk kadar oyuncudan uzaklýkta, oyuncuyu takip eder
        transform.position = Vector3.Lerp(transform.position, Target.position + Target_offset, .125f);
    }
}
