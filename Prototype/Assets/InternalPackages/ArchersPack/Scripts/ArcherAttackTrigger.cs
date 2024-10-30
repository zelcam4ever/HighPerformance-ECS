using UnityEngine;
using System.Collections;

public class ArcherAttackTrigger : MonoBehaviour
{
    public GameObject gArcher;

    void Start()
    {
        
    }

    void Update()
    {

    }

    private void OnEnable()
    {
        gArcher.GetComponent<Archer>().gArcherAttack();
    }

}
