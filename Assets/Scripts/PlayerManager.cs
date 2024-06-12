using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] List<GameObject> playerPrefabs;
    ScoreKeeper scoreKeeper;
    PlayerShip player;
    PlayerShip player2;
    PlayerShip player3;
    UIDisplay uIDisplay;
    Slider healthSlider;
    AudioPlayer audioPlayer;
    int currentLevel = 1;
    bool isLeveledUp = false;

    void Awake()
    {
        scoreKeeper = FindObjectOfType<ScoreKeeper>();
        player = FindObjectOfType<PlayerShip>();
        uIDisplay = FindObjectOfType<UIDisplay>();
        healthSlider = FindObjectOfType<Slider>();
        audioPlayer = FindObjectOfType<AudioPlayer>();
    }

    void Update()
    {
        UpgradePlayerLevel(currentLevel);
    }

    void UpgradePlayerLevel(int level)
    {
        if (scoreKeeper.GetScore() == 500 && !isLeveledUp)
        {
            isLeveledUp = true;
            Destroy(player.gameObject);
            Instantiate(playerPrefabs[level],
                        player.transform.position,
                            Quaternion.identity);
            player2 = FindObjectOfType<PlayerShip>();
            NewPlayerAnimation(player2);
            audioPlayer.PlayLevelUpClip();
            UpdateUI(player2);
            currentLevel++;
        }
        else if (scoreKeeper.GetScore() > 500 && isLeveledUp)
        {
            isLeveledUp = false;
        }
        else if (scoreKeeper.GetScore() == 1000 & !isLeveledUp)
        {
            isLeveledUp = true;
            Destroy(player2.gameObject);
            Instantiate(playerPrefabs[level],
                        player2.transform.position,
                            Quaternion.identity);
            player3 = FindObjectOfType<PlayerShip>();
            NewPlayerAnimation(player3);
            audioPlayer.PlayLevelUpClip();
            UpdateUI(player3);
            currentLevel++;
        }
    }

    void UpdateUI(PlayerShip _player)
    {
        uIDisplay.UpdatePlayerHealth(_player.GetComponent<Health>());
        healthSlider.maxValue = _player.GetComponent<Health>().GetHealth();
    }

    void NewPlayerAnimation(PlayerShip _player)
    {
        _player.GetComponent<Animator>().SetTrigger("isLevelUp");
    }
}
