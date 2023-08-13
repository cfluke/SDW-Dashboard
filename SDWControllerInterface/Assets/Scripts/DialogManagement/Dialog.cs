using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DialogManagement
{
    public class Dialog : MonoBehaviour
    {
         public void Destroy()
         {
             Destroy(gameObject);
         }
    }
}