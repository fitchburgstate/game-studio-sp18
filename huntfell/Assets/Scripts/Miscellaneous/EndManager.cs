using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hunter {
    public class EndManager : MonoBehaviour {

        void Start () {
            StartCoroutine(WaitThenLoad());
        }

        private IEnumerator WaitThenLoad ()
        {
            yield return new WaitForSeconds(8);
            GameManager.instance.LoadNewScene("UI_Title_Menu", false);
        }
    }
}
