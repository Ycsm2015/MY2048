using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewGameButtonController : MonoBehaviour
{
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
                if (hit.collider.gameObject.name == "ReStartBtn")
                    GameController.GetInstance().NewGame();
        }
    }
}
