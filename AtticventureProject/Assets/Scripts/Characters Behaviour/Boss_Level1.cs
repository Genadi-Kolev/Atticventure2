using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(HealthManager), typeof(Rigidbody2D), typeof(BoxCollider2D))]
public class Boss_Level1 : MonoBehaviour, IShooter
{
    private Rigidbody2D rb2D;

    // [SerializeField] List<GameObject> shields = new List<GameObject>();
    // [SerializeField] private int shieldOrbitSpeed = 20;
    [SerializeField] private int speed = 4;
    [SerializeField] private int damage = 10;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private int bulletSpeed = 20;

    [SerializeField] private AudioSource shotSFX;

    private Vector2 direction;

    bool canMove;
    bool Move { get => Move;
        set {
            if (value) {
                canMove = true;
                StartCoroutine(Movement());
            } else {
                canMove = false;
                StopCoroutine(Movement());
            } 
        }
    }
    
    private void Awake() {
        rb2D = GetComponent<Rigidbody2D>();
        direction = UnityEngine.Random.Range(0,2) == 1 ? Vector2.right : Vector2.left;
    }

    private void Start() {
        Move = true;
    }

    // void Update()
    // {
    //     for (int i = shields.Count - 1; i >= 0 ; i--)
    //         if (!shields[i]) shields.Remove(shields[i]);
    // }

    IEnumerator Movement() {
        while (canMove) {
            rb2D.MovePosition(rb2D.position + speed * Time.fixedDeltaTime * direction);
            // foreach (var shield in shields)
            //     shield.transform.RotateAround(transform.position, Vector3.forward, shieldOrbitSpeed * Time.deltaTime);
            
            yield return new WaitForFixedUpdate();
        }
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.TryGetComponent<BulletScript>(out BulletScript bullet)) return;
        if (other.gameObject.TryGetComponent<PlayerMove>(out PlayerMove player)) return;

        var colPosition = (Vector2)other.gameObject.transform.localPosition;
        var bosPosition = transform.localPosition;

        if (colPosition.y == 0) direction = new Vector2(0, bosPosition.y > 0 ? -1 : 1);
        if (colPosition.x == 0) direction = new Vector2(bosPosition.x > 0 ? -1 : 1, 0);
    }

    public void Shoot() {
        var pattern = UnityEngine.Random.Range(0,2);
        switch (pattern) {
            case 0:
                CrossPattern();
                break;
            case 1:
                DiagonalPattern();
                break;
        }
    }

    private void CrossPattern() {
        GameObject bulletU = Instantiate(bulletPrefab, transform.position, transform.rotation);
        bulletU.GetComponent<BulletScript>().damage = damage;
        bulletU.GetComponent<Rigidbody2D>().velocity = bulletSpeed * new Vector2(0,1).normalized;

        GameObject bulletD = Instantiate(bulletPrefab, transform.position, transform.rotation);
        bulletD.GetComponent<BulletScript>().damage = damage;
        bulletD.GetComponent<Rigidbody2D>().velocity = bulletSpeed * new Vector2(0, -1).normalized;
        
        GameObject bulletL = Instantiate(bulletPrefab, transform.position, transform.rotation);
        bulletL.GetComponent<BulletScript>().damage = damage;
        bulletL.GetComponent<Rigidbody2D>().velocity = bulletSpeed * new Vector2(-1, 0).normalized;
        
        GameObject bulletR = Instantiate(bulletPrefab, transform.position, transform.rotation);
        bulletR.GetComponent<BulletScript>().damage = damage;
        bulletR.GetComponent<Rigidbody2D>().velocity = bulletSpeed * new Vector2(1, 0).normalized;
        
        shotSFX.Play();
    }

    private void DiagonalPattern() {
        GameObject bulletUR = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        bulletUR.GetComponent<BulletScript>().damage = damage;
        bulletUR.GetComponent<Rigidbody2D>().velocity = bulletSpeed * new Vector2(1,1).normalized;
        
        GameObject bulletUL = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        bulletUL.GetComponent<BulletScript>().damage = damage;
        bulletUL.GetComponent<Rigidbody2D>().velocity = bulletSpeed * new Vector2(-1, 1).normalized;
        
        GameObject bulletDR = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        bulletDR.GetComponent<BulletScript>().damage = damage;
        bulletDR.GetComponent<Rigidbody2D>().velocity = bulletSpeed * new Vector2(1, -1).normalized;
        
        GameObject bulletDL = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        bulletDL.GetComponent<BulletScript>().damage = damage;
        bulletDL.GetComponent<Rigidbody2D>().velocity = bulletSpeed * new Vector2(-1, -1).normalized;
        
        shotSFX.Play();
    }
}
