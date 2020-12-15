using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnClickCard : MonoBehaviour
{
    public void UseCard()
    {
        GameManager.Instance.UseCard(name, this.gameObject);
    }
}
