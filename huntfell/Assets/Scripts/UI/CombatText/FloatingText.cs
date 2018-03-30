using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Hunter.Character
{
    public class FloatingText : MonoBehaviour
    {
        public Animator anim;
        //private Text damageText;

        void Start()
        {
            var clipInfo = anim.GetCurrentAnimatorClipInfo(0);
            var clipLength = clipInfo[0].clip.length;
            Destroy(gameObject, clipLength);
            //damageText = anim.GetComponent<Text>();
        }

        public void SetDamageText(string damage)
        {
            anim.GetComponent<Text>().text = damage;
        }

        public void SetTextColor(Element type)
        {
            if(type is Element.Fire)
            {
                var colorRed = Color.red;
                anim.GetComponent<Text>().color = colorRed;
            }
            else if(type is Element.Ice)
            {
                var colorLightBlue = Color.cyan;
                anim.GetComponent<Text>().color = colorLightBlue;
            }
            else if(type is Element.Lightning)
            {
                var colorYellow = Color.yellow;
                anim.GetComponent<Text>().color = colorYellow;
            }
        }
    }
}
