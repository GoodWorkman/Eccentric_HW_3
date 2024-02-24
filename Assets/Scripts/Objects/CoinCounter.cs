using System;
using System.Collections.Generic;
using UnityEngine;

public class CoinCounter : MonoBehaviour
{
   public Action<int> OnCoinsCountChanged;
   public Action OnAllCoinsCollected;

   private readonly List<Coin> _coins = new();

   public int TotalCoins => _coins.Count;

   private void Start()
   {
      _coins.AddRange(FindObjectsOfType<Coin>());

      foreach (Coin coin in _coins)
      {
         coin.OnCoinCollected += CollectCoin;
      }
   }
   
   private void CollectCoin(Coin coin)
   {
      coin.OnCoinCollected -= CollectCoin;

      _coins.Remove(coin);
        
      OnCoinsCountChanged?.Invoke(_coins.Count);

      if (_coins.Count == 0)
      {
         OnAllCoinsCollected?.Invoke();
      }
   }
}
