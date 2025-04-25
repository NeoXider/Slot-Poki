using UnityEngine;

[CreateAssetMenu(fileName = "BetsDafualt", menuName = "Slot/Bets")]
public class BetsData : ScriptableObject
{
    [SerializeField] private int[] _bets = { 10, 20, 50, 100, 200, 500, 1000 };

    public int[] bets => _bets;
}

