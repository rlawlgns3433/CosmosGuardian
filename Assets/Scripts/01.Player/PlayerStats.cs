using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    private CharacterTable characterTable;
    private Dictionary<CharacterColumn.Stat, float> stats; // �ɷ�ġ ����, �ɷ�ġ ����

    private void Awake()
    {
        characterTable = DataTableMgr.Get<CharacterTable>(DataTableIds.Character);
    }
}
