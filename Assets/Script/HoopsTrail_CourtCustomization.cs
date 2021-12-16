using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoopsTrail_CourtCustomization : MonoBehaviour
{
    [Header("Home Logo")]
    public MeshRenderer HomeLogo;
    public Texture LogoSource;

    [Header("CourtColor")]
    public MeshRenderer CourtFloor1;
    public MeshRenderer CourtFloor2;
    public Color HomeColor1;


    [Header("CourtSideColor")]
    public MeshRenderer CourtSide1;
    public MeshRenderer CourtSide2;
    public Color HomeColor2;


    [Header("CourtText")]
    public MeshRenderer CourtText1;
    public MeshRenderer CourtText2;
    public Texture Text;


    [Header("SideText")]
    public MeshRenderer SideText1;
    public MeshRenderer SideText2;
    public Texture SideText;

    private void Start()
    {
        HomeLogo.material.SetTexture("_MainTex", LogoSource);
        CourtFloor1.material.SetColor("_Color", HomeColor1);
        CourtFloor2.material.SetColor("_Color", HomeColor1);
        CourtSide1.material.SetColor("_Color", HomeColor2);
        CourtSide2.material.SetColor("_Color", HomeColor2);
        CourtText1.material.SetTexture("_MainTex", Text);
        CourtText2.material.SetTexture("_MainTex", Text);
        SideText1.material.SetTexture("_MainTex", SideText);
        SideText2.material.SetTexture("_MainTex", SideText);
    }
}
