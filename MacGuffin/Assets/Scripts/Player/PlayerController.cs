﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f; // Karakterin sureti
    public float jumpForce = 7f; // Hoppanma Gucu
    private bool isGrounded = true; // Zeminde olup olmadığını kontrol edir
    private Rigidbody2D rb;
    private Animator animator;

    public float breathDuration = 10.0f; // Nefes alma vaxti (10 saniye)
    private float breathTimer = 0.0f;
    // Karakterin nefes alıp almadığını kontrol etmek üçün


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Karakterin yatay hareketini al
        float moveX = 0.0f;

        if (Input.GetKey("a"))
        {
            // "A" tuşuna basıldığında sola bakıyor ve yürüyor
            moveX = -1.0f;
            transform.localScale = new Vector3(-1, 1, 1);
            animator.SetBool("isWalking", true);
        }
        else if (Input.GetKey("d"))
        {
            // "D" tuşuna basıldığında sağa bakıyor ve yürüyor
            moveX = 1.0f;
            transform.localScale = new Vector3(1, 1, 1);
            animator.SetBool("isWalking", true);
        }
        else
        {
            // Hiçbir tuşa basılmadığında yürüme animasyonunu kapatır
            animator.SetBool("isWalking", false);
        }

        // Karakteri yatay yönde hareket ettir
        Vector2 movement = new Vector2(moveX * moveSpeed, GetComponent<Rigidbody2D>().velocity.y);
        GetComponent<Rigidbody2D>().velocity = movement;

        // Hppanmaq
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            isGrounded = false;
        }

        animator.SetBool("isGrounded", isGrounded);
        animator.SetFloat("moveSpeed", Mathf.Abs(moveX));


        // Animator Controller'daki QPressed şərtini true olarak ayarlayarak geç
        animator.SetBool("QPressed", true);


        if (Input.GetKeyDown(KeyCode.Q))
        {
            // Q tuşuna basıldı, nefes almayı yeniden başlat
            breathTimer = 0.0f;
            Debug.Log("Q isliyir");
            // Animator Controller'daki QPressed şartını true olarak ayarlayarak geç
            animator.SetBool("QPressed", true);
        }
        else
        {
            // Q tuşuna basılmadı, zamanı artır
            breathTimer += Time.deltaTime;

            // Məlum bir vaxt boyunca nefes alınmazsa animasyonu oynat
            if (breathTimer >= breathDuration)
            {
                animator.SetBool("NefesKesilmesi", true); // "NefesKesilmesi" trigger'ını etkinleştir

                // Animator Controller'daki QPressed şartını false olarak ayarlayarak geçişi ləğv et
                animator.SetBool("QPressed", false);
            }

            // Kalan vaxti Console'a yazdır
            int remainingTime = Mathf.CeilToInt(breathDuration - breathTimer);
            Debug.Log("Kalan Vaxt: " + remainingTime.ToString("0.00") + " saniye");
        }
    }

    // Yerde olduğumuza dair kontrol
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }
}
