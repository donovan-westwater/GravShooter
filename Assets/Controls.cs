using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controls : MonoBehaviour
{
    //Movement Variables
    Vector3 grav = new Vector3(0, -1f, 0);
    public Vector3 velo = new Vector2(0, 0);
    public float speed = 5f;

    //Aiming Variables
    Vector2 aimDir;
    Transform aimRet;
    // Start is called before the first frame update
    void Start()
    {
        aimRet = this.transform.GetChild(0);
        aimDir = aimRet.transform.position - this.transform.position;
    }

    // Update is called once per frame
    //TODO: Rework the aim system to use 8 directional aim. turn off sprites except the direction aimed
    //EX:
    // \ | /
    // -   -
    // / | \
    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Vector3 veloIN = new Vector3(h, 0, 0);
        if (!Input.anyKey) velo = new Vector3(0, 0, 0);
        if(v < 0) grav = new Vector3(0, -1f, 0);
        else if(v > 0) grav = new Vector3(0, 1f, 0);
        velo = ((veloIN.normalized+ grav.normalized)*speed);
        this.transform.Translate(velo * Time.deltaTime);

        //Aiming Contorls
        Vector3 mouseVect = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x - transform.position.x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y - transform.position.y);
        Vector3 currentDir = new Vector3(aimRet.transform.position.x - transform.position.x, aimRet.transform.position.y - transform.position.y);
        float angleTemp = Mathf.Acos(Vector3.Dot(currentDir, mouseVect) / (currentDir.magnitude * mouseVect.magnitude)); //Is in Radians!
        if (Vector3.Cross(mouseVect.normalized, currentDir.normalized).z < 0 && angleTemp > Mathf.Abs(0.01f))
        {
            aimRet.transform.RotateAround(transform.position, new Vector3(0, 0, 1), angleTemp*Mathf.Rad2Deg);
        }
        else if (Vector3.Cross(mouseVect.normalized, currentDir.normalized).z > 0 && angleTemp > Mathf.Abs(0.01f))
        {
            aimRet.transform.RotateAround(transform.position, new Vector3(0, 0, 1), -angleTemp*Mathf.Rad2Deg);
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        ContactPoint2D[] points = new ContactPoint2D[500];
        collision.GetContacts(points);
        ContactPoint2D? prevP = null;
        foreach (ContactPoint2D p in points)
        {
            if (p.collider == null) continue;
            if (prevP != null && Mathf.Abs(prevP.Value.point.y - p.point.y) > 0.1f)
            {
                return;
            }
            prevP = p;
        }
        //velo = new Vector3(0, 0, 0);
        grav = new Vector3(0, 0, 0);
    }
}
