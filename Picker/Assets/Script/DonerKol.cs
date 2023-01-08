using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DonerKol : MonoBehaviour
{
    bool Don;
    /* Kollar platformun aþaðýsýndan mýknatýsýn uçlarýna doðru geliyor
    Kollar mýknatýsa yerleþinçe dönmeye baþlýyor */
    [SerializeField] private float DonusDegeri;//1 mýknatýsta deðeri + diðerinde - dir
    public void DonmeyeBasla()
    {
        Don = true;
    }
    void Update()
    {
        /*2 kolun toplarý almasý için mýknatýsýn içine doðru dönmesi gerekiyor
        DonusDegeri bu deðerle birbirlerine zýt þekilde ve mýknatýsýn içine doðru
        donuyorlar*/
        if (Don)
        transform.Rotate(0, 0, DonusDegeri, Space.Self);
    }
}
