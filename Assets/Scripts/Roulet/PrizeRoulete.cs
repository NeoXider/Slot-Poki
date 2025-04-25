using Neoxider.Shop;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PrizeRoulete : MonoBehaviour
{
    public Image animalImage;
    public DailyClick animal;
    public TMP_Text textWin;

    public void GetPrize(int id)
    {
        print("Get " + id);

        switch (id)
        {
            case 0:
                Money.Instance.Add(20);
                textWin.text = "+20$";
                break;

            case 1:
                Stats.Instance.AddFreeSpin(5);
                textWin.text = "+5 spins";
                break;

            case 2:
                Money.Instance.Add(30);
                textWin.text = "+30$";
                break;

            case 3:
                Stats.Instance.AddFreeSpin(10);
                textWin.text = "+10 spins";
                break;

            case 4:
                Money.Instance.Add(20);
                textWin.text = "+20$";
                break;

            case 5:
                Money.Instance.Add(50);
                textWin.text = "+50$";
                break;

            case 6:
                animal.Open();
                textWin.text = "You got: " + animal.Name + "!";
                break;

            case 7:
                Stats.Instance.AddFreeSpin(5);
                textWin.text = "+5 spins";
                break;

            default:
                break;
        }

        
    }

    private void OnEnable()
    {
        DailyClick[] animals = Resources.FindObjectsOfTypeAll<DailyClick>();
        int[] prices = Stats.Instance.data.animalClick;
        List<DailyClick> animalsFree = animals
    .Where(animal => prices[animal.ID] > 0)
    .ToList();


        animal = animalsFree[Random.Range(0, animalsFree.Count)];
        if (animalImage != null)
        {
            animalImage.sprite = animal.Sprite;
            animalImage.SetNativeSize();
        }
    }
}
