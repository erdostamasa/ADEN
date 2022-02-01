using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class DeathScreen : MonoBehaviour {
    [SerializeField] CanvasGroup cg;
    [SerializeField] float targetAlpha = 0.6f;
    [SerializeField] float duration = 1.5f;


    void Start() {
        DOTween.To(() => cg.alpha, (x) => cg.alpha = x, targetAlpha, duration);
    }
}