using UnityEngine;
using UnityEngine.UI;

public class CurrencyDisplay : MonoBehaviour
{
    public Text currencyText;
    public Text powerText;

    void Update()
    {
        currencyText.text = "Currency: " + GameManager.Instance.playerCurrency;
        powerText.text = "Total Power: " + GameManager.Instance.totalPowerGenerated;
    }
}
