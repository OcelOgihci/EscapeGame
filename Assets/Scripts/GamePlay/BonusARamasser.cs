using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusARamasser : ObjetARamasser {

    public double ScoreAjoute = 10;
    public bool decay = false;
    public double decayPerSecond = 0.2;

    public override void ActionObjetRamasse()
    {
        if (CheckRessource() || IDRessourceNecessaire == -1)
        {
            base.ActionObjetRamasse();
            CommonDevTools.DEBUG("Bonus +10!");
            _MGR_Score.Instance.setScore(_MGR_Score.Instance.getScore() + ScoreAjoute);
            gameObject.SetActive(false);
            enabled = false;
        }
    }

    private void Update()
    {
        if (decay)
            ScoreAjoute -= decayPerSecond * Time.deltaTime;
    }
}
