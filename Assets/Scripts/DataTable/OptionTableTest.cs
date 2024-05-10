using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionTableTest : MonoBehaviour
{
    public OptionTable optionTable = new OptionTable();
    void Start()
    {
        optionTable = DataTableMgr.Get<OptionTable>(DataTableIds.Option);
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            Debug.Log(optionTable.Get(110000));
        }
    }
}
