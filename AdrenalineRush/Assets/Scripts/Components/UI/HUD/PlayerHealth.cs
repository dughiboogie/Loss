using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private PlayerCombat playerCombat;

    private List<HealthSlot> healthSlots;

    public GameObject healthSlotPrefab;

    private void Awake()
    {
        healthSlots = new List<HealthSlot>();

        /*
         * Instantiate the maximum number of health slots and add each HealthSlot script to the healthSlots list
         */
        for(int i = 0; i < playerCombat.maxHealth; i++) {
            GameObject currentHealthSlot = Instantiate(healthSlotPrefab, transform);
            healthSlots.Add(currentHealthSlot.GetComponent<HealthSlot>());
        }
    }

    public void RemoveHealth()
    {
        for(int i = healthSlots.Count - 1; i >= 0; i--) {
            if(healthSlots[i].isFull) {
                healthSlots[i].Empty();
                return;
            }
        }
    }

    public void AddHealth()
    {
        for(int i = 0; i <= healthSlots.Count - 1; i++) {
            if(!healthSlots[i].isFull) {
                healthSlots[i].Fill();
                return;
            }
        }
    }
}
