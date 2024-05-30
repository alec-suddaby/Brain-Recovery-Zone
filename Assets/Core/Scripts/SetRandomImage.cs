using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetRandomImage : MonoBehaviour
{
    public List<Sprite> images;
    public Image targetImage;
    void Start()
    {
        targetImage.sprite = images[Random.Range(0, images.Count)];
    }

}
