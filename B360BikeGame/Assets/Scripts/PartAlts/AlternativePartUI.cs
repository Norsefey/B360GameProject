using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AlternativePartUI : MonoBehaviour
{
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private Image iconImage;
    [SerializeField] private TMP_Text priceText;
    [SerializeField] private TMP_Text durabilityText;
    public Button buyButton;
    public Button infoButton;

    private int alternativeIndex;
    public void Setup(PartStats alternative, int index)
    {
        alternativeIndex = index;

        if (nameText != null)
            nameText.text = alternative.partName;

        if (iconImage != null && alternative.icon != null)
            iconImage.sprite = alternative.icon;

        if (priceText != null)
            priceText.text = "$" + alternative.price.ToString();

        if (durabilityText != null)
            durabilityText.text = "Dur: " + alternative.durability.ToString("F0");
    }

    public int GetAlternativeIndex()
    {
        return alternativeIndex;
    }
}
