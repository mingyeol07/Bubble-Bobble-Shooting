using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// player - soul, bullet, levelUp
public class LevelUpManager : MonoBehaviour
{
    [Header("SoulBox")]
    [SerializeField] private Image img_soulBox;
    [SerializeField] private Sprite[] img_soulBoxLevel;
    private int soulIndex;
    private int soulLevel;

    [Header("Player")]
    [SerializeField] private Sprite[] img_playerLevel;
    [SerializeField] private SpriteRenderer playerSprite;

    [Header("Bullet")]
    [SerializeField] private GameObject[] obj_bulletLevel;

    // test
    [SerializeField] private GameObject ghostScreen;

    void Start()
    {
        soulLevel = 0;
        img_soulBox.sprite = img_soulBoxLevel[0];
        playerSprite.sprite = img_playerLevel[0];
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1)) GetSoul();

        img_soulBox.fillAmount = soulIndex / 10;
    }

    public void GetSoul()
    {
        soulIndex++;
        if (soulIndex >= 10)
        {
            soulIndex = 0;
            soulLevel++;
            img_soulBox.sprite = img_soulBoxLevel[soulLevel];
            playerSprite.sprite = img_playerLevel[soulLevel];
        }
    }
}
