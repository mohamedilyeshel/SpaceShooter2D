using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.PostProcessing;

public class GameManager : MonoSinglton<GameManager>
{
    public int score = 0;
    public int health = 3;
    private bool _isDead = false;
    [SerializeField]
    private List<GameObject> _playerHurt = new List<GameObject>();
    private bool _isChangeTimeDone = false;
    [SerializeField]
    private int _scoreChange;
    private Animator _camShake;
    public PostProcessVolume postVolume;
    private Vignette vignette;

    private void Start()
    {
        _camShake = Camera.main.GetComponent<Animator>();
    }

    private void Update()
    {
        if (health == 0 && _isDead == false)
        {
            playerDie();
        }

        if(_isDead == true)
        {
            reloadScene();
        }

        if(score >= _scoreChange && _isChangeTimeDone == false)
        {
            _scoreChange += 50;
            SpawnManager.Instance.ChangeSpawnTime();
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    public void StartNegativeEffect(bool isDone)
    {
        postVolume.profile.TryGetSettings(out vignette);
        if (isDone == false)
        {
            vignette.intensity.value = 0.8f;
        }
        else
        {
            vignette.intensity.value = 0.412f;
        }
    }

    public void EnableCameraShake()
    {
        _camShake.SetTrigger("ShakeIT");
    }

    public void ChangeTimeBool()
    {
        _isChangeTimeDone = true;
    }

    private void reloadScene()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    private void playerDie()
    {
        _isDead = true;
        UIManager.Instance.showGameOver();
        SpawnManager.Instance.IsPlayerDead();
        Destroy(GameObject.Find("Player"));
        AudioManager.Instance.ExplosionPlay();
    }

    public void increaseScore(int scoreToAdd)
    {
        score += scoreToAdd;
        UIManager.Instance.scoreTextUpdate();
    }

    public void PlayerHurt()
    {
        bool isDone = false;
        do
        {
            int i = Random.Range(0, 2);
            if (_playerHurt[i].activeInHierarchy == false)
            {
                _playerHurt[i].SetActive(true);
                break;
            }
            else if(health == 0)
            {
                isDone = true;
            }
        } while (isDone == false);
    }

    private void PlayerRecover()
    {
        bool isDone = false;
        do
        {
            int i = Random.Range(0, 2);
            if (_playerHurt[i].activeInHierarchy == true)
            {
                _playerHurt[i].SetActive(false);
                break;
            }
        } while (isDone == false);
    }

    public void AddHealth()
    {
        if (health < 3)
        {
            PlayerRecover();
            health++;
        }
        else
            return;
    }
}
