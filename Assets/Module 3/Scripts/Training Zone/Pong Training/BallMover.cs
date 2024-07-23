using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BallMover : MonoBehaviour
{
    public float delay = 0.1f;

    public float minHeight = 0;
    public float maxHeight = 0;

    public float fadeSpeed = 1f;
    public bool changeAngle = true;
    private bool canMove = false;

    private Transform lastHit;
    public Image image;

    void Start(){
        StartCoroutine("FadeOut");
    }

    public void OnTriggerEnter2D(Collider2D collider){
        lastHit = collider.transform;
        StartCoroutine("Move");
        lastHit.GetComponent<PongBall>().Visibile = false;
    }

    public void OnTriggerExit2D(Collider2D collider){
        lastHit.GetComponent<PongBall>().Visibile = true;
    }

    public void SetActive(bool active){
        if(active){
            StartCoroutine("FadeIn");
            return;
        }

        StartCoroutine("FadeOut");
    }

    IEnumerator FadeIn(){
        canMove = true;
        float alpha = image.color.a;

        while(alpha < 1f){
            alpha = Mathf.Clamp01(alpha + (fadeSpeed * Time.fixedDeltaTime));
            Color colour = image.color;
            colour.a = alpha;
            image.color = colour;

            yield return new WaitForFixedUpdate();
        }
    }

    IEnumerator FadeOut(){
        canMove = false;
        float alpha = image.color.a;

        while(alpha > 0f){
            alpha = Mathf.Clamp01(alpha - (fadeSpeed * Time.fixedDeltaTime));
            Color colour = image.color;
            colour.a = alpha;
            image.color = colour;

            yield return new WaitForFixedUpdate();
        }
    }

    IEnumerator Move(){
        if(!canMove){
            yield break;
        }
        yield return new WaitForSeconds(delay);

        Vector3 position = lastHit.transform.localPosition;
        position.y = Random.Range(minHeight, maxHeight);
        lastHit.transform.localPosition = position;

        if(!changeAngle){
            yield break;
        }
        Rigidbody2D rb = lastHit.GetComponent<Rigidbody2D>();
        Vector2 velocity = rb.velocity;
        float magnitude = velocity.magnitude;

        velocity.y = 0;

        velocity.x /= Mathf.Abs(velocity.x);
        float offset = Random.Range(-0.75f, 0.75f);
        velocity.x *= magnitude * (1f - Mathf.Abs(offset));
        velocity.y = magnitude * offset;

        lastHit.GetComponent<PongBall>().CurrentVelocity = velocity;
    }
}
