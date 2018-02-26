using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Hunter.Character
{
    public class FloatingText : MonoBehaviour
    {
        public Animator anim;
        private Text damageText;

        void Start()
        {
            var clipInfo = anim.GetCurrentAnimatorClipInfo(0);
            var clipLength = clipInfo[0].clip.length;
            Destroy(gameObject, clipLength);
            damageText = anim.GetComponent<Text>();
        }

        public void SetDamageText(string damage)
        {
            anim.GetComponent<Text>().text = damage;
        }

        public void SetTextColor(ElementType type)
        {
            if(type is Elements.Fire)
            {
                Debug.Log("why");
                Debug.Log(anim.GetComponent<Text>().color);
            }
        }
    }
}
