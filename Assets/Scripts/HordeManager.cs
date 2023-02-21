using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HordeManager : MonoBehaviour
{
    [SerializeField]private TardigradeBase[] prefabs;
    [SerializeField]private List <TardigradeBase> selectedTards;
    private Elem selectedType;
    private List<TardigradeBase> allTards;


    private void Start()
    {
        allTards = GetComponentsInChildren<TardigradeBase>().ToList();
    }

    /// <summary>
    /// Highlights a group of Tardigrades based on type
    /// </summary>
    /// <param name="tardType">The elemental type of group that should be selected</param>
    public void SelectGroup(Elem tardType)
    {
        selectedType = tardType;
        Material mat;
        //Turns old highlights off and clears the old selection
        foreach (TardigradeBase obj in selectedTards)
        {
            mat = obj.GetComponent<Renderer>().material;
            mat.SetFloat("_Highlight_Thickness", 0);
        }
        selectedTards.Clear();
        
        //Grabs new selection and Highlights them
        foreach (TardigradeBase tard in allTards)
        {
            if (tard.GetElementType() == tardType)
            {
                selectedTards.Add(tard);
                mat = tard.GetComponent<Renderer>().material;
                if (mat.name != "HighlightMat (Instance)") print(tard + " needs to have highlightmat as the first material");
                mat.SetFloat("_Highlight_Thickness", 0.1f);
            }
        }
    }

    public void SpreadMutation()
    {
        //Turn list of tards into a list of their positions then take the average to find the middle of the group;
        List<Vector3> transforms = selectedTards.Select(go => go.transform.position).ToList();
        Vector3 middleOfGroup = transforms.Aggregate(new Vector3(0,0,0), (s,v) => s + v) / transforms.Count;

        float minDistance = Single.PositiveInfinity;
        TardigradeBase closestTard = null;
        foreach (TardigradeBase tard in allTards)
        {
            float distance = Vector3.Distance(tard.transform.position, middleOfGroup);
            if (tard.GetElementType() == Elem.Neutral && distance < minDistance)
            {
                closestTard = tard;
                minDistance = distance;
            }
        }
        if(closestTard == null) print("Hey There are no neutral tards to transform");
        else Mutate(closestTard);
    }
    
    private void Mutate(TardigradeBase tard)
    {
        //get old tards stats like hp and position
        Transform trans = tard.transform;
        float oldHealth = tard.health;
        //destroy the old tard
        allTards.Remove(tard);
        Destroy(tard.gameObject);
        //instatiate new one in its place
        foreach (TardigradeBase obj in prefabs)
        {
            if (obj.GetElementType() == selectedType)
            {
                TardigradeBase newTard = Instantiate(obj, trans.position, trans.rotation);
                newTard.health = oldHealth;
                allTards.Add(newTard);
            }
        }
        SelectGroup(selectedType);
    }
}
