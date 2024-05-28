using UnityEngine;

public class CharacterTableTest : MonoBehaviour
{
    public CharacterTable characterTable;
    void Start()
    {
        characterTable = DataTableMgr.Get<CharacterTable>(DataTableIds.Character);
    }
}
