using UnityEngine;
using DG.Tweening;
using TMPro;

public class CylinderMovement : MonoBehaviour
{
    private GameManager gameManagerScript;
    // Scale variables
    private Vector3 originalScale;
    private Vector3 scaleTo;
    // Move left variables
    private Vector3 originalPosition;
    private Vector3 moveTo;
    private Vector3 zeroPosition = new Vector3(0, 0, 0);
    private Vector3 onePosition = new Vector3(-0.4f, 0, 0);
    [SerializeField] float moveDuration = 0.5f;
    // Text Spawn
    public Canvas scoreTextPrefab;

    // Start is called before the first frame update
    void Start()
    {
        gameManagerScript = GameObject.Find("Game Manager").GetComponent<GameManager>();
        ScaleCylinder(gameManagerScript.scaleRate);
    }

    // Update is called once per frame
    void Update()
    {
        PressMouse();
    }

    void ScaleCylinder(float scaleRate)
    {
        originalScale = transform.localScale;
        scaleTo = originalScale * scaleRate;
        float scaleDuration = scaleRate / gameManagerScript.scaleTime; //scaleRate / 200;
        transform.DOScaleY(scaleTo.y, scaleDuration).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo).SetId("scale");
    }

    void MoveLeft()
    {
        if (gameManagerScript.isGameOn)
        {
            originalPosition = transform.position;
            moveTo = originalPosition + onePosition;
            transform.DOMoveX(moveTo.x, moveDuration).OnComplete(() =>
            {
                TriggerGameOver();
                SpawnAtOnePosition();
            });
            if (transform.position.x < -5)
            {
                Destroy(gameObject);
            }
        }

    }


    void PressMouse()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            DOTween.Pause("scale");
            if (transform.position.x == zeroPosition.x)
            {
                // Round Up the scale value to whole numbers
                float round = transform.localScale.y * 100;
                round = Mathf.Floor(round) / 100;
                transform.DOScale(new Vector3(1, round, 1), 0.01f);
                // Rescale cylinder if the value is near to previous cylinder
                if (transform.localScale.y > (gameManagerScript.scaleRate - 3) / 100)
                {
                    transform.DOScale(new Vector3(1, gameManagerScript.scaleRate / 100, 1), 0.01f);
                    gameManagerScript.powerUpCount++;
                    SpawnText();
                    if (gameManagerScript.powerUpCount > 2 && gameManagerScript.scaleRate <= 95)
                    {
                        gameManagerScript.scaleRate += 5;
                        gameManagerScript.powerUpCount = 1;
                        DeleteScoreText();
                        transform.DOScale(new Vector3(1, gameManagerScript.scaleRate / 100, 1), 0.5f).OnComplete(() =>
                        {
                            SpawnText();
                        });
                    }
                    else if (gameManagerScript.powerUpCount > 2 && gameManagerScript.scaleRate > 95)
                    {
                        gameManagerScript.scaleRate = 100;
                        gameManagerScript.powerUpCount = 1;
                        DeleteScoreText();
                        transform.DOScale(new Vector3(1, gameManagerScript.scaleRate / 100, 1), 0.5f).OnComplete(() =>
                        {
                            SpawnText();
                        });
                    }
                }
                else
                {
                    float round2 = transform.localScale.y * 100;
                    round2 = Mathf.Floor(round2);
                    gameManagerScript.scaleRate = round2;
                    gameManagerScript.powerUpCount = 1;
                    SpawnText();
                }
                gameManagerScript.UpdateScore(5);
            }
            MoveLeft();
        }
    }

    void SpawnAtOnePosition()
    {
        if (transform.position.x == onePosition.x)
        {
            gameManagerScript.CylinderSpawner();
        }
    }

    void SpawnText()
    {
        Canvas newCanvas =
        Instantiate(scoreTextPrefab, transform.position + new Vector3(0, transform.localScale.y * 2 + 0.1f, 0), scoreTextPrefab.transform.rotation) as Canvas;
        newCanvas.transform.SetParent(transform);
        newCanvas.GetComponentInChildren<TextMeshProUGUI>().text = gameManagerScript.scaleRate.ToString(); // round.ToString();
    }

    void DeleteScoreText()
    {
        Destroy(FindObjectOfType<Canvas>().gameObject);
    }

    void TriggerGameOver()
    {
        if (transform.localScale.y < 0.05f)
        {
            gameManagerScript.GameOver();
        }
    }
}
