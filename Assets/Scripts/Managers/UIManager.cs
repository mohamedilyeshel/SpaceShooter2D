using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoSinglton<UIManager>
{
    [SerializeField]
    private Text _scoreText;
    [SerializeField]
    private Image _livesUI;
    [SerializeField]
    private List<Sprite> _livesSprite = new List<Sprite>();
    [SerializeField]
    private GameObject GameOverUI;

    private void Start()
    {
        _livesUI.sprite = _livesSprite[3];
        GameOverUI.SetActive(false);
    }

    public void showGameOver()
    {
        GameOverUI.SetActive(true);
        StartCoroutine(gameOverFlick());
    }

    IEnumerator gameOverFlick()
    {
        bool isFlicker = false;
        while (true)
        {
            if (isFlicker == false)
            {
                GameOverUI.transform.GetChild(0).gameObject.SetActive(true);
                isFlicker = true;
            }
            else
            {
                GameOverUI.transform.GetChild(0).gameObject.SetActive(false);
                isFlicker = false;
            }
            yield return new WaitForSeconds(0.5f);
        }
    }

    public void scoreTextUpdate()
    {
        _scoreText.text = "Score : " + GameManager.Instance.score;
    }

    public void healthUIUpdate()
    {
        _livesUI.sprite = _livesSprite[GameManager.Instance.health];
    }
}
