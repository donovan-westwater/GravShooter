using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlsProto : MonoBehaviour
{
    // Start is called before the first frame update
    Rigidbody2D phy;
    public Vector3 velo = new Vector2(0,0);
    public float speed = 5f;
    void Start()
    {
        phy = this.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    //Fix gravity problem!!!
    private void Update()
    {
       // float h = Input.GetAxis("Horizontal");
       // velo = new Vector3(h, 0, 0);
        //velo = velo.normalized * speed * Time.deltaTime;
        //phy.MovePosition(phy.transform.position + velo);
        //phy.AddForce(velo);
        if (!Input.anyKey)
        {
            velo = new Vector3(0, 0, 0);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            phy.gravityScale = -phy.gravityScale;
        }
        //if (Mathf.Abs(velo.magnitude) > 8f) velo = velo.normalized * 8f;
       // this.transform.position += velo;
    }
    private void FixedUpdate()
    {
        float h = Input.GetAxis("Horizontal");
        velo = new Vector3(h, 0, 0);
        velo = velo.normalized * speed;
        //if (Mathf.Abs(velo.magnitude) > 8f) velo = velo.normalized * 8f;
        phy.AddForce(velo);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        ContactPoint2D[] points = new ContactPoint2D[500];
        collision.GetContacts(points);
        ContactPoint2D? prevP = null;
        foreach (ContactPoint2D p in points)
        {
            if (prevP != null && prevP.Value.point.y != p.point.y)
            {
                velo = new Vector3(0, 0, 0);
                return;
            }
            prevP = p;
        }
    }
}

