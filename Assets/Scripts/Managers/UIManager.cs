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
    [SerializeField]
    private List<GameObject> _sheildUi = new List<GameObject>();
    [SerializeField]
    private Text _ammoCountText;
    [SerializeField]
    private Image _thrusterBar;    

    private void Start()
    {
        _livesUI.sprite = _livesSprite[3];
        GameOverUI.SetActive(false);
        ammouCountUi();
    }

    public float ThrusterLevel()
    {
        return _thrusterBar.fillAmount;
    }

    public void UpdateThrusterBar(bool isRunning)
    {
        if (isRunning == true)
            _thrusterBar.fillAmount -= 0.005f;
        else
        {
            _thrusterBar.fillAmount += 0.01f;
        }           
    }

    public void ammouCountUi()
    {
        _ammoCountText.text = "Ammou Count : " + PoolingManager.Instance.bulletsAmmount.ToString();
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

    public void SheildUIUpdate(int sheildLevel)
    {
        switch(sheildLevel)
        {
            case 0:
                _sheildUi[0].SetActive(false);
                break;
            case 1:
                _sheildUi[2].SetActive(false);
                break;
            case 2:
                _sheildUi[3].SetActive(false);
                break;
            case 3:
                activeAllSheildUI();
                break;
        }
    }

    private void activeAllSheildUI()
    {
        foreach(var s in _sheildUi)
        {
            if(s.activeInHierarchy == false)
            s.SetActive(true);
        }
    }
}
