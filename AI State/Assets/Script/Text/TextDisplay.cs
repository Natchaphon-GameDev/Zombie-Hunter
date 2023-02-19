using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextDisplay : MonoBehaviour
{
    private void OnEnable()
    {
        StartCoroutine(WaitForDisable());
    }

    private void Disable()
    {
        gameObject.SetActive(false);
    }

    private IEnumerator WaitForDisable()
    {
        yield return new WaitForSeconds(2f);
        Disable();
    }
}
