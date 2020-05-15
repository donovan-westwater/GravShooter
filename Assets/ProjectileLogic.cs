﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProjectileLogic : MonoBehaviour
{
    // Start is called before the first frame update
    Vector2 ogSize;
    SpriteRenderer sprite;
    public float charge = 0;
    public Vector2 velo = new Vector2(0,0);
    public bool fired = false;
    public Controls player; //NEW
    Vector2 grav;  //NEW
    float timer = 6;
    bool isDown; //NEW NON GRAVITY SYSTEM
    GameObject win;
    GameObject manager;
    void Start()
    {
        
        manager = GameObject.Find("Game Manager");
        win = manager.GetComponent<Manager>().win;
        sprite = this.GetComponent<SpriteRenderer>();
        ogSize = this.transform.localScale;
        isDown = player.down; //NEW NON GRAVITY SYSTEM
    }

    // Update is called once per frame
    //TODO: Have a velocity system where projectile translates based on velocity given to it
    void Update()
    {
        this.transform.localScale = ogSize * (1 + charge);
        if (fired)
        {
            if (isDown != player.down) velo.y = 0; //NEW NON GRAVITY SYSTEM was velo.y = -velo.y
            if (player.down) grav = new Vector2(0, 1f); //NEW
            else grav = new Vector2(0, -1f); //NEW
            this.transform.Translate(velo.normalized*charge*10* Time.deltaTime);
            velo += grav.normalized*1.5f*Time.deltaTime; //NEW Controls Gravity!
            //Add timer to destory self
            timer -= 1 * Time.deltaTime;
            if (timer < 0) Destroy(this.gameObject);
            isDown = player.down;
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Projectile")
        {
            float otherC = collision.gameObject.GetComponent<ProjectileLogic>().charge;
            this.charge = charge - Mathf.Abs(otherC);
            if(charge <= 0)
            {
                Destroy(this.gameObject);
            }
        }
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Player2")
        {
            if(collision.gameObject.tag == "Player")
            {
               
                win.SetActive(true);
                manager.GetComponent<Manager>().p2Wins += 1;
                win.transform.GetChild(0).GetComponent<Text>().text = "Player 2 has won this game! Press Enter key or A/X button to restart!\n";
                win.transform.GetChild(0).GetComponent<Text>().text += "Player 1 wins: " + manager.GetComponent<Manager>().p1Wins+"\n";
                win.transform.GetChild(0).GetComponent<Text>().text += "Player 2 wins: " + manager.GetComponent<Manager>().p2Wins+"\n";
                manager.GetComponent<Manager>().outGame = true;
                manager.GetComponent<Manager>().gameOver = true;
            }
            else
            {
                win.SetActive(true);
                manager.GetComponent<Manager>().p1Wins += 1;
                win.transform.GetChild(0).GetComponent<Text>().text = "Player 1 has won this game! Press the Enter key or A/X button to restart!\n";
                win.transform.GetChild(0).GetComponent<Text>().text += "Player 1 wins: " + manager.GetComponent<Manager>().p1Wins + "\n";
                win.transform.GetChild(0).GetComponent<Text>().text += "Player 2 wins: " + manager.GetComponent<Manager>().p2Wins + "\n";
                manager.GetComponent<Manager>().outGame = true;
                manager.GetComponent<Manager>().gameOver = true;
            }
            Destroy(collision.gameObject);
        }
        if (!fired) return;
        if (collision.gameObject.tag == "Wall")
        {
            
            ContactPoint2D point = collision.GetContact(0);
            //Checks to see if the projectile hit a wall horizontally or vertically so it can get the right normal for reflecting
            if(Mathf.Abs(point.point.y - transform.position.y) > Mathf.Abs(point.point.x - transform.position.x))
            {
                this.velo.y = -this.velo.y;
            }
            else if(Mathf.Abs(point.point.y - transform.position.y) < Mathf.Abs(point.point.x - transform.position.x))
            {
                this.velo.x = -this.velo.x;
            }
            else
            {
                this.velo = -this.velo;
            }
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
      

        if (collision.gameObject.tag == "Projectile")
        {
            float otherC = collision.gameObject.GetComponent<ProjectileLogic>().charge;
            
            this.charge = charge - Mathf.Abs(otherC);
            if (charge <= 0)
            {
                Destroy(this.gameObject);
            }
        }
        if (collision.gameObject.tag == "Player")
        {
            Destroy(collision.gameObject);
        }
        if (!fired) return;
        if (collision.gameObject.tag == "Wall") Destroy(this.gameObject);
    }
}
