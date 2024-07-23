using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RawImage))]
public class IconSwitcher : MonoBehaviour
{
    private RawImage image;
    public Texture currentTexture => image.texture;

    public Vector2 positionalVariance = new Vector2(50, 50);
    private Vector2 startingPosition;

    void Awake(){
        image = GetComponent<RawImage>();
        startingPosition = transform.localPosition;
    }

    public void SetIcon(Texture texture){
        image.texture = texture;

        transform.localPosition = startingPosition + new Vector2(Random.Range(-positionalVariance.x, positionalVariance.x), Random.Range(-positionalVariance.y, positionalVariance.y));
    }
}
