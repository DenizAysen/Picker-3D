using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.SceneManagement;

[Serializable]
public class TopAlaniTeknikIslemler
{
    public Animator TopAlaniAsansoru;
    public TextMeshProUGUI SayiText;
    public int AtilmasiGerekenTopSayisi;
    public GameObject[] Toplar;
}
public class GameManager : MonoBehaviour
{
    [Header("LEVEL AYARLARI")]
    [SerializeField] private GameObject ToplayiciObje;
    [SerializeField] private GameObject[] ToplayiciPaletler;
    [SerializeField] private GameObject[] BonusToplar;
    [SerializeField] private GameObject TopKontrolObjesi;
    [SerializeField] private float ToplayiciHareketHizi;
    public bool ToplayiciHareketDurumu;

    [Header("CHECKPOINTLER")]
    [SerializeField] private List<TopAlaniTeknikIslemler> _TopAlaniTeknikIslemler = new List<TopAlaniTeknikIslemler>();
    
    [Header("UI OBJELERI")]
    [SerializeField] private GameObject[] Paneller;
    [SerializeField] private TextMeshProUGUI[] UITextleri;
    //[SerializeField] private GameObject[] Altinlar;
    [Header("SESLER")]
    [SerializeField] private AudioSource[] Sesler;
    [Header("EFEKTLER")]
    [SerializeField]private ParticleSystem[] Efektler;
    [SerializeField] private Transform[] CheckpointPozisyonlari;
    #region privates
    private bool PaletlerVarMi;
    private int AtilanTopSayisi;
    private int ToplamCheckPointSayisi;
    private int MevcutCheckPointIndex;
    private int MevcutCheckPointEfektIndex;
    float ParmakPozX;
    private int toplamAtilanTopSayisi;
    private Transform ObjeTransform;
    private Collider[] ToplananToplar;
    private Vector3 TouchPosition;
    #endregion
    void Start()
    {
        toplamAtilanTopSayisi = 0;
        ToplayiciHareketDurumu = true;
        ObjeTransform = ToplayiciObje.transform;
        ToplamCheckPointSayisi = _TopAlaniTeknikIslemler.Count-1;
        for(int i = 0; i < _TopAlaniTeknikIslemler.Count; i++)
        {
            _TopAlaniTeknikIslemler[i].SayiText.text = AtilanTopSayisi +
            "/" + _TopAlaniTeknikIslemler[i].AtilmasiGerekenTopSayisi;
        }
        MevcutCheckPointIndex = 0;
        MevcutCheckPointEfektIndex = MevcutCheckPointIndex;
    }
    void Update()
    {
        if (ToplayiciHareketDurumu)//Top hareket edebiliyorsa ve oyun durmamýþsa toplayýcýyý hareket ettirir
        {
            ToplayiciObje.transform.position += ToplayiciHareketHizi * Time.deltaTime * ToplayiciObje.transform.forward;
            if(Time.timeScale != 0)
            {
                //Telefonda ekrana dokunuluyorsa bu kodlar çalýþýr
                if(Input.touchCount > 0)
                {
                    Touch touch = Input.GetTouch(0);
                    TouchPosition = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 10f));
                    switch (touch.phase)
                    {
                        case TouchPhase.Began://Ekrana dokunulmaya baþlandýðýnda çalýþýr
                            ParmakPozX = TouchPosition.x - ToplayiciObje.transform.position.x;
                            break;
                        case TouchPhase.Moved://Parmak ekranda hareket ettirildiðinde çalýþýr
                            if(TouchPosition.x -ParmakPozX> 1.15f && TouchPosition.x - ParmakPozX < -1.15f)
                            {
                                ToplayiciObje.transform.position = Vector3.Lerp(ToplayiciObje.transform.position, new Vector3(touch.position.x - ParmakPozX,
                                    ToplayiciObje.transform.position.y, ToplayiciObje.transform.position.z), 3f);
                            }
                            break;
                    }
                }
                else
                {
                    if (Input.GetKey(KeyCode.LeftArrow))
                    {
                        ToplayiciObje.transform.position = Vector3.Lerp(ToplayiciObje.transform.position, new Vector3(ObjeTransform.position.x - .1f
                            , ObjeTransform.position.y, ObjeTransform.position.z), .15f);
                    }
                    if (Input.GetKey(KeyCode.RightArrow))
                    {
                        ToplayiciObje.transform.position = Vector3.Lerp(ToplayiciObje.transform.position, new Vector3(ObjeTransform.position.x + .1f
                            , ObjeTransform.position.y, ObjeTransform.position.z), .15f);
                    }
                }                
            }
        }
    }
    public void SiniraGelindi()
    {
        //Oyuncu checkpointe geldiðinde eðer sahnede aktif palet varsa kapatýlýr
        if (PaletlerVarMi)
        {
            ToplayiciPaletler[0].SetActive(false);
            ToplayiciPaletler[1].SetActive(false);
        }
        PaletlerVarMi = false;
        ToplayiciHareketDurumu = false; //Sýnýrda mýknatýsý durdurur.
        Invoke("AsamaKontrol", 2f);//Sýnýra geldikten 2 sn sonra toplanan top sayýsý kontrol edilir
        /*Bir alana hayali kutu çizer. Alanda bulunan bütün colliderlarý bulur.
         Bulduðu Colliderlarý Collider[] olarak döndürür*/
        ToplananToplar = Physics.OverlapBox(TopKontrolObjesi.transform.position,
            TopKontrolObjesi.transform.localScale/2,Quaternion.identity);
        int i = 0;
        while(i < ToplananToplar.Length)
        {
            ToplananToplar[i].GetComponent<Rigidbody>().AddForce(new Vector3(0, 0, .6f)
                ,ForceMode.Impulse);
             i++;
            //Mýknatýsta bulunan toplara ufak bir güç uygular
        }
        ToplananToplar = null;
    }

    public void ToplariSay()
    {//Checkpointe düþen toplarý sayar
        AtilanTopSayisi++;
        _TopAlaniTeknikIslemler[MevcutCheckPointIndex].SayiText.text = AtilanTopSayisi + 
            "/" + _TopAlaniTeknikIslemler[MevcutCheckPointIndex].AtilmasiGerekenTopSayisi;
    }
    void AsamaKontrol()
    {   /*Ýstenen top sayýsýna ulaþýlmýþsa oyuncu checkpointi geçebilir
        Öbür türlü oyuncu leveli kaybeder*/
        if(AtilanTopSayisi >= _TopAlaniTeknikIslemler[MevcutCheckPointIndex].AtilmasiGerekenTopSayisi)
        {
            EfektiGoster("Checkpoint", CheckpointPozisyonlari[MevcutCheckPointEfektIndex].position);
            toplamAtilanTopSayisi += AtilanTopSayisi;
            _TopAlaniTeknikIslemler[MevcutCheckPointIndex].TopAlaniAsansoru.Play("Asansor");
            foreach (var item in _TopAlaniTeknikIslemler[MevcutCheckPointIndex].Toplar)
            {
                item.SetActive(false);
            }
            if(MevcutCheckPointIndex == ToplamCheckPointSayisi)
            {
             //   toplamAtilanTopSayisi += AtilanTopSayisi;
                ToplayiciHareketDurumu = false;
                Invoke("Kazandin", 1f);
            }
            else
            {
                MevcutCheckPointIndex++;
                MevcutCheckPointEfektIndex = MevcutCheckPointIndex;
                //toplamAtilanTopSayisi += AtilanTopSayisi;
                AtilanTopSayisi = 0;
                Sesler[3].Play();
            }
            Debug.Log(toplamAtilanTopSayisi);
        }
        else
        {
            Kaybettin();
        }
    }
    public void PaletLeriOrtayaCikart()
    {//ToplayiciPaletler arrayindeki paletleri sahnede aktif eder.
        PaletlerVarMi = true;
        ToplayiciPaletler[0].SetActive(true);
        ToplayiciPaletler[1].SetActive(true);
        Sesler[2].Play();
    }
    public void BonusToplariEkle(int BonusTopIndex)
    {
        BonusToplar[BonusTopIndex].SetActive(true);
        Sesler[2].Play();
    }
    public void UIButonlariIslevleri(string islem)
    {
        switch (islem)
        {
            case "durdur":
                Time.timeScale = 0;
                Paneller[0].SetActive(true);
                break;
            case "devamet":
                Time.timeScale = 1;
                Paneller[0].SetActive(false);
                break;
            case "cikis":
                Application.Quit();
                break;
            case "sonraki":
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                break;
            case "tekraret":
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                break;

        }
    }
    void Kazandin()
    {
        UITextleri[0].text = "LEVEL : " + SceneManager.GetActiveScene().name;
        UITextleri[2].text = toplamAtilanTopSayisi.ToString();
        Paneller[1].SetActive(true);
        Sesler[0].Play();
        Time.timeScale = 0;
    }
    void Kaybettin()
    {
        UITextleri[1].text = "LEVEL : " + SceneManager.GetActiveScene().name;
        Paneller[2].SetActive(true);
        Sesler[1].Play();
        Time.timeScale = 0;
    }
    public void EfektiGoster(string EfektAdi,Vector3 efektPos)
    {
        switch (EfektAdi)
        {
            case "Checkpoint":
                Efektler[0].gameObject.SetActive(true);
                Efektler[0].transform.position = efektPos;
                Efektler[0].Play();
                break;
            case "Item":
                Efektler[1].gameObject.SetActive(true);
                Efektler[1].transform.position = efektPos;
                Efektler[1].Play();
                break;
        }
    }
}
