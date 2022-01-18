using System;
using UnityEngine;

[Serializable]
public class Level : MonoBehaviour
{
    [SerializeField]
    private CardBundleData[] _dataBundles;

    public CardBundleData[] DataBundles => _dataBundles;
}
