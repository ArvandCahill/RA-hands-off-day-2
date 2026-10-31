using System.Collections;
using UnityEngine;

public abstract class Card : MonoBehaviour
{
    public abstract IEnumerator OnCardSelected();
    public virtual void OnClick() { }
}
