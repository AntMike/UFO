﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour {

    [Header("Player health")]
    public Image HealthBar;
    public Image DeathPanel;
    public Image YouDied;

    /// <summary>
    /// Fill player HP bar
    /// </summary>
    /// <param name="currentHP">Current HP</param>
    /// <param name="maxHP">Max awailable HP</param>
    public void ShowPlayerHealth(float currentHP, float maxHP)
    {
        HealthBar.fillAmount = (currentHP / maxHP);
    }

    /// <summary>
    /// Death screen
    /// </summary>
    public void PlayerDeath()
    {
        DeathPanel.gameObject.SetActive(true);
        StartCoroutine(DeathScreen());
    }

    /// <summary>
    /// Death screen timer
    /// </summary>
    private float _maxAlpha = 0.9f;
    private float _middleAlpha = 0.6f;
    private IEnumerator DeathScreen()
    {
        while (YouDied.color.a < _maxAlpha)
        {

            DeathPanel.color = new Color(1, 1, 1, Mathf.Lerp(DeathPanel.color.a, 1, Time.deltaTime));
            if (DeathPanel.color.a > _middleAlpha)
            {
                YouDied.color = new Color(1, 1, 1, Mathf.Lerp(YouDied.color.a, 1, Time.deltaTime));
            }
            yield return new WaitForFixedUpdate();
        }

        yield return new WaitForSeconds(4);
        UnityEngine.SceneManagement.SceneManager.LoadScene("main");

    }
}
