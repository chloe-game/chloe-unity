using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Defines an instance of a resource object.
/// This will contain the resource properties (PropertySO) and their values,
/// as well as any state information when it is launched.
/// </summary>
public class ResourceInstance : MonoBehaviour
{
    #region Variables
    /// <summary>
    /// The ScriptableObject instance for this resource type.
    /// </summary>
    private ResourceSO resource;

    /// <summary>
    /// The list of property values for this resource instance.
    /// </summary>
    private List<IPropertyValue> propertyValues = new List<IPropertyValue>();

    /// <summary>
    /// True if the resource has been created in AWS.
    /// </summary>
    private bool isLaunched = false;
    #endregion

    #region Properties
    public ResourceSO Resource
    {
        get { return this.resource; }
    }

    public List<IPropertyValue> PropertyValues
    {
        get { return this.propertyValues; }
    }

    public bool IsLaunched
    {
        get { return this.isLaunched; }
    }
    #endregion

    #region Constructors
    public void Initialize(ResourceSO resourceSO)
    {
        // Add this component to the resource GameObject
        this.resource = resourceSO;

        // Populate property value list with supported properties for this resource.
        foreach (PropertySO propertySO in resourceSO.Properties)
        {
            IPropertyValue propertyValue;
            Type propertySelectType;
            Type specificType;

            Debug.Log($"Prop: {propertySO.ShortName}");

            switch (propertySO.PropertyType)
            {
                case PropertyTypes.SINGLE_LINE:
                    propertyValue = new PropertySingleLine();
                    break;
                case PropertyTypes.MULTI_LINE:
                    propertyValue = new PropertyMultiLine();
                    break;
                case PropertyTypes.BOOLEAN:
                    propertyValue = new PropertyBoolean();
                    break;
                case PropertyTypes.SELECT_ONE:
                    propertySelectType = typeof(PropertySelectOne<>);
                    specificType = propertySelectType.MakeGenericType(propertySO.GetListSource().ItemType());
                    propertyValue = (IPropertyValue)Activator.CreateInstance(specificType);
                    break;
                case PropertyTypes.SELECT_MANY:
                    propertySelectType = typeof(PropertySelectMany<>);
                    specificType = propertySelectType.MakeGenericType(propertySO.GetListSource().ItemType());
                    propertyValue = (IPropertyValue)Activator.CreateInstance(specificType);
                    break;
                case PropertyTypes.NESTED:
                    propertySelectType = typeof(PropertyNested<>);
                    specificType = propertySelectType.MakeGenericType(propertySO.GetListSource().ItemType());
                    propertyValue = (IPropertyValue)Activator.CreateInstance(specificType);
                    break;
                case PropertyTypes.EDITABLE_LIST:
                    propertySelectType = typeof(PropertyEditableList<>);
                    specificType = propertySelectType.MakeGenericType(propertySO.GetNestedSource().ItemType());
                    propertyValue = (IPropertyValue)Activator.CreateInstance(specificType);
                    break;
                default:
                    Debug.LogError($"Unsupported property value type in resource setup: {propertySO.ShortName}");
                    propertyValue = new PropertySingleLine();
                    break;
            }

            propertyValue.Initialize(propertySO, this);
            this.propertyValues.Add(propertyValue);
        }
    }
    #endregion

    #region Unity
    #endregion
}
