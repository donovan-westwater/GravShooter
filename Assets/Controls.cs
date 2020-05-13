using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class Controls : MonoBehaviour
{
    //Movement Variables
    Vector3 grav = new Vector3(0, -1f, 0);
    bool down = false;
    public Vector3 velo = new Vector2(0, 0);
    public float speed = 5f;
    [SerializeField] string[] inputs = new string[5];
    float charge = 0;
    public GameObject proj;
    GameObject cur;
    //Aiming Variables
    Vector2 aimDir;
    Transform aimRet;
    GameObject manager; //new
    // Start is called before the first frame update
    void Start()
    {
        aimRet = this.transform.GetChild(0);
        aimDir = aimRet.transform.position - this.transform.position;
        manager = GameObject.Find("Game Manager"); //new
    }

    // Update is called once per frame
    //TODO: Change gravity to work via button press
    void Update()
    {
        if (manager.GetComponent<Manager>().outGame == true) return; //new
        //Replace literal strings with inputs from the array
        float h = Input.GetAxisRaw(inputs[0]); //Horizontal
        //float v = Input.GetAxisRaw("Vertical");
        Vector3 veloIN = new Vector3(h, 0, 0);
        if (!Input.anyKey) velo = new Vector3(0, 0, 0);
        if (Input.GetButtonDown(inputs[1])) //jump
        {
            //grav = -grav;//grav = new Vector3(0, -1f, 0);
            if(down) grav = new Vector3(0, -1f, 0);
            else grav = new Vector3(0, 1f, 0);
            down = !down;
        }
        //else if (v > 0) grav = new Vector3(0, 1f, 0);
        velo = ((veloIN.normalized+ grav.normalized)*speed);
        this.transform.Translate(velo * Time.deltaTime);

        //Aiming Contorls [Covert the mouse controls into the input system]
        Vector3 mouseVect;
        if (this.gameObject.tag == "Player") { 
            mouseVect = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x - transform.position.x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y - transform.position.y);
        }
        else
        {
            //print("Controller X: " + Input.GetAxisRaw(inputs[3])+" Controller X: " + Input.GetAxisRaw(inputs[4]));
            Vector3 aimDir = new Vector3(aimRet.transform.position.x - transform.position.x, aimRet.transform.position.y - transform.position.y);
            Vector3 inputDir = new Vector3(Input.GetAxisRaw(inputs[3]), Input.GetAxisRaw(inputs[4]));
            mouseVect = aimDir + inputDir.normalized;
        }
        
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
        //TODO: Have projectile move when realeased
        if (Input.GetButton(inputs[2])) //Input.GetMouseButton(0) fire1
        {
            charge += 1 * Time.deltaTime;
            if (charge > 2) charge = 2;
            if (cur == null)
            {
                cur = Instantiate(proj, this.transform.GetChild(0).transform.position + currentDir.normalized * 2, proj.transform.rotation);
                Physics2D.IgnoreCollision(cur.GetComponent<Collider2D>(), this.GetComponent<Collider2D>());
            }
            cur.GetComponent<ProjectileLogic>().charge = charge;
            cur.transform.position = this.transform.GetChild(0).transform.position + currentDir.normalized * 0.5f;
        }
        else
        {
            if(cur != null)
            {
                cur.GetComponent<ProjectileLogic>().velo = currentDir.normalized;
                cur.GetComponent<ProjectileLogic>().fired = true;
                cur = null;
                charge = 0;
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag != "Wall") return;
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
        //Making sure the surface is actually "Under" the player
        if (points[0].point.y - this.transform.position.y < 0 && down) return;
        if (points[0].point.y - this.transform.position.y > 0 && !down) return;
        //velo = new Vector3(0, 0, 0);
        //this.transform.Translate(-velo * Time.deltaTime);
        grav = new Vector3(0, 0, 0);
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag != "Wall") return;
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
        if (down) grav = new Vector3(0, 1f, 0);
        else grav = new Vector3(0, -1f, 0);
    }

}

