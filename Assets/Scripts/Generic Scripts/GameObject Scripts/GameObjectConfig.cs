//———————————————————————–
// <copyright file=”GameObjectConfig.cs” game="KzzzZZZzzT!">
//     Copyright (c) Extreme Z7.  All rights reserved.
// </copyright>
//———————————————————————–
using UnityEngine;
using System;
using Common.Extensions;

[Serializable]
public class GameObjectConfig : System.Object
{
    //enums
    public enum ActionOnComplete
    {
        Nothing,
        SetToInactive,
        Destroy}
    ;

    public enum AttributeConfig
    {
        None,
        OffsetWithCustom,
        CopyNewTransformThenOffsetWithCustom,
        UseCustom}
    ;

    public enum PositionConfig
    {
        None,
        OffsetWithCustom,
        UseCustom}
    ;

    //fields
    [Header("Configuration")]
    [Tooltip("Optional. Leave this empty if you don't want to give the "
        + "object a new name")]
    public string newName;

    [Space(10)]
    public Transform newTransform;
    public Transform newParent;
    public Tag newTag;

    [Space(10)]
    public CustomPosition customPosition;
    public CustomScale customScale;
    public CustomRotation customRotation;
    public CustomVelocity customVelocity;

    [Header("Other Behavior(s)")]
    [Tooltip("What happens to the new Transform after the object is"
        + " configured?")]
    public ActionOnComplete transformOnCreate;

    /// <summary>Configures a given GameObject based on the configuration fields</summary>
    /// <param name="obj">The object to configure</param>
    ///
    public void Configure(GameObject obj)
    {
        SetNewName(obj);
        ConfigurePosition(obj);
        ConfigureRotation(obj); // Configure rotation should be before configure velocity
        ConfigureVelocity(obj);
        ConfigureScale(obj);
        SetNewTag(obj);
        SetNewParent(obj);
        DoFinalTransformAction();
    }

    void SetNewName(UnityEngine.Object obj)
    {
        //Keep the name if no new one was given
        if (newName != "")
        {
            obj.name = newName;
        }
    }

    /// <summary>
    /// Configures the position.
    /// </summary>
    /// <param name="obj">Object.</param>
    void ConfigurePosition(GameObject obj)
    {
        //Keep the position if no new Transform was given
        if (newTransform != null)
        {
            obj.transform.position = newTransform.position;
        }
        else
        {
            newTransform = obj.transform;
        }
            
        Vector3 customP = customPosition.custom;
        switch (customPosition.positionConfig)
        {
            case PositionConfig.OffsetWithCustom:
                {
                    Vector3 origPos = obj.transform.localPosition;
                    obj.transform.localPosition = new Vector3(
                        origPos.x + customP.x,
                        origPos.y + customP.y,
                        origPos.z + customP.z);
                }
                break;

            case PositionConfig.UseCustom:
                obj.transform.localPosition = new Vector3(
                    customP.x,
                    customP.y,
                    customP.z);
                break;
        }
    }

    /// <summary>
    /// Configures the rotation.
    /// </summary>
    /// <param name="obj">Object.</param>
    void ConfigureRotation(GameObject obj)
    {
        float customR = customRotation.custom;
        switch (customRotation.rotationConfig)
        {
            case AttributeConfig.None:
                obj.transform.rotation = Quaternion.identity;
                break;

            case AttributeConfig.OffsetWithCustom:
                obj.transform.localEulerAngles = new Vector3(
                    0, 0, obj.transform.localEulerAngles.z + customR);
                break;

            case AttributeConfig.CopyNewTransformThenOffsetWithCustom:
                obj.transform.localEulerAngles = new Vector3(
                    0, 0, newTransform.localEulerAngles.z + customR);
                break;

            case AttributeConfig.UseCustom:
                obj.transform.localEulerAngles = new Vector3(
                    0, 0, customR);
                break;
        }
    }

    /// <summary>
    /// Configures the velocity.
    /// </summary>
    /// <param name="obj">Object.</param>
    void ConfigureVelocity(GameObject obj)
    {
        if (customVelocity.velocityConfig != AttributeConfig.None)
        {
            if (obj.GetComponent<Rigidbody2D>() == null)
            {
                obj.AddComponent<Rigidbody2D>();
            }
        }
        else
        {
            return;
        }

        Vector2 customV = customVelocity.custom;
        switch (customVelocity.velocityConfig)
        {
            case AttributeConfig.OffsetWithCustom:
                {
                    Vector2 origV = obj.GetComponent<Rigidbody2D>().velocity;
                    obj.GetComponent<Rigidbody2D>().velocity = new Vector2(
                        origV.x + customV.x, origV.y + customV.y);
                }
                break;
            
            case AttributeConfig.CopyNewTransformThenOffsetWithCustom:
                {
                    Vector2 newV = 
                        newTransform.GetComponent<Rigidbody2D>().velocity;
                    obj.GetComponent<Rigidbody2D>().velocity = new Vector2(
                        newV.x + customV.x, newV.y + customV.y);
                }
                break;

            case AttributeConfig.UseCustom:
                obj.GetComponent<Rigidbody2D>().velocity = new Vector2(
                    customV.x, customV.y);
                break;
        }

        if (customVelocity.rotateFromRotation)
        {
            obj.GetComponent<Rigidbody2D>().velocity =
                obj.GetComponent<Rigidbody2D>().velocity.Rotate(
                obj.transform.localEulerAngles.z);
        }
    }

    /// <summary>
    /// Configures the scale.
    /// </summary>
    /// <param name="obj">Object.</param>
    void ConfigureScale(GameObject obj)
    {
        Vector2 customS = customScale.custom;
        switch (customScale.scaleConfig)
        {
            case AttributeConfig.OffsetWithCustom:
                {
                    Vector2 origS = obj.transform.localScale;
                    obj.transform.localScale = new Vector3(
                        origS.x * customS.x, origS.y * customS.y, 1f);
                }
                break;

            case AttributeConfig.CopyNewTransformThenOffsetWithCustom:
                Vector2 newScale = newTransform.localScale;
                obj.transform.localScale = new Vector3(
                    newScale.x * customS.x, newScale.y * customS.y, 1f);
                break;

            case AttributeConfig.UseCustom:
                obj.transform.localScale = new Vector3(
                    customS.x, customS.y, 1f);
                break;
        }
    }

    /// <summary>
    /// Sets the new tag.
    /// </summary>
    /// <param name="obj">Object.</param>
    void SetNewTag(GameObject obj)
    {
        if (newTag.Name != "")
        {
            obj.tag = newTag.Name;
        }
    }

    /// <summary>
    /// Sets the new parent.
    /// </summary>
    /// <param name="obj">Object.</param>
    void SetNewParent(GameObject obj)
    {
        if (newParent == null)
        {
            // Set the parent to the _Dynamic object by default
            newParent = GameObject.FindGameObjectWithTag(
                Tags.dynamicObjects).transform;
        }

        obj.transform.parent = newParent;
    }

    /// <summary>
    /// Does the final transform action.
    /// </summary>
    void DoFinalTransformAction()
    {
        if (newTransform == null)
        {
            return;
        }

        //Perform the new transform action
        switch (transformOnCreate)
        {
            case ActionOnComplete.SetToInactive:
                newTransform.gameObject.SetActive(false);
                break;

            case ActionOnComplete.Destroy:
                UnityEngine.Object.Destroy(newTransform.gameObject);
                break;
        }
    }

    public void Validate()
    {
        if (newTransform == null)
        {
            transformOnCreate = ActionOnComplete.Nothing;

            switch (customScale.scaleConfig)
            {
                case AttributeConfig.CopyNewTransformThenOffsetWithCustom:
                    customScale.scaleConfig = AttributeConfig.OffsetWithCustom;
                    break;
            }

            switch (customRotation.rotationConfig)
            {
                case AttributeConfig.CopyNewTransformThenOffsetWithCustom:
                    customRotation.rotationConfig = AttributeConfig.OffsetWithCustom;
                    break;
            }
        }
    }

    [Serializable]
    public class CustomPosition : System.Object
    {
        [Tooltip("Offset With Custom: Multiplies the original scale with the"
            + " custom scale\n\n Use Custom: Just outright replaces the"
            + "scale with the custom")]
        public PositionConfig positionConfig;
        public Vector3 custom;
    }

    [Serializable]
    public class CustomScale : System.Object
    {
        [Tooltip("None: Does Nothing\n\n" +
            "Offset With Custom: Multiplies the original scale with the custom"
            + " scale\n\n Copy New Transform Then Offset With Custom: Copies "
            + "the scale of the new Transform then multiplies it with the"
            + "custom \n\nUse Custom: Just outright replaces the scale with "
            + "the custom")]
        public AttributeConfig scaleConfig;
        public Vector2 custom;
    }

    [Serializable]
    public class CustomRotation : System.Object
    {
        [Tooltip("None: Does Nothing\n\n" +
            "Offset With Custom: Adds the original rotation with the custom "
            + "rotation\n\n Copy New Transform Then Offset With Custom: Copies"
            + " the rotation of the new Transform then adds it with the custom"
            + "\n\nUse Custom: Just outright replaces the rotation with "
            + "the custom")]
        public AttributeConfig rotationConfig;
        [Range(0f, 360f)]
        public float custom;
    }

    [Serializable]
    public class CustomVelocity : System.Object
    {
        [Tooltip("None: Does Nothing To The Velocity\n\n" +
            "Offset With Custom: Adds the original velocity with the custom "
            + "velocity\n\n Copy New Transform Then Offset With Custom: Copies"
            + " the velocity of the new Transform then adds it with the custom"
            + "\n\nUse Custom: Just outright replaces the velocity with "
            + "the custom")]
        public AttributeConfig velocityConfig;
        public Vector2 custom;
        public bool rotateFromRotation = true;
    }
}