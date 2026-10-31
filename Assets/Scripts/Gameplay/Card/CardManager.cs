using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class CardManager : MonoBehaviour
{
    public static CardManager instance { get; private set; }

    [Header("UI")]
    [SerializeField] private GameObject powerCardParent;
    [SerializeField] private List<PowerUpCard> powerUpCards;
    [SerializeField] private Button confirmButton;

    [Header("Power Up Data")]
    [SerializeField] private List<PowerUpData> allPowerUps;

    [Header("Animation")]
    [SerializeField] private float cardOpenDelay = 0.5f;
    [SerializeField] private float selectionScale = 0.77f;

    private readonly Queue<int> pendingLevelUps = new();

    private PowerUpCard selectedCard;
    private bool isSelecting;
    private bool isProcessingQueue;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    private void Start()
    {
        powerCardParent.SetActive(false);
    }


    public void RequestLevelUp()
    {
        pendingLevelUps.Enqueue(1);

        if (!isProcessingQueue)
            StartCoroutine(ProcessLevelUpQueue());
    }


    private IEnumerator ProcessLevelUpQueue()
    {
        isProcessingQueue = true;
        Time.timeScale = 0f;

        while (pendingLevelUps.Count > 0)
        {
            pendingLevelUps.Dequeue();

            yield return StartCoroutine(StartPowerUpCardSequence());
        }

        Time.timeScale = 1f;
        isProcessingQueue = false;
    }

    private IEnumerator StartPowerUpCardSequence()
    {
        isSelecting = true;
        selectedCard = null;

        yield return new WaitForSecondsRealtime(cardOpenDelay);

        AssignRandomPowerUps();

        powerCardParent.SetActive(true);

        foreach (PowerUpCard card in powerUpCards)
        {
            card.gameObject.SetActive(true);
            card.transform.localScale = Vector3.one * 0.7f;

            card.transform.DOScale(Vector3.one, 0.3f)
                .SetEase(Ease.OutBack)
                .SetUpdate(true);
        }

        isSelecting = false;

        while (powerCardParent.activeSelf)
            yield return null;
    }

    private void AssignRandomPowerUps()
    {
        bool isWeaponLevel = LevelManager.instance.CurrentLevel % 3 == 0;

        List<PowerUpData> statPool = allPowerUps
            .Where(data =>
                data != null &&
                data.powerUpType == PowerUpType.Stat &&
                data.CanAppear()
            )
            .ToList();

        List<PowerUpData> weaponPool = allPowerUps
            .Where(data =>
                data != null &&
                data.powerUpType == PowerUpType.Weapon &&
                data.CanAppear()
            )
            .ToList();

        List<PowerUpData> selectedPowerUps = new();

        if (isWeaponLevel && weaponPool.Count > 0)
        {
            int randomIndex = Random.Range(0, weaponPool.Count);
            selectedPowerUps.Add(weaponPool[randomIndex]);
        }

        int remainingSlots = powerUpCards.Count - selectedPowerUps.Count;

        for (int i = 0; i < remainingSlots && statPool.Count > 0; i++)
        {
            int randomIndex = Random.Range(0, statPool.Count);

            selectedPowerUps.Add(statPool[randomIndex]);
            statPool.RemoveAt(randomIndex);
        }

        if (selectedPowerUps.Count < powerUpCards.Count)
        {
            Debug.LogError("Jumlah PowerUpData tidak mencukupi!");
            return;
        }

        selectedPowerUps = selectedPowerUps
            .OrderBy(_ => Random.value)
            .ToList();

        for (int i = 0; i < powerUpCards.Count; i++)
        {
            powerUpCards[i].Initialize(selectedPowerUps[i]);
        }
    }

    public void SelectCard(PowerUpCard card)
    {
        if (isSelecting || selectedCard != null)
            return;

        if (!powerUpCards.Contains(card))
            return;

        StartCoroutine(ConfirmSelection(card));
    }

    private IEnumerator ConfirmSelection(PowerUpCard card)
    {
        isSelecting = true;
        selectedCard = card;

        yield return StartCoroutine(card.OnCardSelected());

        powerCardParent.SetActive(false);
        selectedCard = null;

        isSelecting = false;
    }

    private void OnDestroy()
    {
        if (instance == this)
            instance = null;
    }
}