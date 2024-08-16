using UnityEngine;

public class ClickSFX : MonoBehaviour
{
    public void Button_ClickSFX() => AudioManager.Instance.PlaySFX("Click");
}
