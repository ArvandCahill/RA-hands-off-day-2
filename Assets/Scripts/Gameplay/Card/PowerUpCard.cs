using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PowerUpCard : Card
{
    [Header("UI")]
    [SerializeField] private Image cardImage;
    [SerializeField] private TextMeshProUGUI cardTitle;
    [SerializeField] private TextMeshProUGUI cardDescription;
    [SerializeField] private Outline outline;
    [SerializeField] private Color highlightColor = Color.yellow;

    public PowerUpData PowerUpData { get; private set; }

    public void Initialize(PowerUpData data)
    {
        PowerUpData = data;

        cardImage.sprite = data.cardImage;
        cardTitle.text = data.cardTitle;
        cardDescription.text = data.cardDescriptionEn;

        transform.localScale = Vector3.one * 0.7f;
    }

    public void Highlight()
    {
        outline.effectColor = highlightColor;
    }

    public override void OnClick()
    {
        CardManager.instance.SelectCard(this);
    }

    public override IEnumerator OnCardSelected()
    {
        if (PowerUpData != null && PlayerStats.instance != null)
            PowerUpData.ApplyEffect(PlayerStats.instance);

        yield break;
    }

    public void FlipAnimation()
    {
        transform.DORotate(new Vector3(0, 180, 0), 0.5f)
            .SetEase(Ease.OutBack)
            .SetUpdate(true)
            .OnComplete(() =>
            {
                transform.DORotate(Vector3.zero, 0.5f)
                    .SetEase(Ease.OutBack)
                    .SetUpdate(true);
            });
    }
}