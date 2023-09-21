using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Utility
{
    public class TeamMemberScroller : MonoBehaviour
    {
        [SerializeField] private string[] teamMembers;
        private TMP_Text _text;

        private void Start()
        {
            _text = GetComponent<TMP_Text>();
            StartCoroutine(ScrollTeamMembers());
        }

        private IEnumerator ScrollTeamMembers()
        {
            int i = 0;
            while (true)
            {
                // Fade out
                while (_text.color.a > 0)
                {
                    Color currentColor = _text.color;
                    currentColor.a -= Time.deltaTime * 2; // Adjust the speed by changing the multiplier
                    _text.color = currentColor;
                    yield return null;
                }

                // Change the text
                _text.text = teamMembers[i++ % teamMembers.Length];

                // Fade in
                while (_text.color.a < 1)
                {
                    Color currentColor = _text.color;
                    currentColor.a += Time.deltaTime * 2; // Adjust the speed by changing the multiplier
                    _text.color = currentColor;
                    yield return null;
                }

                yield return new WaitForSeconds(3.0f);
            }
        }
    }
}
