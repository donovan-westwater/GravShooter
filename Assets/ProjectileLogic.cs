using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileLogic : MonoBehaviour
{
    // Start is called before the first frame update
    Vector2 ogSize;
    SpriteRenderer sprite;
    public float charge = 0;
    public Vector2 velo = new Vector2(0,0);
    public bool fired = false;
    float timer = 5;
    void Start()
    {
        sprite = this.GetComponent<SpriteRenderer>();
        //ogSize = sprite.size;
        ogSize = this.transform.localScale;
    }

    // Update is called once per frame
    //TODO: Have a velocity system where projectile translates based on velocity given to it
    void Update()
    {
        this.transform.localScale = ogSize * (1 + charge);
        if (fired)
        {
            this.transform.Translate(velo.normalized*charge*10* Time.deltaTime);
            //Add timer to destory self
            timer -= 1 * Time.deltaTime;
            if (timer < 0) Destroy(this.gameObject);
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!fired) return;
        if(collision.gameObject.tag == "Projectile")
        {
            float otherC = collision.gameObject.GetComponent<ProjectileLogic>().charge;
            collision.gameObject.GetComponent<ProjectileLogic>().charge = otherC - charge;
            //this.charge = charge - otherC;
            if(charge < 0)
            {
                Destroy(this.gameObject);
            }
        }
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Player2")
        {
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.tag == "Wall") Destroy(this.gameObject);
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (!fired) return;
        if (collision.gameObject.tag == "Projectile")
        {
            float otherC = collision.gameObject.GetComponent<ProjectileLogic>().charge;
            collision.gameObject.GetComponent<ProjectileLogic>().charge = otherC - charge;
            //this.charge = charge - otherC;
            if (charge < 0)
            {
                Destroy(this.gameObject);
            }
        }
        if (collision.gameObject.tag == "Player")
        {
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.tag == "Wall") Destroy(this.gameObject);
    }
}
