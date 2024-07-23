using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class Icon : MonoBehaviour
{
    public enum IconState{
        Normal,
        Selected,
        Deactivated
    }

    public enum IconType{
        Aquarius,
        Aries,
        Cancer,
        Capricorn,
        Gemini,
        Leo,
        Libra,
        Pisces,
        Sagittarius,
        Scorpio,
        Taurus,
        Virgo
    }

    [SerializeField]
    private IconType iconType;
    public IconType GetIconType{
        get => iconType;
    }
    private IconSelector parentSelector;
    public IconSelector GetParentIconSelector{
        get => parentSelector;
    }

    public Color normal;
    public Color selected;
    public Color deactivated;

    private IconState iconState;

    public IconState defaultIconState = IconState.Normal;

    public IconState SetIconState{
        set{
            switch(value){
                case IconState.Normal:
                    image.color = normal;
                    break;
                case IconState.Selected:
                    image.color = selected;
                    break;
                case IconState.Deactivated:
                    image.color = deactivated;
                    break;
            }

            iconState = value;
        }
    }

    [SerializeField]
    private ParticleSystem particles;
    

    private Image image;

    void Awake(){
        parentSelector = transform.parent.parent.GetComponent<IconSelector>();
        image = GetComponent<Image>();
        iconState = defaultIconState;
    }

    public void PlayParticles(){
        particles.Play();
    }
}
