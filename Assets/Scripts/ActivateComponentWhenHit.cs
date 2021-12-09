/*Copyright Jeremy Blair 2021
License (Creative Commons Zero, CC0)
http://creativecommons.org/publicdomain/zero/1.0/

You may use these scripts in personal and commercial projects.
Credit would be nice but is not mandatory.
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System;

public class ActivateComponentWhenHit : MonoBehaviour
{
    public Component ComponentToActivateOnCollision;
    private FieldInfo fieldInfo;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        FieldInfo[] myFieldInfo;

        Type myType = ComponentToActivateOnCollision.GetType();
        // Get the type and fields of FieldInfoClass.
        myFieldInfo = myType.GetFields(BindingFlags.NonPublic | BindingFlags.Instance
            | BindingFlags.Public);

        for (int i = 0; i < myFieldInfo.Length; i++)
        {
            if (myFieldInfo[i].Name == "enabled")
            {
                fieldInfo = myFieldInfo[i];
            }
        }
    }

    public string TagOfObjectToCauseActivation = "Player";


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (enabled)
        {
            if (collision.gameObject.tag.ToLower() == TagOfObjectToCauseActivation.ToLower())
            {
                if (fieldInfo != null)
                    fieldInfo.SetValue(ComponentToActivateOnCollision, true);

                //if (ComponentToActivateOnCollision is Collider2D)
                //{
                //    ((Collider2D)ComponentToActivateOnCollision).enabled = true;
                //}
                //if (ComponentToActivateOnCollision is MonoBehaviour)
                //{
                //    ((MonoBehaviour)ComponentToActivateOnCollision).enabled = true;
                //}
                //if (ComponentToActivateOnCollision is Animator)
                //{
                //    ((Animator)ComponentToActivateOnCollision).enabled = true;
            }
        }
    }
}

